using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    Transform target;
    NavMeshAgent nmAgent;
    Animator anim;

    [SerializeField] private float maxHP = 10; // 최대 체력
    private float currentHP; // 현재 체력
    public GameObject hpBar;
    public float lostDistance;

    enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED
    }

    State state;

    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();

        currentHP = maxHP;
        state = State.IDLE;

        if (hpBar != null)
        {
            Slider slider = hpBar.GetComponent<Slider>();
            if (slider != null)
                slider.value = 1; // 체력 비율 초기화
        }

        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (state != State.KILLED)
        {
            yield return StartCoroutine(state.ToString());
        }

        StartCoroutine(KILLED());
    }

    IEnumerator IDLE()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!curAnimStateInfo.IsName("IdleNormal"))
        {
            anim.Play("IdleNormal", 0, 0);
        }

        int dir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
        float lookSpeed = Random.Range(25f, 40f);

        for (float i = 0; i < curAnimStateInfo.length; i += Time.deltaTime)
        {
            transform.localEulerAngles += new Vector3(0f, dir * Time.deltaTime * lookSpeed, 0f);
            yield return null;
        }
    }

    IEnumerator CHASE()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (!curAnimStateInfo.IsName("WalkFWD"))
        {
            anim.Play("WalkFWD", 0, 0);
            yield return null;
        }

        if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
        {
            ChangeState(State.ATTACK);
        }
        else if (nmAgent.remainingDistance > lostDistance)
        {
            target = null;
            nmAgent.SetDestination(transform.position);
            ChangeState(State.IDLE);
        }
        else
        {
            yield return new WaitForSeconds(curAnimStateInfo.length);
        }
    }

    IEnumerator ATTACK()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        anim.Play("Attack01", 0, 0);

        if (target != null && target.CompareTag("Player"))
        {
            PlayerController player = target.GetComponent<PlayerController>();
            if (player != null)
            {
                float damage = 5.0f;
                player.TakeDamage(damage);
                Debug.Log($"플레이어에게 {damage} 데미지를 줌");
            }
        }

        if (nmAgent.remainingDistance > nmAgent.stoppingDistance)
        {
            ChangeState(State.CHASE);
        }
        else
        {
            yield return new WaitForSeconds(curAnimStateInfo.length * 2f);
        }
    }

    IEnumerator KILLED()
    {
        anim.Play("Die", 0, 0);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    void ChangeState(State newState)
    {
        if (state == State.KILLED) return;
        state = newState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state == State.KILLED) return;

        if (other.CompareTag("Player"))
        {
            target = other.transform;
            nmAgent.SetDestination(target.position);
            ChangeState(State.CHASE);
        }
        else if (other.CompareTag("Weapon"))
        {
            WeaponController weapon = other.GetComponentInParent<WeaponController>();
            if (weapon != null && weapon.enabled)
            {
                TakeDamage(weapon.damage);
            }
        }
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

    void UpdateHPBar()
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
        if (state == State.KILLED) return;

        if (target != null)
        {
            nmAgent.SetDestination(target.position);
        }
    }
}