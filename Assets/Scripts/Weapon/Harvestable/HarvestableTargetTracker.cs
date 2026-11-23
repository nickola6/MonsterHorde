using System;
using System.Collections.Generic;

public class HarvestTargetTracker
{
    private readonly HashSet<Resource> _targets = new();

    public event Action<Resource> TargetEntered;
    public event Action<Resource> TargetExited;

    public IReadOnlyCollection<Resource> Targets => _targets;

    public void Add(Resource resource)
    {
        if (resource == null || _targets.Add(resource) == false)
            return;

        resource.Died += Remove;
        TargetEntered?.Invoke(resource);
    }

    public void Remove(Resource resource)
    {
        if (_targets.Remove(resource) == false)
            return;

        resource.Died -= Remove;
        TargetExited?.Invoke(resource);
    }

    public void Clear()
    {
        foreach (Resource resource in _targets)
            resource.Died -= Remove;

        _targets.Clear();
    }
}