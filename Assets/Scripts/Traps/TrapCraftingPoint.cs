using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TrapCraftingPoint : MonoBehaviour
{
    private const float CraftingTime = 1.5f;

    [SerializeField] private TrapSpawner _spawner;
    [SerializeField] private TrapCraftingView _view;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private TrapConfigSO _scriptableObject;

    private SpriteRenderer _spriteRenderer;
    private Coroutine _craftingRoutine;
    private float _craftingProgress;
    private bool _isCompleted;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        if (_craftingRoutine != null)
            StopCoroutine(_craftingRoutine);

        _craftingRoutine = null;
        _craftingProgress = 0f;
        _view.Hide();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCompleted == true || _craftingRoutine != null)
            return;

        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        _craftingRoutine = StartCoroutine(CraftingRoutine());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (_craftingRoutine == null)
            return;

        StopCoroutine(_craftingRoutine);
        _craftingRoutine = null;
        _craftingProgress = 0f;
        _view.Hide();
    }

    private IEnumerator CraftingRoutine()
    {
        while (_craftingProgress < CraftingTime)
        {
            _craftingProgress += Time.deltaTime;
            _view.SetProgress(_craftingProgress / CraftingTime);

            yield return null;
        }

        CompleteCrafting();
        _craftingRoutine = null;
    }

    private void CompleteCrafting()
    {
        _isCompleted = true;
        _view.Hide();
        _spriteRenderer.enabled = false;
        _spawner.Spawn(_scriptableObject.Prefab, transform);
    }
}