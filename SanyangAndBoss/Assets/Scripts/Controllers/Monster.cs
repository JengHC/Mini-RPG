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

    [SerializeField] private float maxHP = 10; // �ִ� ü��
    private float currentHP; // ���� ü��
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

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        nmAgent = GetComponent<NavMeshAgent>();

        currentHP = maxHP;
        state = State.IDLE;
        StartCoroutine(StateMachine());

        UpdateHPBar();
    }

    IEnumerator StateMachine()
    {
        while (currentHP > 0)
        {
            yield return StartCoroutine(state.ToString());
        }
        // ü���� 0�� �Ǿ��� �� ��� ���·� ��ȯ
        ChangeState(State.KILLED);
        StartCoroutine(KILLED());
    }

    IEnumerator IDLE()
    {
        // ���� animator �������� ���
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // �ִϸ��̼� �̸��� IdleNormal �� �ƴϸ� Play
        if (curAnimStateInfo.IsName("IdleNormal") == false)
            anim.Play("IdleNormal", 0, 0);

        // ���Ͱ� Idle ������ �� �θ��� �Ÿ��� �ϴ� �ڵ�
        // 50% Ȯ���� ��/��� ���� ����
        int dir = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

        // ȸ�� �ӵ� ����
        float lookSpeed = Random.Range(25f, 40f);

        // IdleNormal ��� �ð� ���� ���ƺ���
        for (float i = 0; i < curAnimStateInfo.length; i += Time.deltaTime)
        {
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y + (dir) * Time.deltaTime * lookSpeed, 0f);

            yield return null;
        }
    }

    IEnumerator CHASE()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if (curAnimStateInfo.IsName("WalkFWD") == false)
        {
            anim.Play("WalkFWD", 0, 0);
            // SetDestination �� ���� �� frame�� �ѱ������ �ڵ�
            yield return null;
        }

        // ��ǥ������ ���� �Ÿ��� ���ߴ� �������� �۰ų� ������
        if (nmAgent.remainingDistance <= nmAgent.stoppingDistance)
        {
            // StateMachine �� �������� ����
            ChangeState(State.ATTACK);
        }
        // ��ǥ���� �Ÿ��� �־��� ���
        else if (nmAgent.remainingDistance > lostDistance)
        {
            target = null;
            nmAgent.SetDestination(transform.position);
            yield return null;
            // StateMachine �� ���� ����
            ChangeState(State.IDLE);
        }
        else
        {
            // WalkFWD �ִϸ��̼��� �� ����Ŭ ���� ���
            yield return new WaitForSeconds(curAnimStateInfo.length);
        }
    }

    IEnumerator ATTACK()
    {
        var curAnimStateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // ���� �ִϸ��̼��� ���� �� Idle Battle �� �̵��ϱ� ������ 
        // �ڵ尡 �� ������ ���� ������ Attack01 �� Play
        anim.Play("Attack01", 0, 0);

        // �Ÿ��� �־�����
        if (nmAgent.remainingDistance > nmAgent.stoppingDistance)
        {
            // StateMachine�� �������� ����
            ChangeState(State.CHASE);
        }
        else
            // ���� animation �� �� �踸ŭ ���
            // �� ��� �ð��� �̿��� ���� ������ ������ �� ����.
            yield return new WaitForSeconds(curAnimStateInfo.length * 2f);
    }

    IEnumerator KILLED()
    {
        // ��� �ִϸ��̼� ���
        anim.Play("Die", 0, 0);

        // ��� �ִϸ��̼��� ���� ������ ���
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // ������Ʈ ����
        Destroy(gameObject);
    }

    void ChangeState(State newState)
    {
        state = newState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            nmAgent.SetDestination(target.position);
            ChangeState(State.CHASE);
        }

        else if (other.CompareTag("Weapon"))
        {
            WeaponController weapon = other.GetComponent<WeaponController>();
            if (weapon != null)
            {
                TakeDamage(weapon.damage); // ������ ���ݷ��� ������ ������ ����
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (state == State.KILLED) return;

        currentHP -= damage;
        Debug.Log($"���� ü��: {currentHP}/{maxHP}");

        // HP �� ������Ʈ
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
                slider.value = currentHP / maxHP; // HP ���� ����
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        // target �� null �� �ƴϸ� target �� ��� ����
        nmAgent.SetDestination(target.position);
    }
}