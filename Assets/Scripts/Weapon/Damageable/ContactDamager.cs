using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamageableTargetFilter))]
public class ContactDamager : MonoBehaviour
{
    [SerializeField] private float _damage = 50f;
    [SerializeField] private float _hitInterval = 1f;

    private readonly Dictionary<Collider2D, IDamageable> _contacts = new();
    private readonly Dictionary<IDamageable, float> _nextDamageTimes = new();
    private readonly List<IDamageable> _targets = new();

    private DamageableTargetFilter _targetFilter;

    private void Awake()
    {
        _targetFilter = GetComponent<DamageableTargetFilter>();
    }

    private void OnDisable()
    {
        _contacts.Clear();
        _nextDamageTimes.Clear();
        _targets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_targetFilter.Contains(other.gameObject) == false)
            return;

        if (_contacts.ContainsKey(other))
            return;

        IDamageable target = other.GetComponent<IDamageable>();

        if (target == null)
            return;

        _contacts.Add(other, target);

        if (_nextDamageTimes.ContainsKey(target))
            return;

        target.TakeDamage(_damage);
        _nextDamageTimes.Add(target, Time.time + _hitInterval);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_contacts.TryGetValue(other, out IDamageable target) == false)
            return;

        _contacts.Remove(other);

        foreach (IDamageable contact in _contacts.Values)
        {
            if (contact == target)
                return;
        }

        _nextDamageTimes.Remove(target);
    }

    private void Update()
    {
        _targets.Clear();

        foreach (KeyValuePair<IDamageable, float> target in _nextDamageTimes)
        {
            if (Time.time >= target.Value)
                _targets.Add(target.Key);
        }

        foreach (IDamageable target in _targets)
        {
            target.TakeDamage(_damage);
            _nextDamageTimes[target] = Time.time + _hitInterval;
        }
    }
}
