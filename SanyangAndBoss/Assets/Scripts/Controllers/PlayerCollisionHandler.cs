using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{


    // �浹 ����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            Debug.Log("���Ϳ� �浹!");
        }
    }
    
}