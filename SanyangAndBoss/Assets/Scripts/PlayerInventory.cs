using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Transform weaponSlot; // ���⸦ ������ ��ġ
    [SerializeField]
    private GameObject currentWeapon; // ���� ������ ����

    public void EquipWeapon(GameObject newWeaponPrefab)
    {
        // ���� ���� ����
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        // ���ο� ���� ����
        currentWeapon = Instantiate(newWeaponPrefab, weaponSlot);
        // ���ο� ���� ���� �� ����
        // ��ġ, ȸ��, ũ�� ����
        currentWeapon.transform.localPosition = new Vector3(0.05f, -0.058f, -0.03f);
        currentWeapon.transform.localEulerAngles = new Vector3(-4f, -81f, 51f);
        currentWeapon.transform.localScale = new Vector3(18.65f, 30.92f, 34.096f);

        Debug.Log("Weapon equipped: " + newWeaponPrefab.name);
    }
}
