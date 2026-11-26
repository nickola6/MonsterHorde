
using System.Collections;
using UnityEngine;

public class Bomb : DamageTrap
{
    private const float ExplosionDelay = 1f;
    private const float Cooldown = 5f;

    [SerializeField] private float _explosionRadius = 1.5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private CircleCollider2D _detectionCollider;
    private Color _originalColor;
    private bool _isReady = true;
    private Coroutine _attackRoutine;

    private void Awake()
    {
        base.Awake();
        _detectionCollider = GetComponent<CircleCollider2D>();
        _originalColor = _spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isReady == false || TargetFilter.Contains(other.gameObject) == false)
        {
            return;
        }

        _attackRoutine = StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _isReady = false;

        yield return new WaitForSeconds(ExplosionDelay);

        Explode();

        _spriteRenderer.color = Color.gray;

        yield return new WaitForSeconds(Cooldown);

        _spriteRenderer.color = _originalColor;
        _isReady = true;
        _attackRoutine = null;
    }

    private void Explode()
    {
        Vector2 center = transform.TransformPoint(_detectionCollider.offset);
        Collider2D[] targets = Physics2D.OverlapCircleAll(center, _explosionRadius);

        foreach (Collider2D targetCollider in targets)
        {
            if (TargetFilter.Contains(targetCollider.gameObject) == false)
            {
                continue;
            }

            IDamageable target = targetCollider.GetComponent<IDamageable>();

            target?.TakeDamage(Damage);
        }
    }

    private void OnDisable()
    {
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
        }

        _attackRoutine = null;
        _isReady = true;
    }
}