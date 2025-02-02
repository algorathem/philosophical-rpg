using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTarget : MonoBehaviour
{
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private Light pointLight;
    [SerializeField] public PickUpable shadowCaster;
    [SerializeField] public ShadowTrigger shadowCasterShadow;
    public LayerMask shadowCasterLayer;
    private float timer = 0.0f;
    private float timerInterval = 1.5f;
    private int fullyCoveredTimes = 0;
    private bool questCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        pointLight = FindObjectOfType<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!questCompleted)
        {
            // Check if the mesh collider is fully covered by the shadow every second
            timer += Time.deltaTime;

            if (timer >= timerInterval)
            {
                CheckFullyCovered();
                timer = 0.0f;
            }

            // Check if the quest is complete
            CheckQuestComplete();
        }

    }

    private bool IsMeshColliderCompletelyShadowed()
    {
        // Get the position of point light
        Vector3 lightPosition = pointLight.transform.position;

        // Get the vertices of mesh collider
        Mesh mesh = meshCollider.sharedMesh;
        Transform colliderTransform = meshCollider.transform;

        Vector3[] vertices = mesh.vertices;

        // Check if all vertices are shadowed
        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldVertex = colliderTransform.TransformPoint(vertex);
            Vector3 direction = lightPosition - worldVertex;
            Debug.DrawRay(worldVertex, direction, Color.red);

            // Cast a ray from the light position to the vertex
            if (!Physics.Raycast(worldVertex, lightPosition - worldVertex, Vector3.Distance(worldVertex, lightPosition), shadowCasterLayer))
            {
                // If a single ray reaches the light without being blocked, the collider is not fully covered
                return false;
            }
        }

        return true;  // All vertices are shadowed
    }

    private void CheckFullyCovered()
    {
        // If the shadow outer object is shadowed, do not count as fully covered
        Debug.Log("Is shadowed: " + shadowCasterShadow.isTriggered);
        if (shadowCasterShadow.isTriggered)
        {
            return;
        }

        // Check if the mesh collider is completely covered by the shadow
        if (IsMeshColliderCompletelyShadowed())
        {
            Debug.Log("The mesh collider is completely covered by the shadow for " + fullyCoveredTimes + " seconds.");
            fullyCoveredTimes++;
        }
        else
        {
            Debug.Log("The mesh collider is not fully covered.");
            fullyCoveredTimes = 0;
        }
    }

    // Put the actions to be performed when the quest is complete here
    private void OnQuestComplete()
    {
        Debug.Log("Quest complete!");

        // Set the quest as completed
        questCompleted = true;

        // Disable pickable property of the shadow caster
        shadowCaster.enabled = false;
        shadowCaster.isMovable = false;
        Rigidbody rb = shadowCaster.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void CheckQuestComplete()
    {
        if (fullyCoveredTimes >= 3)
        {
            OnQuestComplete();
        }
    }


}
