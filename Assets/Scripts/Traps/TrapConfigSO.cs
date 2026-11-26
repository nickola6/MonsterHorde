using UnityEngine;

[CreateAssetMenu(fileName = "TrapConfig", menuName = "Game/Trap Config")]
public class TrapConfigSO : ScriptableObject
{
    [SerializeField] private Trap _prefab;

    public Trap Prefab => _prefab;
}
