//using System.Collections.Generic;
//using UnityEngine;

//public class SkillManager : MonoBehaviour
//{
//    public static SkillManager Instance;

//    private Dictionary<string, float> skillCooldowns = new Dictionary<string, float>();

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    // ��ų ��Ÿ�� ����
//    public void StartCooldown(string skillName, float cooldown)
//    {
//        skillCooldowns[skillName] = Time.time + cooldown;
//    }

//    // ��ų ��� ���� ���� Ȯ��
//    public bool IsSkillAvailable(string skillName)
//    {
//        if (!skillCooldowns.ContainsKey(skillName)) return true;

//        return Time.time >= skillCooldowns[skillName];
//    }

//    // ���� ��Ÿ�� ��ȯ
//    public float GetRemainingCooldown(string skillName)
//    {
//        if (!skillCooldowns.ContainsKey(skillName)) return 0;

//        return Mathf.Max(0, skillCooldowns[skillName] - Time.time);
//    }
//}