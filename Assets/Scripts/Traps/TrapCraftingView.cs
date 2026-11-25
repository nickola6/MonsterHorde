using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TrapCraftingView : MonoBehaviour
{
    private Image _progressImage;

    private void Awake()
    {
        _progressImage = GetComponent<Image>();
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