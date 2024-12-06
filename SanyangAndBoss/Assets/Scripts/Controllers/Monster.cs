using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED
    }

    [Header("Monster Settings")]
    public float maxHP = 10f;
    public float lostDistance = 10f;
    public float attackDistance = 1f;

    [Header("Attack Settings")]
    public float attackCooldown = 2.0f; // ���� �� �ּ� �ð�
    private float lastAttackTime; // ������ ���� �ð�

    private float currentHP;
    private NavMeshAgent nmAgent;
    private Animator anim;
    private Transform target;

    public GameObject hpBar;
    public State state { get; private set; }
    public bool IsAlive => state != State.KILLED;

    private Rigidbody rb; // ������ٵ�

    [Header("Audio Settings")]
    public AudioClip dieSound; // �״� ȿ����
    public AudioClip attackSound;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        currentHP = maxHP;
        state = State.IDLE;

        MonsterManager.Instance.monsters.Add(this);

        if (hpBar != null)
        {
            Slider slider = hpBar.GetComponent<Slider>();
            if (slider != null)
                slider.value = 1;
        }

        // Rigidbody ����
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        // AudioSource ����
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    //void OnDestroy()
    //{
    //    MonsterManager.Instance.monsters.Remove(this);
    //}

    public void ChangeState(State newState)
    {
        if (state == State.KILLED) return;
        if (state == newState) return;

        state = newState;

        switch (state)
        {
            case State.IDLE:
                StopMoving();
                PlayAnimation("IdleNormal");
                break;

            case State.CHASE:
                StartChasing();
                PlayAnimation("WalkFWD");
                break;

            case State.ATTACK:
                StopMoving();
                StartCoroutine(ATTACK());
                break;

            case State.KILLED:
                Die();
                break;
        }
    }

    private void StartChasing()
    {
        if (MonsterManager.Instance.player != null)
        {
            target = MonsterManager.Instance.player;
        }
    }

    private void StopMoving()
    {
        nmAgent.SetDestination(transform.position);
    }

    IEnumerator ATTACK()
    {
        while (true)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= attackDistance)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    PlayAnimation("Attack01");
                    lastAttackTime = Time.time;

                    // ���� ȿ���� ���
                    if (audioSource != null && attackSound != null)
                    {
                        audioSource.clip = attackSound;
                        audioSource.Play();
                    }

                    PlayerController player = target.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.TakeDamage(5.0f);
                        Debug.Log("�÷��̾ �����߽��ϴ�!");
                    }
                }
            }
            else
            {
                ChangeState(State.CHASE);
                yield break;
            }

            yield return null;
        }
    }

    private void PlayAnimation(string animationName)
    {
        anim.Play(animationName, 0, 0);
    }

    private void Die()
    {
        PlayAnimation("Die");

        // ȿ���� ���
        if (audioSource != null && dieSound != null)
        {
            audioSource.clip = dieSound;
            audioSource.Play();
        }

        // ���� ��� �� MonsterKillManager�� �˸�
        if (MonsterKillManager.Instance != null)
        {
            MonsterKillManager.Instance.MonsterKilled();
        }
        Destroy(gameObject, 2f);
    }

    public void TakeDamage(float damage)
    {
        if (state == State.KILLED) return;

        currentHP -= damage;
        Debug.Log($"���� ü��: {currentHP}/{maxHP}");
        UpdateHPBar();

        if (currentHP <= 0)
        {
            currentHP = 0;
            ChangeState(State.KILLED);
        }
    }

    private void UpdateHPBar()
    {
        if (hpBar != null)
        {
            Slider slider = hpBar.GetComponent<Slider>();
            if (slider != null)
            {
                slider.value = currentHP / maxHP;
            }
        }
    }

    void Update()
    {
        if (state == State.CHASE && target != null)
        {
            nmAgent.SetDestination(target.position); // �÷��̾� ��ġ ���������� ����
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� �浹!");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(5.0f); // �浹 �� ������
            }
        }
    }
}