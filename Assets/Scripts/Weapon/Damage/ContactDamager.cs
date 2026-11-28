using UnityEngine;

public class ContactDamager : MonoBehaviour
{
    [SerializeField] private TargetDetectionArea _targetDetectionArea;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _hitInterval = 1f;

    private IDamageable _target;
    private float _nextDamageTime;
    private bool _isAttacking;

    private void OnEnable()
    {
        _targetDetectionArea.TargetEntered += OnTargetEntered;
        _targetDetectionArea.TargetExited += OnTargetExited;
        TrySelectTarget();
    }

    private void OnDisable()
    {
        _targetDetectionArea.TargetEntered -= OnTargetEntered;
        _targetDetectionArea.TargetExited -= OnTargetExited;
        _target = null;
        _isAttacking = false;
    }

    private void Update()
    {
        if (_isAttacking == false || _target == null || Time.time < _nextDamageTime)
            return;

        _target.TakeDamage(_damage);
        _nextDamageTime = Time.time + _hitInterval;
    }

    private void OnTargetEntered(Collider2D other)
    {
        if (_target != null)
            return;

        if (other.TryGetComponent<IDamageable>(out IDamageable target))
            _target = target;
    }

    private void OnTargetExited(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target) == false || _target != target)
            return;

        _target = null;
        TrySelectTarget();
    }

    private void TrySelectTarget()
    {
        foreach (Collider2D other in _targetDetectionArea.Targets)
        {
            if (other != null && other.TryGetComponent<IDamageable>(out IDamageable target))
            {
                _target = target;
                return;
            }
        }
    }

    public void StartAttack()
    {
        _isAttacking = true;
        _nextDamageTime = 0f;
    }

    public void StopAttack()
    {
        _isAttacking = false;
    }
}