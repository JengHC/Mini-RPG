using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject weaponPrefab; // 아이템으로 착용할 무기 프리팹

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.EquipWeapon(weaponPrefab);
                Destroy(gameObject); // 아이템을 먹으면 제거
            }
        }
    }
}