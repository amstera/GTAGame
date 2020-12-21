using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    void Update()
    {
        Vector3 followPosition = Camera.main.transform.parent.gameObject.transform.position;
        transform.position = new Vector3(followPosition.x, transform.position.y, followPosition.z);
    }
}
