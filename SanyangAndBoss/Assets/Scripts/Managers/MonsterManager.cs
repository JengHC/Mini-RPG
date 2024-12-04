using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    public List<Monster> monsters = new List<Monster>();
    public Transform player; // 플레이어의 Transform

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (player == null) return;

        // 모든 몬스터와 플레이어의 거리 계산
        foreach (var monster in monsters)
        {
            if (monster != null && monster.IsAlive)
            {
                float distance = Vector3.Distance(player.position, monster.transform.position);

                if (distance <= monster.attackDistance)
                {
                    monster.ChangeState(Monster.State.ATTACK);
                }
                else if (distance <= monster.lostDistance)
                {
                    monster.ChangeState(Monster.State.CHASE);
                }
                else
                {
                    monster.ChangeState(Monster.State.IDLE);
                }
            }
        }
    }
}
