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
    [SerializeField, Tooltip("time a visitor spends in the POI in seconds")]
    private float visitTime = 1f;

    [SerializeField] private Transform entrance;
    [SerializeField] private Transform exit;
    [SerializeField] private int maxVisitors = 1;

    private readonly List<Visitor> _queue = new();
    private readonly List<Visitor> _visitors = new();

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
#if UNITY_EDITOR
        Debug.Log($"{visitor.name} enqueued for {name}");
#endif

        var endOfQueue = GetEndOfQueue();
        _queue.Add(visitor);
        return endOfQueue;
    }

    private void Update()
    {
        if (_queue.Count == 0) return;

        TryEnterPoi();
    }

    private void TryEnterPoi()
    {
        if (_visitors.Count >= maxVisitors) return;

        var visitor = _queue[0];
        var distance = Vector3.Distance(visitor.transform.position, entrance.position);

        if (distance < visitor.QueuePadding) EnterPoi(visitor);
    }

    private void EnterPoi(Visitor visitor)
    {
        _queue.RemoveAt(0);
        _visitors.Add(visitor);
        visitor.VisitPoi();
        visitor.gameObject.SetActive(false);
        StartCoroutine(Visit(visitor));

        if (_queue.Count > 0) _queue[0].PreviousInQueue = null;

#if UNITY_EDITOR
        Debug.Log($"{visitor.name} entered {name}");
#endif
    }

    private IEnumerator Visit(Visitor visitor)
    {
        yield return new WaitForSeconds(visitTime);
        _visitors.Remove(visitor);
        visitor.transform.position = exit.position;
        visitor.gameObject.SetActive(true);
        visitor.ExitPoi();

#if UNITY_EDITOR
        Debug.Log($"{visitor.name} exited {name}");
#endif
    }
}