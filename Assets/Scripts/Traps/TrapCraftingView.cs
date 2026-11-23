using UnityEngine;
using UnityEngine.UI;

public class TrapCraftingView : MonoBehaviour
{
    [SerializeField] private Image _progressImage;

    private void Awake()
    {
        Hide();
    }

    public void SetProgress(float progress)
    {
        _progressImage.fillAmount = Mathf.Clamp01(progress);
        _progressImage.enabled = true;
    }

    public void Hide()
    {
        _progressImage.fillAmount = 0f;
        _progressImage.enabled = false;
    }
}