using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnConfigSO _config;
    [SerializeField] private EnemyPool _pool;
    [SerializeField] private Player _player;

    private readonly HashSet<Enemy> _spawnedEnemies = new();
    private Coroutine _spawnRoutine;

    private void Start()
    {
        StartSpawning();
    }

    private void OnDisable()
    {
        StopSpawning();
    }

    private void OnDestroy()
    {
        foreach (Enemy enemy in _spawnedEnemies)
        enemy.Died -= OnEnemyDied;

        _spawnedEnemies.Clear();
    }

    public void StartSpawning()
    {
        if (_spawnRoutine != null)
        {
            return;
        }

        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (_spawnRoutine == null)
        {
            return;
        }

        StopCoroutine(_spawnRoutine);
        _spawnRoutine = null;
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(_config.StartDelay);

        for (int i = 0; i < _config.SpawnCount; i++)
        {
            Enemy enemy = _pool.Get(_config, transform);

            _spawnedEnemies.Add(enemy);
            enemy.Died += OnEnemyDied;
            enemy.Initialize(_player);

            if (i < _config.SpawnCount - 1)
            yield return new WaitForSeconds(_config.SpawnInterval);
        }

        _spawnRoutine = null;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.Died -= OnEnemyDied;
        _spawnedEnemies.Remove(enemy);
        _pool.Release(enemy);
    }
}
