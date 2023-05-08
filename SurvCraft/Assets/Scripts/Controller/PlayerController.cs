using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Camera
    [Header("ī�޶� ����")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float lookSensitivity; // ī�޶� �ΰ���
    [SerializeField] private float cameraRotationLimit; // ī�޶� �̵� ���� ���� ����
    [SerializeField] private float currentCameraRotationX = 0f; // �Ӹ��� ������ X�� ȸ��
    #endregion

    #region Player
    [Header("�÷��̾� ����")]
    private float applySpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float crouchSpeed;

    [SerializeField] private float crouchPosY; // �󸶳� ���� ������
    private float originPosY; // ���� �⺻ ����
    private float applyCrouchPosY;


    private CapsuleCollider capsuleCollider; // ���� ����
    private Rigidbody myRigidbody;
    #endregion

    #region Bool
    private bool isCrouch = false;
    private bool isRun = false;
    private bool isTouchingGround = true;
    #endregion

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigidbody = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = mainCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        #region Movement
        IsTouchingGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        #endregion

        #region Camera
        CameraRotation();
        CharacterRotation();
        #endregion
    }

    #region Crouch
    /// <summary>
    ///  CTRLŰ�� ������ �� �ɱ� ��� ����
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
    /// Jump�� �÷��̾ ���� �����Ȼ��� (ĸ���ݶ��̴��� ����� ���������) �� �����ϰ� ����
    /// </summary>
    private void IsTouchingGround()
    {
        // (�÷��̾��� ��ġ, ����, �Ÿ�)
        // Vector3.down => ������ ��ġ���� ������ �ϴ��� �����Ѿ� �ϱ� ������
        // capsuleCollider.bounds.extents.y => ĸ���ݶ��̴��� ������ y���� ����
        isTouchingGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    /// <summary>
    /// SpaceŰ�� ������ �� �÷��̾� ����
    /// </summary>
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isTouchingGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // ���� ���¿��� ������ ���� ���� ����
        if (isCrouch)
            Crouch();

        myRigidbody.velocity = transform.up * jumpForce; 
    }
    #endregion

    #region Run
    /// <summary>
    /// ShiftŰ�� ������ �� �÷��̾� �޸���
    /// </summary>
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        // ���� ���¿��� �޸��� ���� ���� ����
        if (isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }
    #endregion

    #region Move
    /// <summary>
    /// �÷��̾��� ������ ����
    /// </summary>
    private void Move()
    {
        // ������ 1, ���� -1, ������ ������ 0
        float moveDirX = Input.GetAxisRaw("Horizontal");

        // 3D������ ����� �ڰ� Z��
        // ���� 1 , �� -1, ������ ������ 0
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        // ����ȭ�� ���� ���� 1�� �ǵ���
        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        // Time.deltaTime �� ���� ��� �����̵��� ��
        // ������ �����̰� �ϱ� ����
        myRigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
    }
    #endregion

    #region Camera
    /// <summary>
    /// �÷��̾��� ���콺�� ���� ī�޶� ���� ������ ����
    /// </summary>
    private void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity; // �ΰ����� �����ؼ� �ڿ������� ������

        // += (���콺 ����), -= (�ְ��� �⺻ ����)
        currentCameraRotationX -= cameraRotationX;

        // ī�޶� Y�� �������� ���� ���� �̻�,���� �Ѿ�� �ʵ��� ����
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        mainCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    /// <summary>
    /// �÷��̾��� ���콺�� ���� ī�޶� �¿� ������ ����
    /// </summary>
    private void CharacterRotation()
    {
        // �¿� ĳ���� ȸ�� 
        // Y�� ȸ��
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivity;

        // Euler ���� Quaternion���� ��ȯ�ؼ� ���� ������
        myRigidbody.MoveRotation(myRigidbody.rotation * Quaternion.Euler(characterRotationY));
    }
    #endregion
}