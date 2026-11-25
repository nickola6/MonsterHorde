using UnityEngine;

[RequireComponent(typeof(DamageableTargetFilter))]
public abstract class DamageTrap : Trap
{
    [SerializeField] private float _damage = 39f;

    protected float Damage => _damage;
    protected DamageableTargetFilter TargetFilter { get; private set; }

    protected virtual void Awake()
    {
        TargetFilter = GetComponent<DamageableTargetFilter>();
    }
}