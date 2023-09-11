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

    [SerializeField] protected Item item_Prefab; // ��� ������
    [SerializeField] public int itemNumber; // �������� ȹ�� ����

    // �̵� �ӵ�
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;

    protected Vector3 destination;

    // bool�� ���º���
    protected bool isWalking;
    protected bool isAction; // �ൿ������ �ƴ���
    protected bool isRunning;
    protected bool isChasing; // �߰������� �Ǻ�
    protected bool isAttacking; // ���������� �Ǻ�
    public bool isDead;

    // ���� ���� �ð�
    [SerializeField] protected float walkTime;
    [SerializeField] protected float idleTime;
    [SerializeField] protected float runTime;
    protected float currentTime;

    // ������Ʈ
    [SerializeField] protected Animator anim;
    [SerializeField] protected Rigidbody myRigidbody;
    [SerializeField] protected BoxCollider boxCollider;
    protected FieldOfViewAngle fieldOfViewAngle;

    // Ÿ�� ����Ʈ
    [SerializeField] protected Material material;
    [SerializeField] protected Color hitColor;
    [SerializeField] protected Color baseColor;

    // ����
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

    // �ð� ���
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0 && !isChasing && !isAttacking)
            {
                // ���� ���� �ൿ ����
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
        // normal ���� 3��
        int random = Random.Range(0, 3);
        PlaySE(audioClips_normal[random]);
    }

    protected void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }


}
