using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private float _damage;
    private bool _hasHit;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (_hasHit)
            return;

        _hasHit = true;

        if (other.TryGetComponent<IDamageable>(out IDamageable target))
            target.TakeDamage(_damage);

        Destroy(gameObject);
    }

    public virtual void Initialize(Vector2 direction, float speed, float damage)
    {
        _damage = damage;
        _hasHit = false;
        _rigidbody.velocity = direction.normalized * speed;
        transform.up = direction.normalized;
    }
}