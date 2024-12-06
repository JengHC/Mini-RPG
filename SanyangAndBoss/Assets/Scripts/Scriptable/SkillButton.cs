using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    // ScriptableObject로 생성한 스킬
    public Skill skill;

    // Player 객체 연결
    public PlayerController player;

    // 스킬 이미지
    public Image imgIcon;

    // Cooldown 이미지
    public Image imgCool;

    // 버튼 비활성화를 위한 Button 컴포넌트
    private Button button;

    void Start()
    {
        // Skill에 등록된 스킬 아이콘 설정
        if (skill != null && imgIcon != null)
        {
            imgIcon.sprite = skill.icon;
        }
        else
        {
            Debug.LogError("Skill 또는 imgIcon이 설정되지 않았습니다.");
        }

        // Cool 이미지 초기화
        if (imgCool != null)
        {
            imgCool.fillAmount = 0;
        }

        // Button 컴포넌트 가져오기
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("SkillButton에 Button 컴포넌트가 없습니다.");
        }
    }

    public void OnClicked()
    {
        // 쿨타임 중이라면 동작하지 않음
        if (imgCool != null && imgCool.fillAmount > 0)
        {
            Debug.Log("스킬이 쿨타임 중입니다.");
            return;
        }

        // PlayerController 연결 확인 및 스킬 실행
        if (player != null)
        {
            player.PerformAttack(skill.animationName, skill.damage); // 스킬 정보 전달
        }
        else
        {
            Debug.LogError("PlayerController가 설정되지 않았습니다.");
            return;
        }

        // 스킬 쿨타임 시작
        StartCoroutine(SC_Cool());
    }

    IEnumerator SC_Cool()
    {
        if (imgCool == null || skill == null)
        {
            Debug.LogError("imgCool 또는 skill이 설정되지 않았습니다.");
            yield break;
        }

        // 버튼 비활성화
        if (button != null)
        {
            button.interactable = false;
        }

        float duration = skill.cool; // 쿨타임 지속 시간
        float elapsed = 0f;

        imgCool.fillAmount = 1;

        // 쿨타임 진행
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            imgCool.fillAmount = Mathf.Clamp01(1 - (elapsed / duration));
            yield return null;
        }

        imgCool.fillAmount = 0;

        // 버튼 활성화
        if (button != null)
        {
            button.interactable = true;
        }
    }
}