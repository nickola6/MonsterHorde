
using System.Collections.Generic;
using UnityEngine;

public class ContactDamager : MonoBehaviour
{
    [SerializeField] private float _damage = 50f;
    [SerializeField] private float _hitInterval = 1f;

    private readonly Dictionary<Collider2D, IDamageable> _contacts = new();
    private readonly Dictionary<IDamageable, int> _contactCounts = new();
    private readonly Dictionary<IDamageable, float> _nextDamageTimes = new();
    private readonly List<IDamageable> _targets = new();

    private void OnDisable()
    {
        _contacts.Clear();
        _contactCounts.Clear();
        _nextDamageTimes.Clear();
        _targets.Clear();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out IDamageable target) == false)
            return;

        if (_contacts.ContainsKey(other))
            return;

        _contacts.Add(other, target);

        if (_contactCounts.ContainsKey(target))
        {
            _contactCounts[target]++;
            return;
        }

        _contactCounts.Add(target, 1);
        target.TakeDamage(_damage);
        _nextDamageTimes.Add(target, Time.time + _hitInterval);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_contacts.TryGetValue(other, out IDamageable target) == false)
            return;

        _contacts.Remove(other);
        _contactCounts[target]--;

        if (_contactCounts[target] > 0)
            return;

        _contactCounts.Remove(target);
        _nextDamageTimes.Remove(target);
    }
}
