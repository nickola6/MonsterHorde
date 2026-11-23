using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Resource : MonoBehaviour
{
    [SerializeField] private ResourceAnimation _animation;

    private Health _health;

    public event Action<Resource> Died;

    public Health Health => _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Died += OnDied;
    }

    private void OnDisable()
    {
        _health.Died -= OnDied;
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);
    }

    public void Restore()
    {
        _health.Restore();
    }

    public void PlayHit()
    {
        _animation.PlayHit();
    }

    private void OnDied()
    {
        Died?.Invoke(this);
    }
}