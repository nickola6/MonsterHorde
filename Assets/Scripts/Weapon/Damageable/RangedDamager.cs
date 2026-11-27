using UnityEngine;

[RequireComponent(typeof(Follower))]
public class RangedDamager : MonoBehaviour
{
    [SerializeField] private TargetDetectionArea _targetDetectionArea;
    [SerializeField] private LineOfSight _lineOfSight;
    [SerializeField] private DamageableWeapon _weapon;

    private Follower _follower;

    private void Awake()
    {
        _follower = GetComponent<Follower>();
    }

    private void Update()
    {
        if (TryGetTargetDirection(out Vector2 direction))
        {
            _follower.StopFollowing();
            _weapon.Aim(direction);
            _weapon.Attack(direction);
            return;
        }

        _follower.ResumeFollowing();
        _weapon.ResetAim();
    }

    private bool TryGetTargetDirection(out Vector2 direction)
    {
        Collider2D closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D target in _targetDetectionArea.Targets)
        {
            if (target == null)
                continue;

            if (_lineOfSight.CanSee(target) == false)
                continue;

            float distance = ((Vector2)target.bounds.center - (Vector2)transform.position).sqrMagnitude;

            if (distance >= closestDistance)
                continue;

            closestDistance = distance;
            closestTarget = target;
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