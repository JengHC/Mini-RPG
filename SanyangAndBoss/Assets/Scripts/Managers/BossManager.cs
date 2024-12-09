using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    public List<BossMonster> bossmonsters = new List<BossMonster>();
    public Transform player; // �÷��̾��� Transform

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

        // ��� ���Ϳ� �÷��̾��� �Ÿ� ���
        foreach (var monster in bossmonsters)
        {
            if (monster != null && monster.IsAlive)
            {
                float distance = Vector3.Distance(player.position, monster.transform.position);

                if (distance <= monster.attackDistance)
                {
                    monster.ChangeState(BossMonster.State.ATTACK);
                }
                else if (distance <= monster.lostDistance)
                {
                    monster.ChangeState(BossMonster.State.CHASE);
                }
                else
                {
                    monster.ChangeState(BossMonster.State.IDLE);
                }
            }
        }
    }
}
