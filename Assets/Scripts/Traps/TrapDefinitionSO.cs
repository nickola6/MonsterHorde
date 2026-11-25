using UnityEngine;

[CreateAssetMenu(fileName = "TrapDefinition", menuName = "Game/Traps/Trap Definition")]
public class TrapDefinitionSO : ScriptableObject
{
    [SerializeField] private Trap _prefab;
    [SerializeField] private float _craftingTime = 1.5f;

    public Trap Prefab => _prefab;
    public float CraftingTime => _craftingTime;
}