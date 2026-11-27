using UnityEngine;

[CreateAssetMenu(fileName = "TargetConfig", menuName = "Configs/Target Config")]
public class TargetConfigSO : ScriptableObject
{
    [SerializeField] public LayerMask TargetLayers { get; private set; }
    [SerializeField] public LayerMask ObstacleLayers { get; private set; }
}