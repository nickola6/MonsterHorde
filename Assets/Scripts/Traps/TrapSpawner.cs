using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public void Spawn(Trap prefab, Transform spawnPoint)
    {
        Trap trap = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        trap.Activate();
    }
}
