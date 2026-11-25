using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageableArea : DetectionArea
{
    public readonly struct TargetInfo
    {
        public Collider2D Collider { get; }
        public Vector2 Position { get; }
        public int Layer { get; }

        public TargetInfo(Collider2D collider)
        {
            Collider = collider;
            Position = collider.transform.position;
            Layer = collider.gameObject.layer;
        }
    }

    private readonly HashSet<Collider2D> _colliders = new();
    private readonly List<TargetInfo> _targets = new();

    public event Action<IReadOnlyList<TargetInfo>> TargetsUpdated;

    public IReadOnlyList<TargetInfo> Targets => _targets;

    private void LateUpdate()
    {
        _targets.Clear();

        foreach (Collider2D target in _colliders)
            if (target != null)
                _targets.Add(new TargetInfo(target));

        TargetsUpdated?.Invoke(_targets);
    }

    private void OnDisable()
    {
        _colliders.Clear();
        _targets.Clear();
    }

    protected override void OnTargetEntered(Collider2D other)
    {
        _colliders.Add(other);
    }

    protected override void OnTargetExited(Collider2D other)
    {
        _colliders.Remove(other);
    }

    private void OnDrawGizmosSelected()
    {
        if (TryGetComponent<CircleCollider2D>(out var circle) == false)
            return;

        Gizmos.color = Color.cyan;

        Matrix4x4 previousMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(circle.offset, circle.radius);
        Gizmos.matrix = previousMatrix;
    }
}