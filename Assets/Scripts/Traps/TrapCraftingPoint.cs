
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TrapCraftingPoint : MonoBehaviour
{
    [SerializeField] private TrapSpawner _spawner;
    [SerializeField] private TrapCraftingView _view;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private TrapDefinitionSO _definition;

    private SpriteRenderer _spriteRenderer;
    private float _craftingProgress;
    private bool _isPlayerInside;
    private bool _isCompleted;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_isPlayerInside == false || _isCompleted == true)
            return;

        _craftingProgress += Time.deltaTime;
        _view.SetProgress(_craftingProgress / _definition.CraftingTime);

        if (_craftingProgress >= _definition.CraftingTime)
            CompleteCrafting();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) != 0)
            _isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0)
            return;

        _isPlayerInside = false;
        _craftingProgress = 0f;
        _view.Hide();
    }

    private void CompleteCrafting()
    {
        _isCompleted = true;
        _isPlayerInside = false;
        _view.Hide();
        _spriteRenderer.enabled = false;
        _spawner.Spawn(_definition.Prefab, transform);
    }
}