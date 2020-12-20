using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    public GameObject Explosion;
    public CarOccupied Occupied;
    public Vector3 TargetPosition;
    public float Speed = 8;

    private GameObject _player;
    private GameObject _crosshair;
    private float _timeEnteredCar;
    private float _acceleration;
    private NavMeshAgent _navMeshAgent;
    private Vector3 _startPosition;
    private int _health = 100;

    void Start()
    {
        _crosshair = GameObject.FindGameObjectWithTag("Crosshair");

        if (Occupied == CarOccupied.Comp)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _startPosition = transform.position;
            _navMeshAgent.destination = TargetPosition;
        }
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
        else if (Occupied == CarOccupied.Comp)
        {
            if (_navMeshAgent.remainingDistance < 0.1f)
            {
                _navMeshAgent.destination = Vector3.Distance(_navMeshAgent.destination, _startPosition) < 1f ? TargetPosition : _startPosition;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
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
            if (Occupied == CarOccupied.Comp)
            {
                _navMeshAgent.enabled = false;
                //add person running out
            }

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

    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            GameObject explosionEffect = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(explosionEffect, 5);
            Destroy(gameObject);
        }
    }
}

public enum CarOccupied
{
    None = 0,
    Comp = 1,
    User = 2
}
