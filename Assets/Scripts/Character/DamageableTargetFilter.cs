using UnityEngine;

public class DamageableTargetFilter : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayers;

    public bool Contains(GameObject target)
    {
        return (_targetLayers.value & (1 << target.layer)) != 0;
    }
}