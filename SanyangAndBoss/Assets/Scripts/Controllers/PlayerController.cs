using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    Vector3 _destPos;
    private bool isAttacking = false; // 공격 중인지 확인하는 플래그

    void Start()
    {
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

        //if (isAttacking) return; // 이미 공격 중이면 실행하지 않음

        //Animator anim = GetComponent<Animator>();
        //anim.SetTrigger("Attack");
        //isAttacking = true; // 공격 중으로 설정

        //// 일정 시간 뒤 공격 종료 (애니메이션 길이 이후)
        //StartCoroutine(ResetAttack());


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