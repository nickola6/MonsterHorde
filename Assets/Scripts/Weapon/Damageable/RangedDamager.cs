using UnityEngine;

public class RangedDamager : MonoBehaviour
{
    [SerializeField] private DamageableArea _damageableArea;
    [SerializeField] private LineOfSight _lineOfSight;
    [SerializeField] private DamageableWeapon _weapon;

    private void Update()
    {
        if (TryGetTargetDirection(out Vector2 direction))
        {
            _weapon.Aim(direction);
            _weapon.Attack(direction);

            return;
        }

        _weapon.ResetAim();
    }

    private bool TryGetTargetDirection(out Vector2 direction)
    {
        Collider2D closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (DamageableArea.TargetInfo target in _damageableArea.Targets)
        {
            if (target.Collider == null || _weapon.TargetFilter.Contains(target.Collider.gameObject) == false)
                continue;

            if (_lineOfSight.CanSee(target.Collider) == false)
                continue;

            float distance = (target.Position - (Vector2)transform.position).sqrMagnitude;

            if (distance >= closestDistance)
                continue;

            if (target.Collider.GetComponent<IDamageable>() == null)
                continue;

            closestDistance = distance;
            closestTarget = target.Collider;
        }

        if (closestTarget == null)
        {
            direction = Vector2.zero;
            return false;
        }

        direction = (Vector2)closestTarget.bounds.center - (Vector2)_weapon.transform.position;
        return direction.sqrMagnitude > Mathf.Epsilon;
    }
}