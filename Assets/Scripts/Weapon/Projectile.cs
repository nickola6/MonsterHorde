using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private DamageableTargetFilter _targetFilter;
    private float _damage;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (_targetFilter.Contains(other.gameObject) == false)
        {
            return;
        }

        if (other.TryGetComponent<IDamageable>(out IDamageable target) == false)
        {
            return;
        }

        target.TakeDamage(_damage);
        Destroy(gameObject);
    }

    public virtual void Initialize(Vector2 direction, float speed, float damage, Weapon sourceWeapon, DamageableTargetFilter targetFilter)
    {
        _damage = damage;
        _targetFilter = targetFilter;
        _rigidbody.velocity = direction.normalized * speed;
        transform.up = direction.normalized;
    }


}
