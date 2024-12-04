using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public float playerHealth = 100f;

    // �浹 ����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("���Ϳ� �浹!");
            TakeDamage(10f); // ���Ϳ� �浹 �� �÷��̾� ü�� ����
        }
    }

    // ü�� ���� ó��
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        Debug.Log($"�÷��̾� ü��: {playerHealth}");

        if (playerHealth <= 0)
        {
            Debug.Log("�÷��̾ ����߽��ϴ�.");
            // �߰� ó�� (��: ������, ���� ���� ��)
        }
    }
}