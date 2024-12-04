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

//    // 스킬 쿨타임 시작
//    public void StartCooldown(string skillName, float cooldown)
//    {
//        skillCooldowns[skillName] = Time.time + cooldown;
//    }

//    // 스킬 사용 가능 여부 확인
//    public bool IsSkillAvailable(string skillName)
//    {
//        if (!skillCooldowns.ContainsKey(skillName)) return true;

//        return Time.time >= skillCooldowns[skillName];
//    }

//    // 남은 쿨타임 반환
//    public float GetRemainingCooldown(string skillName)
//    {
//        if (!skillCooldowns.ContainsKey(skillName)) return 0;

//        return Mathf.Max(0, skillCooldowns[skillName] - Time.time);
//    }
//}