using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    private List<PlanetCollider> allPlanets = new List<PlanetCollider>();
    public GameObject winScreen;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        allPlanets.AddRange(FindObjectsOfType<PlanetCollider>());
        Debug.Log($"PuzzleManager found {allPlanets.Count} planets.");
    }

    public void CheckCompletion()
    {
        int pSize = allPlanets.Count;
        int count = 0;
        foreach (var planet in allPlanets)
        {
            if (planet.isPlacedCorrectly)
            {
                count++;
            }
        }
        Debug.Log(count + "/" + pSize + " Completed");
        foreach (var planet in allPlanets)
        {
            if (!planet.isPlacedCorrectly)
            {   

                Debug.Log("Puzzle not yet complete...");
                return;
            }
        }

        
        Debug.Log("All planets placed correctly!");
        OnPuzzleComplete();
    }

    private void OnPuzzleComplete()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }

        // Optional: freeze planets
        foreach (var planet in allPlanets)
        {
            planet.enabled = false;
        }

        // Optional: trigger audio, FX, next stage etc.
    }
}
