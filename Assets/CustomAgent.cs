using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class CustomAgent : MonoBehaviour
{
    [SerializeField] private float waitTime = 1f;

    private NavMeshAgent agent;
    [SerializeField] private PointOfInterest currentTarget;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SelectRandomPoi();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isWaiting) return;

        var oPoi = other.GetComponent<PointOfInterest>();
        if (oPoi && oPoi.IsTaken && oPoi == currentTarget)
        {
            StartCoroutine(WaitInQueue());
        }
    }

    private bool Reserve(PointOfInterest poi)
    {
        return poi && !poi.IsTaken && poi.TryReserve(this);
    }
/*
    private void OnTriggerExit(Collider other)
    {
        var oPoi = other.GetComponent<PointOfInterest>();
        if (oPoi && oPoi.IsTaken)
        {
            oPoi.OnLeave(this);
        }
    }
*/

    IEnumerator WaitInQueue()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        currentTarget.OnLeave(this);
        SelectRandomPoi();
    }

    PointOfInterest[] GetPois()
    {
        return FindObjectsOfType<PointOfInterest>();
    }

    [ContextMenu("Select Random POI")]
    void SelectRandomPoi()
    {
        var activePois = new List<PointOfInterest>(GetPois());
        activePois.RemoveAll(poi => poi.IsTaken);
        var randomPoi = currentTarget;
        do
        {
            while (randomPoi == currentTarget)
                randomPoi = activePois[Random.Range(0, activePois.Count)];
        } while (!Reserve(randomPoi));

        agent.SetDestination(randomPoi.transform.position);
        currentTarget = randomPoi;
    }

    public void PoiMoved()
    {
        agent.SetDestination(currentTarget.transform.position);
    }
}