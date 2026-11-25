using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrapCrafterPoint : MonoBehaviour
{
    [SerializeField] private TrapCrafter _crafter;
    [SerializeField] private TrapConfigSO _config;
    [SerializeField] private TrapCrafterView _visual;

    private Collider2D _collider;
    private TrapCrafterView _crafterView;
    private bool _isCompleted;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _crafterView = GetComponent<TrapCrafterView>();

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
        _crafterView.SetProgress(progress);
    }

    public void ResetProgress()
    {
        _crafterView.HideProgress();
    }

    public void Complete()
    {
        _isCompleted = true;
        _collider.enabled = false;
        _crafterView.HideVisuals();
    }
}