using UnityEngine;

public abstract class HarvestWeapon : Weapon
{
    [SerializeField] private float _attackSpeed = 2f;

    public float AttackSpeed => _attackSpeed;

    public abstract void Attack();
}
