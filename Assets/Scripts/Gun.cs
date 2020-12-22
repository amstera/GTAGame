using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public AudioSource GunShot;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GunShot.Play();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Pedestrian pedestrian = hit.collider.GetComponentInParent<Pedestrian>();
                if (pedestrian != null)
                {
                    StartCoroutine(KillPedestrian(pedestrian, hit.point));
                }
                else
                {
                    Car car = hit.collider.GetComponentInParent<Car>();
                    if (car != null)
                    {
                        StartCoroutine(ShootCar(car));
                    }
                }
            }
        }
    }

    private IEnumerator KillPedestrian(Pedestrian pedestrian, Vector3 origin)
    {
        yield return new WaitForSeconds(0.15f);

        pedestrian.Die(transform.forward, origin);
    }

    private IEnumerator ShootCar(Car car)
    {
        yield return new WaitForSeconds(0.15f);

        car.TakeDamage(35);
    }
}
