using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HarvestableArea : DetectionArea
{
    [SerializeField] private HarvestableWeapon _weapon;

    private readonly HarvestTargetTracker _tracker = new();
    private readonly List<Resource> _attackTargets = new();

    private float _nextAttackTime;

    public IReadOnlyCollection<Resource> Targets => _tracker.Targets;

    private void OnEnable()
    {
        _tracker.TargetEntered += OnResourceEntered;
    }

    private void OnDisable()
    {
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

    protected override void OnTargetEntered(Collider2D other)
    {
        if (other.TryGetComponent<Resource>(out var resource))
            _tracker.Add(resource);
    }

    protected override void OnTargetExited(Collider2D other)
    {
        if (other.TryGetComponent<Resource>(out var resource))
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