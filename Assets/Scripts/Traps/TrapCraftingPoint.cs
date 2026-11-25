using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(TrapCraftingView))]
public class TrapCraftingPoint : MonoBehaviour
{
    [SerializeField] private TrapCrafter _crafting;
    [SerializeField] private TrapConfigSO _config;

    private Collider2D _collider;
    private TrapCraftingView _view;
    private bool _isCompleted;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _view = GetComponent<TrapCraftingView>();
    }

    private void OnDisable()
    {
        _crafting.Cancel(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCompleted == true)
            return;

        _crafting.StartCrafting(this, _config, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _crafting.StopCrafting(this, other);
    }

    public void SetProgress(float progress)
    {
        _view.SetProgress(progress);
    }

    public void ResetProgress()
    {
        _view.HideProgress();
    }

    public void Complete()
    {
        _isCompleted = true;
        _collider.enabled = false;
        _view.HideVisuals();
    }
}