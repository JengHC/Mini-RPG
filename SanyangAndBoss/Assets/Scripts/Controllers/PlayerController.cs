using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _speed = 10.0f;
    [SerializeField] float maxHP = 100.0f; // 최대 HP
    float currentHP;
    [SerializeField] PlayerHPBar hpBar; // PlayerHPBar를 연결

    Vector3 _destPos;
    private bool isAttacking = false; // 공격 중인지 확인하는 플래그

    [SerializeField] private PopupManager popupManager;

    [Header("Audio")]
    [SerializeField] private AudioSource footstepAudioSource; // 발소리용 AudioSource
    [SerializeField] private AudioClip footstepClip; // 발소리 AudioClip
    [SerializeField] private float footstepInterval = 0.5f; // 발소리 간격 (초 단위)
    private float lastFootstepTime = 0f; // 마지막 발소리 재생 시간 기록

    //[Header("Skill Sound Effects")]
    //[SerializeField] private AudioSource skillAudioSource; // 스킬 효과음용 AudioSource
    //[SerializeField] private AudioClip skillQClip; // Q 스킬 효과음
    //[SerializeField] private AudioClip skillWClip; // W 스킬 효과음
    //[SerializeField] private AudioClip skillEClip; // E 스킬 효과음
    //[SerializeField] private AudioClip skillRClip; // R 스킬 효과음

    // 스킬 버튼 연결 (Q, W, E, R 각각)
    public SkillButton skillQButton;
    public SkillButton skillWButton;
    public SkillButton skillEButton;
    public SkillButton skillRButton;

    private void Start()
    {
        currentHP = maxHP; // 초기 HP 설정

        // HPBar 초기화
        if (hpBar != null)
        {
            hpBar.SetMaxHP(maxHP);
        }

        // Input 이벤트 연결
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        // AudioSource 확인 및 설정
        if (footstepAudioSource == null)
        {
            footstepAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (footstepClip != null)
        {
            footstepAudioSource.clip = footstepClip;
        }

        //if (skillAudioSource == null)
        //{
        //    skillAudioSource = gameObject.AddComponent<AudioSource>();
        //}
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

            // 발소리 재생
            if (Time.time - lastFootstepTime >= footstepInterval)
            {
                PlayFootstepSound();
                lastFootstepTime = Time.time;
            }
        }

        // 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetBool("isWalk", true);
    }

    void PlayFootstepSound()
    {
        if (footstepAudioSource != null && footstepClip != null)
            footstepAudioSource.PlayOneShot(footstepClip);
    }

    //void PlaySkillSound(AudioClip clip)
    //{
    //    if (skillAudioSource != null && clip != null)
    //    {
    //        skillAudioSource.PlayOneShot(clip);
    //    }
    //}

    void UpdateIdle()
    {
        // 애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetBool("isWalk", false);
    }

    public void PerformAttack(string attackTrigger, float attackDamage)
    {
        if (_state == PlayerState.Die || isAttacking) return;

        // 공격 애니메이션 트리거
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger(attackTrigger); // 전달받은 트리거 사용

        // 무기 공격 처리
        WeaponController weapon = GetComponentInChildren<WeaponController>();
        if (weapon != null)
        {
            weapon.SetDamage(attackDamage); // 무기 데미지 설정
            weapon.StartAttack();
        }

        isAttacking = true;
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
        }

        // 키보드 입력 처리
        if (_state != PlayerState.Attack)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                skillQButton?.OnClicked(); // Q 버튼 스킬 호출
                //PlaySkillSound(skillQClip); // Q 스킬 효과음 재생
                Debug.Log("Q를 눌렀습니다");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                skillWButton?.OnClicked(); // W 버튼 스킬 호출
                //PlaySkillSound(skillWClip); // W 스킬 효과음 재생
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                skillEButton?.OnClicked(); // E 버튼 스킬 호출
                //PlaySkillSound(skillEClip); // E 스킬 효과음 재생
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                skillRButton?.OnClicked(); // R 버튼 스킬 호출
                //PlaySkillSound(skillRClip); // R 스킬 효과음 재생
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (_state == PlayerState.Die) return;

        currentHP -= damage;
        Debug.Log($"플레이어 체력: {currentHP}/{maxHP}");

        if (currentHP <= 0)
        {
            currentHP = 0;
            _state = PlayerState.Die;

            Animator anim = GetComponent<Animator>();
            anim.Play("Die");

            popupManager.ShowGameOverPopup();
            Debug.Log("플레이어가 죽었습니다!");
        }

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
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}