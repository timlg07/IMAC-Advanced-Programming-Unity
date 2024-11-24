using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Visitor : MonoBehaviour
{
    public float QueuePadding => queuePadding;

    public Visitor PreviousInQueue
    {
        set => previousInQueue = value;
    }

    [SerializeField] private float queuePadding = 1f;

    [FormerlySerializedAs("_currentTarget")] [SerializeField, Readonly] private PointOfInterest currentTarget;
    [FormerlySerializedAs("_isInQueue")] [SerializeField, Readonly] private bool isInQueue = false;
    [FormerlySerializedAs("_isInPOI")] [SerializeField, Readonly] private bool isInPoi = false;
    [FormerlySerializedAs("_previousInQueue")] [CanBeNull, SerializeField, Readonly] private Visitor previousInQueue;

    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        currentTarget = PoiStore.GetRandom();
    }

    private Vector3 GetTargetPosition()
    {
        if (isInQueue)
        {
            if (previousInQueue == null)
            {
                return currentTarget.GetEntrancePosition();
            }
            else
            {
                return previousInQueue.transform.position;
            }
        }
        else
        {
            return currentTarget.GetEndOfQueuePosition();
        }
    }

    void Update()
    {
        if (isInPoi) return; // currently visiting a POI

        var targetPosition = GetTargetPosition();
        var currentPosition = transform.position;
        var distance = Vector3.Distance(currentPosition, targetPosition);
        var reachedTarget = distance <= QueuePadding;

        if (reachedTarget && !isInQueue)
        {
            previousInQueue = currentTarget.Enqueue(this);
            isInQueue = true;
        }

        _agent.SetDestination(reachedTarget ? currentPosition : targetPosition);
    }

    public void VisitPoi() // called by PointOfInterest
    {
        isInPoi = true;
        isInQueue = false;
        previousInQueue = null;
    }

    public void ExitPoi()
    {
        isInPoi = false;
        currentTarget = PoiStore.GetRandom();
    }
}