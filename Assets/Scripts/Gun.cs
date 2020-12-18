using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Pedestrian pedestrian = hit.collider.GetComponent<Pedestrian>();
                if (pedestrian != null)
                {
                    StartCoroutine(KillPedestrian(pedestrian, hit.point));
                }
            }
        }
    }

    private IEnumerator KillPedestrian(Pedestrian pedestrian, Vector3 origin)
    {
        yield return new WaitForSeconds(0.25f);

        pedestrian.Die(transform.forward, origin);
    }
}
