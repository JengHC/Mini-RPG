using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private bool x, y, z;   // 값이 true이면, Target의 조표, false면 현재 좌표
    [SerializeField]        
    private Transform target;   // 쫒차가야할 대상 Transform

    // Update is called once per frame
    void Update()
    {
        // 쫓아갈 대상이 없으면 종료
        if (!target) return;

        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));

    }
}
