using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TrapCraftingPoint : MonoBehaviour
{
    [SerializeField] private TrapSpawner _spawner;
    [SerializeField] private TrapCraftingView _view;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Trap _trapPrefab;
    [SerializeField] private float _craftingTime = 1.5f;

    private SpriteRenderer _spriteRenderer;
    private float _craftingProgress;
    private bool _isPlayerInside;
    private bool sdfsdf;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_craftingTime <= 0f)
            throw new System.ArgumentOutOfRangeException(nameof(_craftingTime));
    }

    private void Update()
    {
        if (_isPlayerInside == false || sdfsdf == true)
            return;

        _craftingProgress += Time.deltaTime;
        _view.SetProgress(_craftingProgress / _craftingTime);

        if (_craftingProgress >= _craftingTime)
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
        sdfsdf = true;
        _isPlayerInside = false;
        _view.Hide();
        _spriteRenderer.enabled = false;
        _spawner.Spawn(_trapPrefab, transform);
    }
}