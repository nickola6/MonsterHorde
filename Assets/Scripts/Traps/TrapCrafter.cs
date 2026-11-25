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
        _activePoint.Complete();
        _spawner.Spawn(_activeConfig.Prefab, _activePoint.transform);

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