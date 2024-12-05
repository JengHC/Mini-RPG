using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterKillManager : MonoBehaviour
{
    public static MonsterKillManager Instance; // �̱��� ����

    private int monsterKillCount = 0; // ���� ���� ��
    public int killCounts = 7; // �� ��ȯ�� ���� ���� ��

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ���Ͱ� ���� �� ȣ��� �޼���
    public void MonsterKilled()
    {
        monsterKillCount++;
        Debug.Log($"���� ���� ��: {monsterKillCount}/{killCounts}");

        // ���Ͱ� �ʿ��� ��ŭ ������ �� ��ȯ
        if (monsterKillCount >= killCounts)
        {
            ChangeScene();
        }
    }

    // �� ��ȯ
    private void ChangeScene()
    {
        Debug.Log("���� ������ �̵��մϴ�!");
        SceneManager.LoadScene("BossScene"); // ������ ������ �̵�
    }
}