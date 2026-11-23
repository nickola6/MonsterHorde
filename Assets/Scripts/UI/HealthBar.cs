using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _health;

    public event Action<float> HealthChanged;

    public float CurrentValue => _health.MaxHealth > 0f ? Mathf.Clamp01(_health.CurrentHealth / _health.MaxHealth) : 0f;

    private void OnEnable()
    {
        _health.DamageTaken += OnDamageTaken;
    }

    private void OnDisable()
    {
        _health.DamageTaken -= OnDamageTaken;
    }

    private void OnDamageTaken(float currentHealth, float maxHealth)
    {
        HealthChanged?.Invoke(maxHealth > 0f ? Mathf.Clamp01(currentHealth / maxHealth) : 0f);
    }
}