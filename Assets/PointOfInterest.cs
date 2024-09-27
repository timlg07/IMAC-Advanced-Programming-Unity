using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public bool IsTaken => placeTaken;
    public PointOfInterest Next => next != null ? next.GetComponent<PointOfInterest>() : null;

    [SerializeField] private bool placeTaken = false;
    [SerializeField] private GameObject next;
    [SerializeField] private CustomAgent agent;

    public bool TryReserve(CustomAgent pAgent)
    {
        if (placeTaken) return false;

        placeTaken = true;
        agent = pAgent;

        var newPos = transform.position + new Vector3(2, 0);
        next = Instantiate(gameObject, newPos, Quaternion.identity);
        next.transform.SetParent(transform.parent);
        Next.placeTaken = false;
        Next.agent = null;
        Next.next = null;

        return true;
    }

    public void OnLeave(CustomAgent pAgent)
    {
        if (agent == pAgent)
        {
            MakeRoom();
            Destroy(gameObject);
        }
    }

    private void MakeRoom()
    {
        if (next != null)
        {
            next.transform.position -= new Vector3(2, 0);
            if (Next.agent != null) Next.agent.PoiMoved();

            Next.MakeRoom();
        }
    }
}