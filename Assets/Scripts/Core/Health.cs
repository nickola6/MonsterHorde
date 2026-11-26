using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;

    private float _currentHealth;
    private bool _isDead;

    public event Action<float, float> HealthChanged;
    public event Action<float, float> DamageTaken;
    public event Action Died;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f || _isDead == true)
            return;

        _currentHealth = Mathf.Max(0f, _currentHealth - damage);

        HealthChanged?.Invoke(_currentHealth, _maxHealth);
        DamageTaken?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth > 0f)
            return;

        _isDead = true;

        Died?.Invoke();
    }

    public void Restore()
    {
        _currentHealth = _maxHealth;
        _isDead = false;
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
}
