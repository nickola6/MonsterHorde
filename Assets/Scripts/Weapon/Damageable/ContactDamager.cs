using System.Collections.Generic;
using UnityEngine;

public class ContactDamager : MonoBehaviour
{
    [SerializeField] private TargetDetectionArea _targetDetectionArea;
    [SerializeField] private float _damage = 50f;
    [SerializeField] private float _hitInterval = 1f;

    private readonly Dictionary<IDamageable, float> _nextDamageTimes = new();
    private readonly List<IDamageable> _targets = new();

    private void OnEnable()
    {
        _targetDetectionArea.TargetEntered += OnTargetEntered;
        _targetDetectionArea.TargetExited += OnTargetExited;
    }

    private void OnDisable()
    {
        _targetDetectionArea.TargetEntered -= OnTargetEntered;
        _targetDetectionArea.TargetExited -= OnTargetExited;

        _nextDamageTimes.Clear();
        _targets.Clear();
    }

    private void Update()
    {
        _targets.Clear();

        foreach (KeyValuePair<IDamageable, float> target in _nextDamageTimes)
        {
            if (Time.time >= target.Value)
                _targets.Add(target.Key);
        }

        foreach (IDamageable target in _targets)
        {
            target.TakeDamage(_damage);
            _nextDamageTimes[target] = Time.time + _hitInterval;
        }
    }

    private void OnTargetEntered(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target) == false)
            return;

        if (_nextDamageTimes.ContainsKey(target))
            return;

        target.TakeDamage(_damage);
        _nextDamageTimes.Add(target, Time.time + _hitInterval);
    }

    private void OnTargetExited(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target) == false)
            return;

        foreach (Collider2D contact in _targetDetectionArea.Targets)
        {
            if (contact.TryGetComponent<IDamageable>(out IDamageable currentTarget) && currentTarget == target)
                return;
        }

        _nextDamageTimes.Remove(target);
    }
}