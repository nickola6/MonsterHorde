using UnityEngine;

[CreateAssetMenu(fileName = "TrapConfig", menuName = "Game/Trap Config")]
public class TrapConfigSO : ScriptableObject
{
    [SerializeField] private Trap _prefab;
    [SerializeField] private float _craftingTime = 1.5f;

    public Trap Prefab => _prefab;
    public float CraftingTime => _craftingTime;
}