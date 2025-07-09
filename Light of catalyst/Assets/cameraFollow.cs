using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public float FollowSpeed = 3f;
    public Transform target;






    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
