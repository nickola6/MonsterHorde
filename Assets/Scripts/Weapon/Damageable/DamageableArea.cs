using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(LineRenderer))]
public class DamageableArea : DetectionArea
{
    private const int CircleSegments = 64;
    private const float CircleRadians = Mathf.PI * 2f;

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

    [SerializeField] private Color _rangeColor = new(0.55f, 0.68f, 0.72f, 0.25f);
    [SerializeField] private float _lineWidth = 0.025f;

    private readonly HashSet<Collider2D> _colliders = new();
    private readonly List<TargetInfo> _targets = new();

    private CircleCollider2D _circleCollider;
    private LineRenderer _rangeRenderer;

    public event Action<IReadOnlyList<TargetInfo>> TargetsUpdated;

    public IReadOnlyList<TargetInfo> Targets => _targets;

    private void Awake()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _rangeRenderer = GetComponent<LineRenderer>();

        InitializeRange();
    }

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

    private void InitializeRange()
    {
        _rangeRenderer.positionCount = CircleSegments;
        _rangeRenderer.useWorldSpace = false;
        _rangeRenderer.loop = true;
        _rangeRenderer.startWidth = _lineWidth;
        _rangeRenderer.endWidth = _lineWidth;
        _rangeRenderer.startColor = _rangeColor;
        _rangeRenderer.endColor = _rangeColor;

        for (int i = 0; i < CircleSegments; i++)
        {
            float angle = CircleRadians * i / CircleSegments;

            Vector3 position = _circleCollider.offset + new Vector2(
                Mathf.Cos(angle) * _circleCollider.radius,
                Mathf.Sin(angle) * _circleCollider.radius
            );

            _rangeRenderer.SetPosition(i, position);
        }
    }
}