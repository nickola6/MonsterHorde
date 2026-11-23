using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    [SerializeField] private TrapAppearance _trapPrefab;

    public void Spawn(Transform spawnPoint)
    {
        TrapAppearance trap = Instantiate(_trapPrefab, spawnPoint.position, spawnPoint.rotation);
        trap.Play();
    }
}