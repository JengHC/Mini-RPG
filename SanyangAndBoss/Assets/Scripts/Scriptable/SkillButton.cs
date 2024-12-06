using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    // ScriptableObject�� ������ ��ų
    public Skill skill;

    // Player ��ü ����
    public PlayerController player;

    // ��ų �̹���
    public Image imgIcon;

    // Cooldown �̹���
    public Image imgCool;

    // ��ư ��Ȱ��ȭ�� ���� Button ������Ʈ
    private Button button;

    void Start()
    {
        // Skill�� ��ϵ� ��ų ������ ����
        if (skill != null && imgIcon != null)
        {
            imgIcon.sprite = skill.icon;
        }
        else
        {
            Debug.LogError("Skill �Ǵ� imgIcon�� �������� �ʾҽ��ϴ�.");
        }

        // Cool �̹��� �ʱ�ȭ
        if (imgCool != null)
        {
            imgCool.fillAmount = 0;
        }

        // Button ������Ʈ ��������
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("SkillButton�� Button ������Ʈ�� �����ϴ�.");
        }
    }

    public void OnClicked()
    {
        // ��Ÿ�� ���̶�� �������� ����
        if (imgCool != null && imgCool.fillAmount > 0)
        {
            Debug.Log("��ų�� ��Ÿ�� ���Դϴ�.");
            return;
        }

        // PlayerController ���� Ȯ�� �� ��ų ����
        if (player != null)
        {
            player.PerformAttack(skill.animationName, skill.damage); // ��ų ���� ����
        }
        else
        {
            Debug.LogError("PlayerController�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ��ų ��Ÿ�� ����
        StartCoroutine(SC_Cool());
    }

    IEnumerator SC_Cool()
    {
        if (imgCool == null || skill == null)
        {
            Debug.LogError("imgCool �Ǵ� skill�� �������� �ʾҽ��ϴ�.");
            yield break;
        }

        // ��ư ��Ȱ��ȭ
        if (button != null)
        {
            button.interactable = false;
        }

        float duration = skill.cool; // ��Ÿ�� ���� �ð�
        float elapsed = 0f;

        imgCool.fillAmount = 1;

        // ��Ÿ�� ����
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            imgCool.fillAmount = Mathf.Clamp01(1 - (elapsed / duration));
            yield return null;
        }

        imgCool.fillAmount = 0;

        // ��ư Ȱ��ȭ
        if (button != null)
        {
            button.interactable = true;
        }
    }
}