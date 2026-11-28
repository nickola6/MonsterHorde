using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [SerializeField] private HarvestArea _harvestableArea;
    [SerializeField] private TargetDetectionArea _targetDetectionArea;

    private Health _health;

    public IDamageable Health => _health;
    public TargetDetectionArea TargetDetectionArea => _targetDetectionArea;
    public IReadOnlyCollection<Resource> Resources => _harvestableArea.Targets;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }
}