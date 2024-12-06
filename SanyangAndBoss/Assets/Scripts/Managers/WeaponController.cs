using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float damage = 10f;
    [SerializeField] private Collider weaponCollider;
    [SerializeField] private float attackDuration = 0.5f;

    //[Header("Slash Sound Effect Settings")]
    //public AudioClip slashSound; // 공격 효과음
    //private AudioSource audioSource;

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

        //audioSource = GetComponent<AudioSource>();
        //// AudioSource 설정
        //if (audioSource == null)
        //{
        //    audioSource = gameObject.AddComponent<AudioSource>();
        //}
        //audioSource.playOnAwake = false;
        //if (audioSource != null)
        //{
        //    audioSource.clip = slashSound;
        //    audioSource.Play();
        //}
    }

    public void StopAttack()
    {
        weaponCollider.enabled = false;
        isAttacking = false;
    }

    public void SetDamage(float damageValue)
    {
        damage = damageValue;
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