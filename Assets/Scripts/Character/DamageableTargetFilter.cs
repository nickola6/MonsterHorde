using UnityEngine;

public class DamageableTargetFilter : MonoBehaviour
{
    [SerializeField] private TargetConfigSO _config;

    public bool Contains(GameObject target)
    {
        return (_config.TargetLayers.value & (1 << target.layer)) != 0;
    }
}