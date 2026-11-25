
using System.Collections;
using UnityEngine;

public class TrapCrafter : MonoBehaviour
{
    private const float CraftingTime = 1.5f;

    [SerializeField] private TrapSpawner _spawner;
    [SerializeField] private LayerMask _playerLayer;

    private Coroutine _craftingRoutine;
    private TrapCraftingPoint _activePoint;
    private TrapConfigSO _activeConfig;
    private float _craftingProgress;

    public void StartCrafting(TrapCraftingPoint point, TrapConfigSO config, Collider2D other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (_craftingRoutine != null)
            return;

        _activePoint = point;
        _activeConfig = config;
        _craftingRoutine = StartCoroutine(CraftingRoutine());
    }

    public void StopCrafting(TrapCraftingPoint point, Collider2D other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (_activePoint != point || _craftingRoutine == null)
            return;

        CancelCrafting();
    }

    public void Cancel(TrapCraftingPoint point)
    {
        if (_activePoint != point || _craftingRoutine == null)
            return;

        CancelCrafting();
    }

    private void OnDisable()
    {
        if (_craftingRoutine != null)
            StopCoroutine(_craftingRoutine);

        _craftingRoutine = null;
        _activePoint?.ResetProgress();
        _activePoint = null;
        _activeConfig = null;
        _craftingProgress = 0f;
    }

    private IEnumerator CraftingRoutine()
    {
        while (_craftingProgress < CraftingTime)
        {
            _craftingProgress += Time.deltaTime;
            _activePoint.SetProgress(_craftingProgress / CraftingTime);

            yield return null;
        }

        CompleteCrafting();
        _craftingRoutine = null;
    }

    private void CompleteCrafting()
    {
        if (_activePoint == null)
        {
            Debug.LogError("TrapCrafter: ActivePoint is null.");
            return;
        }

        if (_activeConfig == null)
        {
            Debug.LogError("TrapCrafter: ActiveConfig is null.");
            return;
        }

        if (_spawner == null)
        {
            Debug.LogError("TrapCrafter: Spawner is not assigned.");
            return;
        }

        if (_activeConfig.Prefab == null)
        {
            Debug.LogError("TrapCrafter: Prefab is not assigned in TrapConfigSO.");
            return;
        }

        Debug.Log("TrapCrafter: Calling Complete()");
        _activePoint.Complete();

        Debug.Log("TrapCrafter: Calling Spawn()");
        Debug.Log($"Spawner: {_spawner}");
        Debug.Log($"ActivePoint: {_activePoint}");
        Debug.Log($"ActiveConfig: {_activeConfig}");

        Trap prefab = _activeConfig.Prefab;
        Debug.Log($"Prefab: {prefab}");

        Transform spawnPoint = _activePoint.transform;
        Debug.Log($"SpawnPoint: {spawnPoint}");

        _spawner.Spawn(prefab, spawnPoint);

        Debug.Log("TrapCrafter: Crafting completed");

        _activePoint = null;
        _activeConfig = null;
        _craftingProgress = 0f;
    }

    private void CancelCrafting()
    {
        StopCoroutine(_craftingRoutine);

        _craftingRoutine = null;
        _activePoint.ResetProgress();
        _activePoint = null;
        _activeConfig = null;
        _craftingProgress = 0f;
    }
}
