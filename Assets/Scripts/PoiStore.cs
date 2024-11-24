using UnityEngine;
using UnityEngine.Assertions;

public class PoiStore : MonoBehaviour
{
    private static PoiStore _instance;
    [SerializeField, Readonly] private PointOfInterest[] _pois;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
        }

        _instance = this;
        QueryPoIs();
    }

    private void QueryPoIs()
    {
        _pois = FindObjectsOfType<PointOfInterest>();
        Assert.IsTrue(_pois.Length > 0, "No POIs found!");
    }

    public static PointOfInterest GetRandom()
    {
        return _instance._pois[Random.Range(0, _instance._pois.Length)];
    }
}