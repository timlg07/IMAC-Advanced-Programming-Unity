using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    public float QueuePadding => queuePadding;
    public Visitor PreviousInQueue { set => _previousInQueue = value; }
    
    [SerializeField] private float queuePadding = 1f;

    [SerializeField, Readonly] private PointOfInterest _currentTarget;
    [SerializeField, Readonly] private bool _isInQueue = false;
    [SerializeField, Readonly] private bool _isInPOI = false;
    [CanBeNull, SerializeField, Readonly] private Visitor _previousInQueue;
    
    private NavMeshAgent _agent;
    
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentTarget = POIStore.GetRandom();
    }
    
    private Vector3 GetTargetPosition()
    {
        if (_isInQueue)
        {
            if (_previousInQueue == null)
            {
                return _currentTarget.GetEntrancePosition();
            }
            else
            {
                return _previousInQueue.transform.position;
            }
        }
        else
        {
            return _currentTarget.GetEndOfQueuePosition();
        }
    }

    void Update()
    {
        if (_isInPOI) return; // currently visiting a POI
        
        var targetPosition = GetTargetPosition();
        var currentPosition = transform.position;
        var distance = Vector3.Distance(currentPosition, targetPosition);
        var reachedTarget = distance <= QueuePadding;

        if (reachedTarget && !_isInQueue)
        {
            _previousInQueue = _currentTarget.Enqueue(this);
            _isInQueue = true;
        }
        
        _agent.SetDestination(reachedTarget ? currentPosition : targetPosition);
    }
    
    public void VisitPOI() // called by PointOfInterest
    {
        _isInPOI = true;
        _isInQueue = false;
        _previousInQueue = null;
    }
    
    public void ExitPOI()
    {
        _isInPOI = false;
        _currentTarget = POIStore.GetRandom();
    }
}
