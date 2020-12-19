using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 2;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * Speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Speed * Time.deltaTime;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Car car = collision.collider.GetComponent<Car>();
        if (car != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                car.EnterCar(gameObject);
            }
        }
    }
}
