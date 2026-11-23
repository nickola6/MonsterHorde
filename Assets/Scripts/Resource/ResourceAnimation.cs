using UnityEngine;

public abstract class ResourceAnimation : MonoBehaviour
{
    protected abstract void PlayAnimation();

    public void PlayHit()
    {
        PlayAnimation();
    }
}