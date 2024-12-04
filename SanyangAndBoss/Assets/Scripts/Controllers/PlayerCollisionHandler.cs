using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public float playerHealth = 100f;

    // 충돌 감지
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("몬스터와 충돌!");
            TakeDamage(10f); // 몬스터와 충돌 시 플레이어 체력 감소
        }
    }

    // 체력 감소 처리
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        Debug.Log($"플레이어 체력: {playerHealth}");

        if (playerHealth <= 0)
        {
            Debug.Log("플레이어가 사망했습니다.");
            // 추가 처리 (예: 리스폰, 게임 종료 등)
        }
    }
}