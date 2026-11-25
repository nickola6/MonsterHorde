using UnityEngine;
using UnityEngine.UI;

public class TrapCraftingView : MonoBehaviour
{
    private Image _progressImage;

    private void Awake()
    {
        _progressImage = GetComponentInChildren<Image>();

        HideProgress();
    }

    public void SetProgress(float progress)
    {
        _progressImage.fillAmount = Mathf.Clamp01(progress);
        _progressImage.enabled = true;
    }

    public void HideProgress()
    {
        _progressImage.fillAmount = 0f;
        _progressImage.enabled = false;
    }

    public void HideVisuals()
    {
        gameObject.SetActive(false);
    }
}