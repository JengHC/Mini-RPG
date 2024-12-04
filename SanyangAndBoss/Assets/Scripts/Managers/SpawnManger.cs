//using System.Collections.Generic;
//using UnityEngine;

//public class SpawnManger : MonoBehaviour
//{
//    public GameObject monsterPrefab; // ���� ������
//    public int initialPoolSize = 10; // �ʱ� Ǯ ũ��
//    public Transform[] spawnPoints; // ���� ��ġ �迭

//    private Queue<Monster> monsterPool = new Queue<Monster>();

//    private void Start()
//    {
//        // �ʱ� ������Ʈ Ǯ ����
//        for (int i = 0; i < initialPoolSize; i++)
//        {
//            CreateNewMonster();
//        }

//        // ����: 3�ʸ��� ���� ����
//        InvokeRepeating(nameof(SpawnMonster), 0f, 3f);
//    }

//    private Monster CreateNewMonster()
//    {
//        GameObject obj = Instantiate(monsterPrefab);
//        obj.SetActive(false);

//        Monster monster = obj.GetComponent<Monster>();
//        if (monster != null)
//        {
//            monsterPool.Enqueue(monster);
//        }
//        return monster;
//    }

//    public void SpawnMonster()
//    {
//        if (spawnPoints == null || spawnPoints.Length == 0)
//        {
//            Debug.LogError("Spawn points�� �������� �ʾҽ��ϴ�!");
//            return;
//        }

//        if (monsterPool.Count > 0)
//        {
//            Monster monster = monsterPool.Dequeue();
//            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

//            if (spawnPoint == null)
//            {
//                Debug.LogError("Spawn point�� null�Դϴ�!");
//                return;
//            }

//            monster.Initialize(spawnPoint); // ���� �߻� ����
//        }
//        else
//        {
//            Debug.LogWarning("������Ʈ Ǯ�� ���Ͱ� �����ϴ�. ���ο� ���͸� �����մϴ�.");
//            CreateNewMonster();
//        }
//    }

//    public void ReturnMonster(Monster monster)
//    {
//        // ���͸� Ǯ�� ��ȯ
//        monsterPool.Enqueue(monster);
//    }
//}
