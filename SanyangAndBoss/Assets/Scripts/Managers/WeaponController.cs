using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float damage = 10f;
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private float attackDuration = 0.5f;

    private bool isAttacking = false;

    private void Awake()
    {
        if (weaponCollider == null)
            weaponCollider = GetComponent<Collider>();

        if (weaponCollider != null)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = false;
        }
    }

    public void StartAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        weaponCollider.enabled = true;
        StartCoroutine(DisableColliderAfterAttack());
    }

    private IEnumerator DisableColliderAfterAttack()
    {
        yield return new WaitForSeconds(attackDuration);
        weaponCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
                Debug.Log($"몬스터에게 {damage} 데미지를 입혔습니다.");
            }
        }
    }
}