using UnityEngine;
using TMPro;
using Yarn.Unity;
using System.Collections.Generic;

public class YarnObjectActivator : MonoBehaviour
{
    [System.Serializable]
    public class Activatable
    {
        public string id; // identifier used in Yarn
        public GameObject gameObject; // full object
        public TextMeshProUGUI textElement; // optional
    }

    public List<Activatable> activatables;

    private DialogueRunner dialogueRunner;
    private Dictionary<string, Activatable> lookup;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueRunner.AddCommandHandler<string>("activate", ActivateById);

        // Cache for performance
        lookup = new Dictionary<string, Activatable>();
        foreach (var a in activatables)
        {
            if (!string.IsNullOrEmpty(a.id))
                lookup[a.id] = a;
        }
    }

    void ActivateById(string id)
    {
        if (lookup.TryGetValue(id, out var target))
        {
            if (target.gameObject != null)
                target.gameObject.SetActive(true);
            if (target.textElement != null)
                target.textElement.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"No activatable object found with ID '{id}'");
        }
    }
}
