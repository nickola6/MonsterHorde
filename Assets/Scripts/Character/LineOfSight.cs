using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private const float VisibilityCheckInterval = 0.1f;

    private struct VisibilityState
    {
        public bool IsVisible;
        public float NextCheckTime;
    }

    [SerializeField] private LayerMask _obstacleLayers;

    private readonly Dictionary<Collider2D, VisibilityState> _visibility = new();

    private void OnDisable()
    {
        _visibility.Clear();
    }

    public bool CanSee(Collider2D target)
    {
        if (target == null)
            return false;

        if (_visibility.TryGetValue(target, out VisibilityState state) && Time.time < state.NextCheckTime)
            return state.IsVisible;

        Vector2 direction = (Vector2)target.bounds.center - (Vector2)transform.position;
        float distance = direction.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, distance, _obstacleLayers);
        bool isVisible = hit.collider == null || hit.collider == target;

        _visibility[target] = new VisibilityState
        {
            IsVisible = isVisible,
            NextCheckTime = Time.time + VisibilityCheckInterval
        };

        return isVisible;
    }
}