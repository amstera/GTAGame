using System.Collections;
using UnityEngine;

public class Police : Pedestrian
{
    public GameObject Gun;
    public AudioSource GunShot;

    void Start()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }

        StartCoroutine(GunShoot());
    }

    void Update()
    {
        if (IsDead)
        {
            return;
        }

        transform.LookAt(Camera.main.transform.parent.transform);

        if (GameManager.Instance.WantedLevel == 0)
        {
            IsDead = true;
            Destroy(gameObject);
        }
    }

    private IEnumerator GunShoot()
    {
        yield return new WaitForSeconds(1.5f);

        GunShot.Play();
        RaycastHit hit;
        if (Physics.Raycast(Gun.transform.position + Vector3.down, Gun.transform.forward, out hit))
        {
            Player player = hit.collider.GetComponent<Player>();
            if (player != null)
            {
                player.Hit();
            }
            else
            {
                Car car = hit.collider.GetComponent<Car>();
                if (car != null && car.Occupied == CarOccupied.User)
                {
                    car.TakeDamage(35);
                }
            }
        }

        if (!IsDead)
        {
            StartCoroutine(GunShoot());
        }
    }
}
