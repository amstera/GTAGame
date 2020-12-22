using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject ThrownPerson;
    public CarOccupied Occupied;
    public Vector3 TargetPosition;
    public float Speed = 8;
    public AudioSource Door;
    public AudioSource Driving;
    public AudioSource CarHit;
    public AudioSource Burning;
    public AudioSource ExplosionSound;
    public AudioSource MetalBang;

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
            Driving.Play();
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

                if (!Driving.isPlaying)
                {
                    Driving.Play();
                }
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
                Driving.Stop();
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
        Car car = collision.collider.GetComponent<Car>();
        if (car != null && collision.relativeVelocity.magnitude > 0f)
        {
            MetalBang.Play();
        }
    }

    public void EnterCar(GameObject player)
    {
        Driving.Stop();
        if (Occupied != CarOccupied.User)
        {
            Door.Play();
            if (Occupied == CarOccupied.Comp)
            {
                GameManager.Instance.AddWantedLevel();
                _navMeshAgent.enabled = false;
                GameObject person = Instantiate(ThrownPerson, transform.position - (transform.right * 1.5f), Quaternion.identity);
                foreach (Rigidbody rb in person.GetComponentsInChildren<Rigidbody>())
                {
                    rb.AddForce(-transform.right * 5, ForceMode.Impulse);
                }
                Destroy(person, 10);
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
        Door.Play();
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
        CarHit.Play();
        if (_health <= 0)
        {
            ExplosionSound.Play();
            if (Occupied == CarOccupied.Comp)
            {
                GameManager.Instance.AddMoney(500);
            }
            GameManager.Instance.AddWantedLevel();
            GameObject explosionEffect = Instantiate(Explosion, transform.position, Quaternion.identity);
            Destroy(explosionEffect, 2);
            if (Occupied == CarOccupied.User)
            {
                _player.GetComponent<Player>().Hit(60);
                LeaveCar();
            }
            foreach (Renderer rendererComponent in GetComponentsInChildren<Renderer>())
            {
                rendererComponent.enabled = false;
            }
            Destroy(gameObject, 5);
        }
        else if (_health <= 65)
        {
            if (!Burning.isPlaying)
            {
                Burning.Play();
            }
            transform.Find("Smoke").gameObject.SetActive(true);
        }
    }
}

public enum CarOccupied
{
    None = 0,
    Comp = 1,
    User = 2
}
