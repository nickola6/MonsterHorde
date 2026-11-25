using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrapCraftingPoint : MonoBehaviour
{
    [SerializeField]private TrapCrafter _crafter;
    [SerializeField]private TrapCraftingView _craftingView;
    [SerializeField] private TrapConfigSO _config;

    private Collider2D _collider;
    private bool _isCompleted;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
    }

    private void OnDisable()
    {
        _crafter.Cancel(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isCompleted == true)
            return;

        _crafter.StartCrafting(this, _config, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _crafter.StopCrafting(this, other);
    }

    public void SetProgress(float progress)
    {
        _craftingView.SetProgress(progress);
    }

    public void ResetProgress()
    {
        _craftingView.HideProgress();
    }

    public void Complete()
    {
        _isCompleted = true;
        _collider.enabled = false;
        _craftingView.HideVisuals();
    }
}