using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab; // ������ ���� ������
    public int initialSize = 10; // �ʱ� Ǯ ũ��

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        // �ʱ� Ǯ ����
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false); // ��Ȱ��ȭ ���·� ����
        pool.Enqueue(obj);
        return obj;
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true); // Ȱ��ȭ
            return obj;
        }
        else
        {
            // Ǯ�� ���� ������Ʈ�� ������ ���� ����
            return CreateNewObject();
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false); // ��Ȱ��ȭ
        pool.Enqueue(obj); // Ǯ�� �ٽ� �߰�
    }
}