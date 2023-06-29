using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] private string animalName;
    [SerializeField] private int hp;

    // �̵� �ӵ�
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;

    private Vector3 direction; 

    // bool�� ���º���
    private bool isWalking;
    private bool isAction;
    private bool isRunning;
    private bool isDead;

    // ���� ���� �ð�
    [SerializeField] private float walkTime;
    [SerializeField] private float idleTime;
    [SerializeField] private float runTime;
    private float currentTime;

    // ������Ʈ
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody pig_rigidbody;
    [SerializeField] private BoxCollider boxCollider;

    // Ÿ�� ����Ʈ
    [SerializeField] private Material material;
    [SerializeField] private Color hitColor;
    [SerializeField] private Color baseColor;

    // ����
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] pig_audioClips_normal;
    [SerializeField] private AudioClip pig_audioClip_hurt;
    [SerializeField] private AudioClip pig_audioClip_dead;

    private void Start()
    {
        currentTime = idleTime;
        isAction = true;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isDead)
        {
            Move();
            Rotation();
            ElapseTime();
        }
    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            pig_rigidbody.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        }
    }

    private void Rotation()
    {
        if (isWalking || isRunning)
        {
            Vector3 rotation;

            if (isWalking)
            {
                rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
                pig_rigidbody.MoveRotation(Quaternion.Euler(rotation));
            }
            else if (isRunning)
            {
                rotation = Vector3.Lerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.05f);
                pig_rigidbody.MoveRotation(Quaternion.Euler(rotation));
            }

            
        }
    }
    
    // �ð� ���
    private void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <= 0)
            {
                // ���� ���� �ൿ ����
                ResetAction();
            }
        }
    }

    private void RandomAction()
    {
        RandomSound();
        isAction = true;

        // ���, Ǯ���, �θ���, �ȱ�
        int random = Random.Range(0, 4); // �ּ� �� ���� �ִ밪 ������

        Pig_RandomAction(random);
    }

    private void Pig_RandomAction(int random)
    {
        if(random < 3)
        {
            currentTime = idleTime;
        }
        else if(random >= 3)
        {
            currentTime = walkTime; 
        }

        if (random == 0)
        {      
            Debug.Log("���");
        }
        else if (random == 1)
        {
            Debug.Log("Ǯ���");
            anim.SetTrigger("Eat");
        }
        else if (random == 2)
        {
            Debug.Log("�θ���");
            anim.SetTrigger("Peek");
        }
        else if (random == 3)
        {
            isWalking = true;
            Debug.Log("�ȱ�");
            applySpeed = walkSpeed;
            anim.SetBool("Walking", isWalking);
            
        }
    }

    private void Run(Vector3 targetPos)
    {
        applySpeed = runSpeed;

        direction = Quaternion.LookRotation(transform.position - targetPos).eulerAngles;
        currentTime = runTime;

        isWalking = false;
        isRunning = true;

        anim.SetBool("Running", isRunning);
    }

    private void ResetAction()
    {
        isWalking = false;
        isRunning = false;
        isAction = true;

        applySpeed = walkSpeed;

        anim.SetBool("Walking", isWalking);
        anim.SetBool("Running", isRunning);
        direction.Set(0f, Random.Range(0f, 360f), 0f);

        RandomAction();
    }

    public void Damage(int damage, Vector3 targetPos)
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

            PlaySE(pig_audioClip_hurt);

            anim.SetTrigger("Hurt");
            Run(targetPos);
        }     
    }

    private IEnumerator ChangeColor()
    {
        material.color = hitColor;
        yield return new WaitForSeconds(0.8f);

        material.color = baseColor;
    }

    private void Dead()
    {
        PlaySE(pig_audioClip_dead);

        isWalking = false;
        isRunning = false;
        isDead = true;

        pig_rigidbody.isKinematic = true;
        boxCollider.enabled = false;

        anim.SetTrigger("Dead");
    }

    private void RandomSound()
    {
        // normal ���� 3��
        int random = Random.Range(0, 3);
        PlaySE(pig_audioClips_normal[random]);
    }

    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
