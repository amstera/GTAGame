using UnityEngine;

public class Car : MonoBehaviour
{
    public CarOccupied Occupied;
    public float Speed = 8;
    private GameObject _player;
    private GameObject _crosshair;
    private float _timeEnteredCar;
    private float _acceleration;

    void Start()
    {
        _crosshair = GameObject.FindGameObjectWithTag("Crosshair");
    }

    void Update()
    {
        if (Occupied == CarOccupied.User && (Time.time - _timeEnteredCar) > 1 && Input.GetKeyDown(KeyCode.Space))
        {
            LeaveCar();
        }

        if (Occupied == CarOccupied.User)
        {
            bool isMoving = false;
            if (Input.GetKey(KeyCode.W))
            {
                isMoving = true;
                transform.Translate(Vector3.forward * Speed * _acceleration * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                isMoving = true;
                transform.Translate(Vector3.back * Speed * _acceleration * Time.deltaTime);
            }

            if (isMoving)
            {
                _acceleration = Mathf.Clamp(_acceleration += Time.deltaTime / 2, 0, 1);

                if (Input.GetKey(KeyCode.A))
                {
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * -125f);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * 125f);
                }
            }
            else
            {
                _acceleration = Mathf.Clamp(_acceleration -= Time.deltaTime/2, 0, 1);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Pedestrian pedestrian = collision.collider.GetComponent<Pedestrian>();
        if (pedestrian != null && collision.relativeVelocity.magnitude > 0f)
        {
            pedestrian.Die(transform.forward, null);
        }
    }

    public void EnterCar(GameObject player)
    {
        if (Occupied != CarOccupied.User)
        {
            player.SetActive(false);
            _crosshair.SetActive(false);
            _player = player;
            Occupied = CarOccupied.User;
            transform.Find("Camera").gameObject.SetActive(true);
            _timeEnteredCar = Time.time;
        }
    }

    public void LeaveCar()
    {
        if (Occupied == CarOccupied.User)
        {
            _player.SetActive(true);
            _crosshair.SetActive(true);
            _player.transform.position = transform.position - (transform.right * 1.5f) + (Vector3.up * 0.5f);
            _player = null;
            Occupied = CarOccupied.None;
            transform.Find("Camera").gameObject.SetActive(false);
        }
    }
}

public enum CarOccupied
{
    None = 0,
    Comp = 1,
    User = 2
}
