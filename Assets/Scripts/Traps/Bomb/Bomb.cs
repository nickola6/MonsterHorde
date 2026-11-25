
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Bomb : DamageTrap
{
    private CircleCollider2D _collider;

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TargetFilter.Contains(other.gameObject) == false)
            return;

        Vector2 center = transform.TransformPoint(_collider.offset);
        float scale = Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.y));
        float radius = _collider.radius * scale;

        Collider2D[] targets = Physics2D.OverlapCircleAll(center, radius);

        foreach (Collider2D targetCollider in targets)
        {
            if (TargetFilter.Contains(targetCollider.gameObject) == false)
                continue;

            if (targetCollider.TryGetComponent<IDamageable>(out IDamageable target))
                target.TakeDamage(Damage);
        }

        Destroy(gameObject);
    }
}
