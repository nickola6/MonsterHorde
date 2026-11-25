using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class TrapCrafting : MonoBehaviour
{
    private const float CraftingTime = 1.5f;

    private TrapCraftingContext _context;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _craftingRoutine;
    private float _craftingProgress;
    private bool _isCompleted;

    protected abstract Trap Prefab { get; }

    private void Awake()
    {
        _context = GetComponentInParent<TrapCraftingContext>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        if (_craftingRoutine != null)
            StopCoroutine(_craftingRoutine);

        _craftingRoutine = null;
        _craftingProgress = 0f;
        _context.View.Hide();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCompleted == true || _craftingRoutine != null)
            return;

        if ((_context.PlayerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        _craftingRoutine = StartCoroutine(CraftingRoutine());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((_context.PlayerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        if (_craftingRoutine == null)
            return;

        StopCoroutine(_craftingRoutine);
        _craftingRoutine = null;
        _craftingProgress = 0f;
        _context.View.Hide();
    }

    private IEnumerator CraftingRoutine()
    {
        while (_craftingProgress < CraftingTime)
        {
            _craftingProgress += Time.deltaTime;
            _context.View.SetProgress(_craftingProgress / CraftingTime);

            yield return null;
        }

        CompleteCrafting();
        _craftingRoutine = null;
    }

    private void CompleteCrafting()
    {
        _isCompleted = true;
        _context.View.Hide();
        _spriteRenderer.enabled = false;
        _context.Spawner.Spawn(Prefab, transform);
    }
}