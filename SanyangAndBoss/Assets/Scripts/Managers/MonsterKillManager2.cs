using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterKillManager2 : MonoBehaviour
{
    public static MonsterKillManager2 Instance; // 싱글턴 패턴

    private int monsterKillCount = 0; // 죽인 몬스터 수
    public int killCounts = 1; // 씬 전환을 위한 몬스터 수

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // 몬스터가 죽을 때 호출될 메서드
    public void MonsterKilled()
    {
        monsterKillCount++;
        Debug.Log($"죽인 몬스터 수: {monsterKillCount}/{killCounts}");

        // 몬스터가 필요한 만큼 죽으면 씬 전환
        if (monsterKillCount >= killCounts)
        {
            ChangeScene();
        }
    }

    // 씬 전환
    private void ChangeScene()
    {
        Debug.Log("다음 씬으로 이동합니다!");
        SceneManager.LoadScene("BossScene"); // 지정된 씬으로 이동
    }
}
