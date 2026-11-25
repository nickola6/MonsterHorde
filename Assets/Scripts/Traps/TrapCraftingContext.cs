using UnityEngine;

public class TrapCraftingContext : MonoBehaviour
{
    [SerializeField] private TrapSpawner _spawner;
    [SerializeField] private TrapCraftingView _view;
    [SerializeField] private LayerMask _playerLayer;

    public TrapSpawner Spawner => _spawner;
    public TrapCraftingView View => _view;
    public LayerMask PlayerLayer => _playerLayer;
}