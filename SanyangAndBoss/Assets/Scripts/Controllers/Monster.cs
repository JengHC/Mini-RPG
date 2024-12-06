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
    public float attackCooldown = 2.0f; // 공격 간 최소 시간
    private float lastAttackTime; // 마지막 공격 시간

    private float currentHP;
    private NavMeshAgent nmAgent;
    private Animator anim;
    private Transform target;

    public GameObject hpBar;
    public State state { get; private set; }
    public bool IsAlive => state != State.KILLED;

    private Rigidbody rb; // 리지드바디

    [Header("Audio Settings")]
    public AudioClip dieSound; // 죽는 효과음
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

        // Rigidbody 설정
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        // AudioSource 설정
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

                    // 공격 효과음 재생
                    if (audioSource != null && attackSound != null)
                    {
                        audioSource.clip = attackSound;
                        audioSource.Play();
                    }

                    PlayerController player = target.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        player.TakeDamage(5.0f);
                        Debug.Log("플레이어를 공격했습니다!");
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

        // 효과음 재생
        if (audioSource != null && dieSound != null)
        {
            audioSource.clip = dieSound;
            audioSource.Play();
        }

        // 몬스터 사망 시 MonsterKillManager에 알림
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
        Debug.Log($"몬스터 체력: {currentHP}/{maxHP}");
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
            nmAgent.SetDestination(target.position); // 플레이어 위치 지속적으로 갱신
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어와 충돌!");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(5.0f); // 충돌 시 데미지
            }
        }
    }
}