using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Camera
    [Header("카메라 설정")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float lookSensitivity; // 카메라 민감도
    [SerializeField] private float cameraRotationLimit; // 카메라 이동 각도 제한 범위
    [SerializeField] private float currentCameraRotationX = 0f; // 머리의 움직임 X축 회전
    #endregion

    #region Player
    [Header("플레이어 설정")]
    private float applySpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchSpeed;

    [SerializeField] private float crouchPosY; // 얼마나 깊게 앉을지
    private float originPosY; // 원래 기본 높이
    private float applyCrouchPosY;


    private CapsuleCollider capsuleCollider; // 착지 여부
    private Rigidbody myRigidbody;

    #endregion

    #region Bool
    private bool isWalk = false;
    private bool isCrouch = false;
    private bool isRun = false;
    private bool isTouchingGround = true;
    #endregion

    private GunController gunController;
    private Crosshair crosshair;
    private StatusController statusController;

    // 임시 실행 bgm
    [SerializeField] private string bgm_Sound;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        gunController = FindObjectOfType<GunController>();
        crosshair = FindObjectOfType<Crosshair>();
        statusController = FindObjectOfType<StatusController>();

        SoundManager.soundManager.PlayBGM(bgm_Sound);

        applySpeed = walkSpeed;
        applyCrouchPosY = originPosY;
        originPosY = mainCamera.transform.localPosition.y;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        #region Movement
        IsTouchingGround();
        TryJump();
        TryRun();
        TryCrouch();
        MoveCheck();
        #endregion

        #region Camera
        if (!Inventory.inventoryActivated)
        {
            CameraRotation();
            CharacterRotation();
        }
        #endregion
    }

    #region Crouch
    /// <summary>
    ///  CTRL키를 눌렀을 때 앉기 기능 제어
    /// </summary>
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        crosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
        //mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x, applyCrouchPosY, mainCamera.transform.localPosition.z);
    }

    IEnumerator CrouchCoroutine()
    {
        float posY = mainCamera.transform.localPosition.y;
        int count = 0;

        while(posY != applyCrouchPosY)
        {
            count++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.2f);
            mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x, posY, mainCamera.transform.localPosition.z);
            if (count > 15)
                break;
            yield return null;
        }
        mainCamera.transform.localPosition = new Vector3(0f, applyCrouchPosY, 0f);
    }
    #endregion

    #region Jump
    /// <summary>
    /// Jump는 플레이어가 땅에 착지된상태 (캡슐콜라이더가 지면과 닿아있을때) 만 가능하게 제어
    /// </summary>
    private void IsTouchingGround()
    {
        // (플레이어의 위치, 방향, 거리)
        // Vector3.down => 고정된 위치에서 무조건 하단을 가르켜야 하기 때문에
        // capsuleCollider.bounds.extents.y => 캡슐콜라이더의 영역의 y값의 절반
        isTouchingGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);

        
        crosshair.JumpingAnimation(!isTouchingGround);
    }

    /// <summary>
    /// Space키를 눌렀을 때 플레이어 점프
    /// </summary>
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isTouchingGround && statusController.GetCurrentSP() > 0)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // 앉은 상태에서 점프시 앉은 상태 해제
        if (isCrouch)
            Crouch();
        statusController.DecreaseStamina(400);
        myRigidbody.velocity = transform.up * jumpForce; 
    }
    #endregion

    #region Run
    /// <summary>
    /// Shift키를 눌렀을 때 플레이어 달리기
    /// </summary>
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && statusController.GetCurrentSP() > 0)
        {
            Running();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || statusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        // 앉은 상태에서 달릴시 앉은 상태 해제
        if (isCrouch)
            Crouch();

        gunController.CancelFineSight();

        isRun = true;

        crosshair.RunningAnimation(isRun);
        statusController.DecreaseStamina(2);
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        crosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }
    #endregion

    #region Move
    /// <summary>
    /// 플레이어의 움직임 제어
    /// </summary>
    private void Move()
    {
        // 오른쪽 1, 왼쪽 -1, 누르지 않으면 0
        float moveDirX = Input.GetAxisRaw("Horizontal");

        // 3D에서는 정면과 뒤가 Z값
        // 정면 1 , 뒤 -1, 누르지 않으면 0
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        // 정규화를 시켜 합이 1이 되도록
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        // Time.deltaTime 이 없을 경우 순간이동을 함
        // 스르륵 움직이게 하기 위함
        myRigidbody.MovePosition(transform.position + velocity * Time.deltaTime);

        isWalk = moveDirX != 0 || moveDirZ != 0;
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isTouchingGround)
        {
            if (!isWalk)
            {
                // 움직이지 않을 때의 처리
                isWalk = false;
            }
            crosshair.WalkingAnimation(isWalk);
            
        }
    }
    #endregion

    #region Camera
    /// <summary>
    /// 플레이어의 마우스에 따른 카메라 상하 움직임 제어
    /// </summary>
    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity; // 민감도를 설정해서 자연스러운 움직임

        // += (마우스 반전), -= (주관적 기본 상태)
        currentCameraRotationX -= cameraRotationX;

        // 카메라 Y축 움직임을 일정 각도 이상,이하 넘어가지 않도록 제한
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        mainCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    /// <summary>
    /// 플레이어의 마우스에 따른 카메라 좌우 움직임 제어
    /// </summary>
    private void CharacterRotation()
    {
        // 좌우 캐릭터 회전 
        // Y축 회전
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;

        // Euler 값을 Quaternion으로 변환해서 값을 곱해줌
        myRigidbody.MoveRotation(myRigidbody.rotation * Quaternion.Euler(characterRotationY));
    }
    #endregion
}
