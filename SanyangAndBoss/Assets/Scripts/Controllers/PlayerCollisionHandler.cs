using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{


    // 충돌 감지
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("몬스터와 충돌!");
        }
    }
    
}