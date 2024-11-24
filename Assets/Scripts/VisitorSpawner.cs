using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class VisitorSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> visitorPrefabs;
    [SerializeField] private float spawnRange = 25f;
    [SerializeField, Readonly] private int visitorCount = 1;

    private GameObject RandomVisitorPrefab => visitorPrefabs[Random.Range(0, visitorPrefabs.Count)];

    void Start()
    {
        Assert.IsTrue(visitorPrefabs.Count > 0, "No visitor prefabs given!");
    }

    public void SpawnVisitor()
    {
        var randomPosition = transform.position + Random.insideUnitSphere * spawnRange;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
        var visitor = Instantiate(RandomVisitorPrefab, hit.position, Quaternion.identity);
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
