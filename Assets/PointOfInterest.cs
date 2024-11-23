using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField, Tooltip("time a visitor spends in the POI in seconds")] private float visitTime = 1f;
    [SerializeField] private Transform entrance;
    [SerializeField] private Transform exit;
    [SerializeField] private int maxVisitors = 1;
    
    private List<Visitor> _queue = new();
    private List<Visitor> _visitors = new();
    
    void Start()
    {
        Assert.IsNotNull(entrance);
        Assert.IsNotNull(exit);
    }
    
    public Vector3 GetEndOfQueuePosition()
    {
        return GetEndOfQueue()?.transform.position ?? entrance.position;
    }
    
    [CanBeNull]
    private Visitor GetEndOfQueue()
    {
        return _queue.Count > 0 ? _queue[^1] : null;
    }
    
    public Vector3 GetEntrancePosition()
    {
        return entrance.position;
    }
    
    [CanBeNull]
    public Visitor Enqueue(Visitor visitor)
    {
        Debug.Log($"{visitor.name} enqueued for {name}");
        
        var endOfQueue = GetEndOfQueue();
        _queue.Add(visitor);
        return endOfQueue;
    }

    private void Update()
    {
        if (_queue.Count == 0) return;
        
        TryEnterPOI();
    }
    
    private void TryEnterPOI()
    {
        if (_visitors.Count >= maxVisitors) return;

        var visitor = _queue[0];
        var distance = Vector3.Distance(visitor.transform.position, entrance.position);
        
        if (distance < visitor.QueuePadding) EnterPOI(visitor);
    }
    
    private void EnterPOI(Visitor visitor)
    {
        _queue.RemoveAt(0);
        _visitors.Add(visitor);
        visitor.VisitPOI();
        visitor.gameObject.SetActive(false);
        StartCoroutine(Visit(visitor));
        
        if (_queue.Count > 0) _queue[0].PreviousInQueue = null;
        
        Debug.Log($"{visitor.name} entered {name}");
    }
    
    private IEnumerator Visit(Visitor visitor)
    {
        yield return new WaitForSeconds(visitTime);
        _visitors.Remove(visitor);
        visitor.transform.position = exit.position;
        visitor.gameObject.SetActive(true);
        visitor.ExitPOI();
        
        Debug.Log($"{visitor.name} exited {name}");
    }
}