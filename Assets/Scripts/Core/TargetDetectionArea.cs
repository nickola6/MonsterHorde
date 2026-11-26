using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class TargetDetectionArea : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayers;

    private readonly HashSet<Collider2D> _targets = new HashSet<Collider2D>();

    public event Action<Collider2D> TargetEntered;
    public event Action<Collider2D> TargetExited;

    public IReadOnlyCollection<Collider2D> Targets => _targets;

    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void OnDisable()
    {
        _targets.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((_targetLayers.value & (1 << other.gameObject.layer)) == 0)
        {
            return;
        }

        if (_targets.Add(other))
        TargetEntered?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_targets.Remove(other))
        TargetExited?.Invoke(other);
    }
}
