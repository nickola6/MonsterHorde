using UnityEngine;

public class DamageableWeapon : Weapon
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private DamageableTargetFilter _targetFilter;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _projectileSpeed = 12f;
    [SerializeField] private float _projectileOffset = 0.5f;
    [SerializeField] private float _attackSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 720f;

    private Quaternion _initialLocalRotation;
    private Quaternion _targetRotation;
    private float _aimAngleOffset;
    private float _nextAttackTime;
    private bool _isAiming;

    public float AttackSpeed => _attackSpeed;
    public DamageableTargetFilter TargetFilter => _targetFilter;

    private void Awake()
    {
        _initialLocalRotation = _spriteRenderer.transform.localRotation;
        _aimAngleOffset = _spriteRenderer.transform.localEulerAngles.z;
    }

    private void Update()
    {
        if (_isAiming)
        {
            _spriteRenderer.transform.rotation = Quaternion.RotateTowards(_spriteRenderer.transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            return;
        }

        _spriteRenderer.transform.localRotation = Quaternion.RotateTowards(_spriteRenderer.transform.localRotation, _initialLocalRotation, _rotationSpeed * Time.deltaTime);
    }

    public void Aim(Vector2 direction)
    {
        if (direction.sqrMagnitude <= Mathf.Epsilon)
        {
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _targetRotation = Quaternion.Euler(0f, 0f, angle + _aimAngleOffset);
        _isAiming = true;
    }

    public void ResetAim()
    {
        _isAiming = false;
    }

    public void Attack(Vector2 direction)
    {
        if (direction.sqrMagnitude <= Mathf.Epsilon || Time.time < _nextAttackTime)
        {
            return;
        }

        Vector2 normalizedDirection = direction.normalized;
        Vector2 spawnPosition = (Vector2)_spriteRenderer.transform.position + normalizedDirection * _projectileOffset;
        Projectile projectile = Instantiate(_projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.Initialize(normalizedDirection, _projectileSpeed, Damage, this, _targetFilter);
        _nextAttackTime = Time.time + 1f / _attackSpeed;
    }
}
