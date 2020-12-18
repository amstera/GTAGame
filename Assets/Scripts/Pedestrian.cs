using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour
{
    public bool IsWalking;

    private Vector3 _startPosition;
    private NavMeshAgent _navMeshAgent;

    void Start()
    {
        if (IsWalking)
        {
            _startPosition = transform.position;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.destination = _startPosition - (Vector3.forward * 25);
        }
    }

    private void Update()
    {
        if (IsWalking && _navMeshAgent.remainingDistance < 0.1f)
        {
            _navMeshAgent.destination = Vector3.Distance(_navMeshAgent.destination, _startPosition) < 1f ? _startPosition - (Vector3.forward * 25) : _startPosition;
        }
    }
}
