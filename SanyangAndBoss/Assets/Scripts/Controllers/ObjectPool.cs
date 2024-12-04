using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // 생성할 몬스터 프리팹
    public int initialSize = 10; // 초기 풀 크기

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        // 초기 풀 생성
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false); // 비활성화 상태로 시작
        pool.Enqueue(obj);
        return obj;
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true); // 활성화
            return obj;
        }
        else
        {
            // 풀에 남은 오브젝트가 없으면 새로 생성
            return CreateNewObject();
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // 비활성화
        pool.Enqueue(obj); // 풀에 다시 추가
    }
}