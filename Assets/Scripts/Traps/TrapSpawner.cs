using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public void Spawn(Trap prefab, Transform spawnPoint)
    {
        Debug.Log("Spawn: Before Instantiate");

        Trap trap = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        Debug.Log("Spawn: After Instantiate");

        trap.Activate();

        Debug.Log("Spawn: After Activate");
    }
}