using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    protected StatusController statusController;

    [SerializeField] public string animalName;
    [SerializeField] protected int hp;
    protected NavMeshAgent nav;

    [SerializeField] protected Item item_Prefab; // 드랍 아이템
    [SerializeField] public int itemNumber; // 아이템의 획득 갯수

    // 이동 속도
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    protected Vector3 destination;

    // bool형 상태변수
    protected bool isWalking;
    protected bool isAction; // 행동중인지 아닌지
    protected bool isRunning;
    protected bool isChasing; // 추격중인지 판별
    protected bool isAttacking; // 공격중인지 판별
    public bool isDead;

    // 상태 적용 시간
    [SerializeField] protected float walkTime;
    [SerializeField] protected float idleTime;
    [SerializeField] protected float runTime;
    protected float currentTime;

    // 컴포넌트
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody myRigidbody;
    [SerializeField] protected BoxCollider boxCollider;
    protected FieldOfViewAngle fieldOfViewAngle;

    // 타격 이펙트
    [SerializeField] protected Material material;
    [SerializeField] protected Color hitColor;
    [SerializeField] protected Color baseColor;

    // 사운드
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip[] audioClips_normal;
    [SerializeField] protected AudioClip audioClip_hurt;
    [SerializeField] protected AudioClip audioClip_dead;


    private void Start()
    {
        currentTime = idleTime;
        isAction = true;

        statusController = FindObjectOfType<StatusController>();
        nav = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        fieldOfViewAngle = GetComponent<FieldOfViewAngle>();
    }

    protected virtual void Update()
    {
        if (!isDead)
        {
            Move();
            /*Rotation();*/
            ElapseTime();
        }
    }

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            //myRigidbody.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
            nav.SetDestination(transform.position + destination * 5f);
        }
    }

/*    protected void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 rotation;

            if (isWalking)
            {
                rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), turningSpeed);
                myRigidbody.MoveRotation(Quaternion.Euler(rotation));
            }
            else if (isRunning)
            {
                rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.05f);
                myRigidbody.MoveRotation(Quaternion.Euler(rotation));
            }


        }
    }*/

    // 시간 경과
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && !isAttacking)
            {
                // 다음 랜덤 행동 개시
                ResetAction();
            }
        }
    }

    protected virtual void ResetAction()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;

        nav.speed = walkSpeed;
        nav.ResetPath();

        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        destination.Set(Random.Range(-0.2f, 0.2f), 0f, Random.Range(0.5f, 1f));
    }

    public virtual void Damage(int damage, Vector3 targetPos)
    {
        if (!isDead)
        {
            hp -= damage;
            StartCoroutine(ChangeColor());

            if (hp <= 0)
            {
                hp = 0;

                Dead();

                return;
            }

            PlaySE(audioClip_hurt);

            anim.SetTrigger("Hurt");
        }
    }

    private IEnumerator ChangeColor()
    {
        material.color = hitColor;
        yield return new WaitForSeconds(0.8f);

        material.color = baseColor;
    }

    protected void Dead()
    {
        PlaySE(audioClip_dead);

        isWalking = false;
        isRunning = false;
        isChasing = false;
        isAttacking = false;
        nav.ResetPath();
        isDead = true;

        /*myRigidbody.isKinematic = true;
        boxCollider.enabled = false;*/

        anim.SetTrigger("Dead");
    }

    protected void RandomSound()
    {
        // normal 사운드 3개
        int random = Random.Range(0, 3);
        PlaySE(audioClips_normal[random]);
    }

    protected void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


}
