//using System.Collections.Generic;
//using UnityEngine;

//public class SpawnManger : MonoBehaviour
//{
//    public GameObject monsterPrefab; // 몬스터 프리팹
//    public int initialPoolSize = 10; // 초기 풀 크기
//    public Transform[] spawnPoints; // 스폰 위치 배열

//    private Queue<Monster> monsterPool = new Queue<Monster>();

//    private void Start()
//    {
//        // 초기 오브젝트 풀 생성
//        for (int i = 0; i < initialPoolSize; i++)
//        {
//            CreateNewMonster();
//        }

//        // 예시: 3초마다 몬스터 스폰
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
//            Debug.LogError("Spawn points가 설정되지 않았습니다!");
//            return;
//        }

//        if (monsterPool.Count > 0)
//        {
//            Monster monster = monsterPool.Dequeue();
//            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

//            if (spawnPoint == null)
//            {
//                Debug.LogError("Spawn point가 null입니다!");
//                return;
//            }

//            monster.Initialize(spawnPoint); // 오류 발생 가능
//        }
//        else
//        {
//            Debug.LogWarning("오브젝트 풀에 몬스터가 없습니다. 새로운 몬스터를 생성합니다.");
//            CreateNewMonster();
//        }
//    }

//    public void ReturnMonster(Monster monster)
//    {
//        // 몬스터를 풀에 반환
//        monsterPool.Enqueue(monster);
//    }
//}
