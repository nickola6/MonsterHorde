using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider), typeof(CanvasGroup))]
public class HealthBarView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;

    private Slider _slider;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        _healthBar.HealthChanged += SetValue;
    }

    private void OnDisable()
    {
        _healthBar.HealthChanged -= SetValue;
    }

    private void SetValue(float value)
    {
        _slider.value = value;
        _canvasGroup.alpha = 1f;
    }
}