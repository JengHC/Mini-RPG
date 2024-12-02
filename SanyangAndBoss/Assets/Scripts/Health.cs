using System.Runtime.InteropServices;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float Hp = 10;
    public float MaxHp = 10;

    void Start()
    {
    }

    public void Damage(float damage)
    {
        if (Hp > 0)
        {
            Hp -= damage;

            if (Hp <= 0)
            {
                Debug.Log("Á×À½");
            }
            else
            {
                Debug.Log("´ÙÄ§");
            }
        }
    }
}
