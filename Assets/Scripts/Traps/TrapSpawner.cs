
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public void Spawn(Trap prefab, Transform spawnPoint)
    {
        Debug.Log("TrapSpawner: Before Instantiate");

        Trap trap = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log("TrapSpawner: After Instantiate");

        trap.Activate();

        Debug.Log("TrapSpawner: After Activate");
    }
}
