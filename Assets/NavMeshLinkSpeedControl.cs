using UnityEngine;
using UnityEngine.AI;

public class NavMeshLinkSpeedControl : MonoBehaviour
{
    public float linkSpeed = .1f;
    
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
            _agent.speed *= linkSpeed;
        }
        else if (_agent.isOnNavMesh && _linking)
        {
            _linking = false;
            _agent.velocity = Vector3.zero;
            _agent.speed = _origSpeed;
        }
    }
}