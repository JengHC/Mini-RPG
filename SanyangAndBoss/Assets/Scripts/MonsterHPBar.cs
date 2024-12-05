using UnityEngine;
using System.Collections.Generic;

public class MonsterHPBar : MonoBehaviour
{
    [SerializeField] GameObject goPrefabs = null;
    List<Transform> objectList = new List<Transform>();
    List<GameObject> hpBarList = new List<GameObject>();

    Camera cam = null;

    void Start()
    {
        cam = Camera.main;

        GameObject[] tagObjects = GameObject.FindGameObjectsWithTag("Monster");
        for (int i = 0; i < tagObjects.Length; i++)
        {
            objectList.Add(tagObjects[i].transform);

            // HP 바 생성 및 설정
            GameObject hpbar = Instantiate(goPrefabs.gameObject, tagObjects[i].transform.position, Quaternion.identity, transform);
            hpBarList.Add(hpbar);

            // HP 바를 몬스터와 연결
            Monster monster = tagObjects[i].GetComponent<Monster>();
            if (monster != null)
            {
                monster.hpBar = hpbar; // Monster 스크립트의 hpBar 참조에 할당
            }
        }
    }

    void Update()
    {
        for (int i = objectList.Count - 1; i >= 0; i--)
        {
            if (objectList[i] == null)
            {
                Destroy(hpBarList[i]);
                hpBarList.RemoveAt(i);
                objectList.RemoveAt(i);
            }
            else
            {
                hpBarList[i].transform.position = cam.WorldToScreenPoint(objectList[i].position + new Vector3(0, 2.0f, 0));
            }
        }
    }
}