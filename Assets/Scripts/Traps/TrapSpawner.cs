using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public void Spawn(Trap prefab, Transform spawnPoint)
    {
        Debug.Log($"Spawner: Prefab = {prefab}");
        Debug.Log($"Spawner: SpawnPoint = {spawnPoint}");

        Trap trap = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log($"Spawner: Created trap = {trap}");

        trap.Activate();

        Debug.Log("Spawner: Activated");
    }
}