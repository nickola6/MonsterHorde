using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;

    public float Damage => _damage;
}
