using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour
{
    public GameObject Blood;
    public bool IsWalking;

    private Vector3 _startPosition;
    private NavMeshAgent _navMeshAgent;
    private bool _isDead;

    void Start()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }

        if (IsWalking)
        {
            _startPosition = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.destination = _startPosition - (Vector3.forward * 25);
        }
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (IsWalking && _navMeshAgent.remainingDistance < 0.1f)
        {
            _navMeshAgent.destination = Vector3.Distance(_navMeshAgent.destination, _startPosition) < 1f ? _startPosition - (Vector3.forward * 25) : _startPosition;
        }
    }

    public void Die(Vector3 bulletDirection, Vector3? bloodOrigin)
    {
        if (bloodOrigin != null)
        {
            GameObject bloodParticle = Instantiate(Blood, bloodOrigin.Value, Quaternion.identity);
            Destroy(bloodParticle, 5);
        }

        if (_isDead)
        {
            return;
        }

        GameManager.Instance.AddWantedLevel();
        GameManager.Instance.AddMoney(500);

        _isDead = true;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;

        if (IsWalking)
        {
            _navMeshAgent.enabled = false;
        }

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
            rb.AddForce(bulletDirection * 10, ForceMode.Impulse);
        }

        Destroy(gameObject, 10);
    }
}
