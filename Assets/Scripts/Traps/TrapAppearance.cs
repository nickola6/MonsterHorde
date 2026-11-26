using System.Collections;
using UnityEngine;

public class TrapAppearance : MonoBehaviour
{
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _rotation = 360f;
    [SerializeField] private float _initialScale = 0.1f;

    private Vector3 _targetScale;
    private Quaternion _targetRotation;
    private Coroutine _appearanceRoutine;

    private void Awake()
    {
        _targetScale = transform.localScale;
        _targetRotation = transform.rotation;
    }

    public void Play()
    {
        if (_appearanceRoutine != null)
        StopCoroutine(_appearanceRoutine);

        _appearanceRoutine = StartCoroutine(AppearanceRoutine());
    }

    private IEnumerator AppearanceRoutine()
    {
        float elapsedTime = 0f;
        Vector3 initialScale = _targetScale * _initialScale;
        transform.localScale = initialScale;

        while (elapsedTime < _duration)
        {
            float progress = elapsedTime / _duration;
            float easedProgress = Mathf.SmoothStep(0f, 1f, progress);

            transform.localScale = Vector3.LerpUnclamped(initialScale, _targetScale, easedProgress);
            transform.rotation = _targetRotation * Quaternion.Euler(0f, 0f, _rotation * progress);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localScale = _targetScale;
        transform.rotation = _targetRotation;
        _appearanceRoutine = null;
    }
}
