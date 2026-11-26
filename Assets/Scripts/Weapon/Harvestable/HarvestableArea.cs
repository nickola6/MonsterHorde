using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TargetDetectionArea))]
public class HarvestableArea : MonoBehaviour
{
    private TargetDetectionArea _targetDetectionArea;
    [SerializeField] private HarvestableWeapon _weapon;

    private readonly HarvestTargetTracker _tracker = new();
    private readonly List<Resource> _attackTargets = new();

    private float _nextAttackTime;

    public IReadOnlyCollection<Resource> Targets => _tracker.Targets;

    private void Awake()
    {
        _targetDetectionArea = GetComponent<TargetDetectionArea>();
    }

    private void OnEnable()
    {
        _targetDetectionArea.TargetEntered += OnTargetEntered;
        _targetDetectionArea.TargetExited += OnTargetExited;
        _tracker.TargetEntered += OnResourceEntered;
    }

    private void OnDisable()
    {
        _targetDetectionArea.TargetEntered -= OnTargetEntered;
        _targetDetectionArea.TargetExited -= OnTargetExited;
        _tracker.TargetEntered -= OnResourceEntered;
        _tracker.Clear();
        _attackTargets.Clear();
    }

    private void Update()
    {
        if (Targets.Count == 0 || Time.time < _nextAttackTime)
            return;

        PerformHarvest();
    }

    private void OnTargetEntered(Collider2D other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource))
            _tracker.Add(resource);
    }

    private void OnTargetExited(Collider2D other)
    {
        if (other.TryGetComponent<Resource>(out Resource resource))
            _tracker.Remove(resource);
    }

    private void OnResourceEntered(Resource resource)
    {
        if (Targets.Count == 1)
            PerformHarvest();
    }

    private void PerformHarvest()
    {
        _attackTargets.Clear();
        _attackTargets.AddRange(Targets);

        if (_attackTargets.Count == 0)
            return;

        _weapon.Attack();

        foreach (Resource resource in _attackTargets)
        {
            if (resource != null && Targets.Contains(resource))
                resource.TakeDamage(_weapon.Damage);

            if (resource != null && Targets.Contains(resource))
                resource.PlayHit();
        }

        _nextAttackTime = Time.time + 1f / _weapon.AttackSpeed;
    }
}
