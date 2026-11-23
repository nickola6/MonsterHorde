using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [SerializeField] private HarvestableArea _harvestableArea;
    [SerializeField] private DamageableArea _damageableArea;

    private Health _health;

    public Health Health => _health;
    public DamageableArea DamageableArea => _damageableArea;
    public IReadOnlyCollection<Resource> Resources => _harvestableArea.Targets;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }
}