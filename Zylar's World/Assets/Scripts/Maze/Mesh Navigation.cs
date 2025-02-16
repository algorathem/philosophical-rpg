using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeshNavigation : MonoBehaviour
{

    [SerializeField]
    private Vector3 desiredDestination;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject compass;

    [SerializeField]
    private GameObject compassDisplay;

    [SerializeField]
    private Button compassButton;

    [SerializeField]
    private TextMeshProUGUI DistanceDisplayText;

    [SerializeField]
    private GameObject DistanceDisplay;

    void Start()
    {
        GetComponent<NavMeshAgent>().destination = desiredDestination;
        Debug.Log("Path set");

        GetComponent<NavMeshAgent>().updatePosition = false;

        compassButton.onClick.AddListener(GetNextPath);
    }

    private void OnDestroy()
    {
        compassButton.onClick.RemoveListener(GetNextPath);
    }

    void GetNextPath()
    {
        GetComponent<NavMeshAgent>().nextPosition = player.transform.position;

        var path = ReCalculatePath();

        var pathLength = path.corners.Length;

        if (path != null && pathLength > 1)
        {

            float angleOfRotation = Mathf.Atan2((path.corners[1].z - path.corners[0].z), (path.corners[1].x - path.corners[0].x)) * Mathf.Rad2Deg - 90;
            Debug.Log($"Rotation angle: {angleOfRotation}");

            float zDistance = path.corners[pathLength - 1].z - path.corners[0].z;
            float xDistance = path.corners[pathLength - 1].x - path.corners[0].x;
            float distance = Mathf.Sqrt(zDistance * zDistance + xDistance * xDistance);

            DistanceDisplayText.text = $"Exit: {distance.ToString()}m away";

            transform.eulerAngles = new Vector3(0, angleOfRotation, 0);

            Vector3 relativePos = new Vector3(path.corners[pathLength - 1].x, path.corners[pathLength - 1].y, path.corners[pathLength - 1].z) - transform.position;

            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            compassDisplay.transform.rotation = rotation;

            StartCoroutine(ShowAndHide(DistanceDisplay, 1.0f));
            StartCoroutine(ShowAndHide(compassDisplay, 1.0f));

            //compass.transform.position = player.transform.position + new Vector3(0, 10, 0);
        }
        Debug.Log(path.corners[0].ToString());
        Debug.Log(path.corners[1].ToString());
    }

    void Update()
    {
        //Wait for 'P' key press to start pathfinding
        if (Input.GetKeyDown(KeyCode.P))
            {
                GetNextPath();
                //for (global::System.Int32 i = 0; i < path.corners.Length; i++)
                //{
                //    Debug.Log(path.corners[i].ToString());
                //}
                //transform.position = path.corners[1];
            }
    }

    NavMeshPath ReCalculatePath()
    {
        var agent = GetComponent<NavMeshAgent>();
        var path = new NavMeshPath();
        agent.CalculatePath(desiredDestination, path);

        return path;
    }

    IEnumerator ShowAndHide(GameObject obj, float delay)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
