using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Spikes : DamageTrap
{
    private const int HitCount = 7;
    private const float HitInterval = 1f;
    private const float Cooldown = 10f;

    private bool _isBusy;
    private Coroutine _attackRoutine;

    private void OnDisable()
    {
        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);

        _attackRoutine = null;
        _isBusy = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryActivate(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        TryActivate(other);
    }

    private void TryActivate(Collider2D other)
    {
        if (_isBusy == true)
            return;

        if (TargetFilter.Contains(other.gameObject) == false)
            return;

        IDamageable target = other.GetComponent<IDamageable>();

        if (target == null)
            return;

        Component targetComponent = target as Component;

        if (targetComponent == null)
            return;

        _isBusy = true;
        _attackRoutine = StartCoroutine(AttackRoutine(target, targetComponent));
    }

    private IEnumerator AttackRoutine(IDamageable target, Component targetComponent)
    {
        WaitForSeconds hitWait = new WaitForSeconds(HitInterval);
        int remainingHits = HitCount;

        while (remainingHits > 0 && targetComponent != null)
        {
            target.TakeDamage(Damage);
            remainingHits--;

            if (remainingHits > 0)
                yield return hitWait;
        }

        yield return new WaitForSeconds(Cooldown);

        _isBusy = false;
        _attackRoutine = null;
    }
}
