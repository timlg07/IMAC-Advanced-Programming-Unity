using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class VisitorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject visitorPrefab;
    [SerializeField] private float spawnRange = 25f;
    [SerializeField, Readonly] private int visitorCount = 1;
    
    void Start()
    {
        Assert.IsNotNull(visitorPrefab, "Visitor prefab not set!");
    }
    
    public void SpawnVisitor()
    {
        var randomPosition = transform.position + Random.insideUnitSphere * spawnRange;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
        var visitor = Instantiate(visitorPrefab, hit.position, Quaternion.identity);
        visitor.name = $"Visitor {visitorCount++}";
    }
    
    public void SpawnVisitors(int count)
    {
        for (var i = 0; i < count; i++)
        {
            SpawnVisitor();
        }
    }
    
    // Draw spawn range gizmo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }
    
}
