using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    [SerializeField]
    float maxHP = 100.0f; // 최대 HP
    float currentHP;

    [SerializeField]
    PlayerHPBar hpBar; // PlayerHPBar를 연결

    Vector3 _destPos;
    private bool isAttacking = false; // 공격 중인지 확인하는 플래그

    void Start()
    {
        currentHP = maxHP; // 초기 HP 설정

        // HPBar 초기화
        if (hpBar != null)
        {
            hpBar.SetMaxHP(maxHP);
        }

        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Attack // 공격 상태 추가
    }

    PlayerState _state = PlayerState.Idle;

    void UpdateDie()
    {
        // 죽었으니 아무것도 못함
    }

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 30 * Time.deltaTime);
        }

        // 애니메이션       
        Animator anim = GetComponent<Animator>();
        anim.SetBool("isWalk", true);
    }

    void UpdateIdle()
    {
        // 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetBool("isWalk", false);
    }

    void UpdateAttack()
    {
        if (isAttacking) return;

        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Attack");
        isAttacking = true;

        WeaponController weapon = GetComponentInChildren<WeaponController>();
        if (weapon != null)
        {
            weapon.StartAttack(); // 무기 공격 시작
        }

        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.0f); // 애니메이션 길이에 맞게 조정
        _state = PlayerState.Idle;
        isAttacking = false; // 공격 종료
    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Die:
                UpdateDie();
                break;

            case PlayerState.Moving:
                UpdateMoving();
                break;

            case PlayerState.Idle:
                UpdateIdle();
                break;

            case PlayerState.Attack:
                UpdateAttack();
                break;
        }

        // Q 키를 누르면 공격 상태로 전환
        if (Input.GetKeyDown(KeyCode.Q) && _state != PlayerState.Attack)
        {
            _state = PlayerState.Attack;
            Debug.Log("Q 키 입력: 공격 상태로 전환");
        }
    }

    // 플레이어가 피해를 입었을 때 호출
    public void TakeDamage(float damage)
    {
        if (_state == PlayerState.Die) return;

        currentHP -= damage;
        Debug.Log($"플레이어 체력: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            currentHP = 0;
            _state = PlayerState.Die;
            Debug.Log("플레이어가 죽었습니다!");
        }

        // PlayerHPBar 업데이트
        if (hpBar != null)
        {
            hpBar.UpdateHP(currentHP);
        }
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}