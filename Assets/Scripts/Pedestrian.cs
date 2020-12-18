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

    public void Die(Vector3 bulletDirection, Vector3 bloodOrigin)
    {
        GameObject bloodSplatter = Instantiate(Blood, bloodOrigin, Quaternion.identity);
        Destroy(bloodSplatter, 5);

        if (_isDead)
        {
            return;
        }

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
            rb.AddForce(bulletDirection * 5, ForceMode.Impulse);
        }

        Destroy(gameObject, 10);
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
}
