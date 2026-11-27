using UnityEngine;

[CreateAssetMenu(fileName = "TargetConfig", menuName = "Configs/Target Config")]
public class TargetConfigSO : ScriptableObject
{
    [field: SerializeField] public LayerMask TargetLayers { get; private set; }
    [field: SerializeField] public LayerMask ObstacleLayers { get; private set; }
}