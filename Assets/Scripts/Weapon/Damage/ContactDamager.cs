using UnityEngine;

public class ContactDamager : MonoBehaviour
{
    [SerializeField] private TargetDetectionArea _targetDetectionArea;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _hitInterval = 1f;

    private IDamageable _target;
    private float _nextDamageTime;
    private bool _isAttacking;

    private void Update()
    {
        if (_isAttacking == false || _target == null || Time.time < _nextDamageTime)
            return;

        _target.TakeDamage(_damage);
        _nextDamageTime = Time.time + _hitInterval;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target) == false)
            return;

        _target = target;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target) == false)
            return;

        if (_target == target)
        {
            _target = null;
            _isAttacking = false;
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