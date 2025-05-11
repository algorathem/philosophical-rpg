using UnityEngine;
using Yarn.Unity;

public class YarnObjectSpawner: MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    public GameObject prefabToSpawn;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.AddCommandHandler("spawn", SpawnObject);
    }

    void SpawnObject()
    {
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No prefab assigned to spawn.");
        }
    }
}

