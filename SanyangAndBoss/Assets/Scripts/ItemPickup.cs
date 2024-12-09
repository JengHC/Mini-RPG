using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject weaponPrefab; // ���������� ������ ���� ������

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.EquipWeapon(weaponPrefab);
                Destroy(gameObject); // �������� ������ ����
            }
        }
    }
}