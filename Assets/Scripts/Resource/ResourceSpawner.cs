using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourcePool _pool;
    [SerializeField] private Transform _spawnPoints;

    private readonly Dictionary<Resource, Transform> _spawnedResources = new();
    private readonly List<Transform> _points = new();

    private void Start()
    {
        for (int i = 0; i < _spawnPoints.childCount; i++)
            _points.Add(_spawnPoints.GetChild(i));

        SpawnResources();
    }

    private void OnDestroy()
    {
        foreach (Resource resource in _spawnedResources.Keys)
            resource.Died -= OnResourceDied;

        _spawnedResources.Clear();
    }

    private void SpawnResources()
    {
        foreach (Transform point in _points)
        {
            ResourceSpawnConfigSO config = _pool.GetRandomConfig();
            Resource resource = _pool.Get(config, point);

            _spawnedResources.Add(resource, point);
            resource.Died += OnResourceDied;
        }
    }

    private void OnResourceDied(Resource resource)
    {
        resource.Died -= OnResourceDied;
        _spawnedResources.Remove(resource);
        _pool.Release(resource);
    }
}
