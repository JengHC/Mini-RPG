using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Transform weaponSlot; // 무기를 장착할 위치
    [SerializeField]
    private GameObject currentWeapon; // 현재 장착된 무기

    public void EquipWeapon(GameObject newWeaponPrefab)
    {
        // 기존 무기 제거
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(false);
        }

        // 새로운 무기 생성
        currentWeapon = Instantiate(newWeaponPrefab, weaponSlot);
        // 새로운 무기 생성 및 장착
        // 위치, 회전, 크기 설정
        currentWeapon.transform.localPosition = new Vector3(0.05f, -0.058f, -0.03f);
        currentWeapon.transform.localEulerAngles = new Vector3(-4f, -81f, 51f);
        currentWeapon.transform.localScale = new Vector3(18.65f, 30.92f, 34.096f);

        Debug.Log("Weapon equipped: " + newWeaponPrefab.name);
    }
}
