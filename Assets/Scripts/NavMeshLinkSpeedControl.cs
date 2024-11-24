using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class NavMeshLinkSpeedControl : MonoBehaviour
{
    public float linkSpeedFactor = .1f;

    private NavMeshAgent _agent;
    private bool _linking;
    private float _origSpeed;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _origSpeed = _agent.speed;
        _linking = false;
    }

    void FixedUpdate()
    {
        if (_agent.isOnOffMeshLink && !_linking)
        {
            _linking = true;
            _agent.speed *= linkSpeedFactor;
        }
        else if (_agent.isOnNavMesh && _linking)
        {
            _linking = false;
            _agent.velocity = Vector3.zero;
            _agent.speed = _origSpeed;
        }
    }
}