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

    private void OnDisable()
    {
        CancelCrafting();
    }

    public void StartCrafting(TrapCraftingPoint point, TrapConfigSO config, Collider2D other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }
        if (_craftingRoutine != null)
        {
            return;
        }
        _activePoint = point;
        _activeConfig = config;
        _craftingRoutine = StartCoroutine(CraftingRoutine());
    }

    public void StopCrafting(TrapCraftingPoint point, Collider2D other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }
        if (_activePoint != point || _craftingRoutine == null)
        {
            return;
        }

        CancelCrafting();
    }

    public void Cancel(TrapCraftingPoint point)
    {
        if (_activePoint != point || _craftingRoutine == null)
        {
            return;
        }

        CancelCrafting();
    }
    private IEnumerator CraftingRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < CraftingTime)
        {
            elapsedTime += Time.deltaTime;
            _activePoint.SetProgress(elapsedTime / CraftingTime);

            yield return null;
        }
        CompleteCrafting();
        _craftingRoutine = null;
    }
    private void CompleteCrafting()
    {
        TrapCraftingPoint point = _activePoint;
        TrapConfigSO config = _activeConfig;
        TrapSpawner spawner = _spawner;
        Trap prefab = config.Prefab;
        Transform spawnPoint = point.transform;
        point.Complete();
        spawner.Spawn(prefab, spawnPoint);
        ClearState();
    }
    private void CancelCrafting()
    {
        if (_craftingRoutine != null)
        StopCoroutine(_craftingRoutine);
        _craftingRoutine = null;
        _activePoint?.ResetProgress();
        ClearState();
    }
    private void ClearState()
    {
        _activePoint = null;
        _activeConfig = null;
    }
}
