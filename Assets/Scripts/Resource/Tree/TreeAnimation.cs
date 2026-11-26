using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TreeAnimation : ResourceAnimation
{
    private const string SwayingTrigger = "Swaying";

    [SerializeField] private ParticleSystem _leafParticles;
    [SerializeField] private ParticleSystem _woodChipsParticles;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void PlayAnimation()
    {
        _animator.SetTrigger(SwayingTrigger);
        _leafParticles.Play();
        _woodChipsParticles.Play();
    }
}
