using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class QuestItem : MonoBehaviour
{
    [SerializeField] float maxDistance = 5;
    [SerializeField] float distance;
    Player player;
    public Transform cameraTarget;
    public CinemachineVirtualCamera vcam;
    private CinemachineFramingTransposer framingTransposer;

    private bool isHovered = false;
    private bool isInteracted = false;
    public float outlineWidth = 5.0f;
    private float originalCameraSpeed;
    private float newCameraSpeed = 0f;
    private float originalCameraDistance;
    private float newCameraDistance = 3f;
    private float targetCameraDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        originalCameraDistance = framingTransposer.m_CameraDistance;
        targetCameraDistance = originalCameraDistance;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if isHovered, apply outline
        OutlineOnHover();

        // Key E
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Key E pressed");
            OnKeyE();
        }
    }

    private void LateUpdate()
    {
        framingTransposer.m_CameraDistance = targetCameraDistance;
    }

    private void OnMouseEnter()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= maxDistance)
        {
            player.uiUtility.isMessaging = true;
            player.uiUtility.DisableAimCursor();

            isHovered = true;
        }

    }

    private void OnMouseExit()
    {
        player.uiUtility.DisableMessageCursor();
        isHovered = false;
    }

    private void OutlineOnHover()
    {
        if (isHovered)
        {
            if (GetComponent<Outline>() != null)
            {
                GetComponent<Outline>().enabled = true;
            }
            else
            {
                Outline outline = gameObject.AddComponent<Outline>();
                outline.enabled = true;
                outline.OutlineColor = Color.white;
                outline.OutlineWidth = outlineWidth;
            }
        }
        else
        {
            if (GetComponent<Outline>() != null)
            {
                GetComponent<Outline>().enabled = false;
            }
        }
    }

    private void OnKeyE()
    {
        if (isHovered && !isInteracted)
        {
            // Toggle isInteracted
            isInteracted = !isInteracted;

            // Disable player input
            player.playerInput.InputActions.Disable();

            // Toggle camera target
            Transform newTarget = cameraTarget;
            vcam.Follow = newTarget;
            vcam.LookAt = newTarget;

            // Toggle camera rotate speed
            originalCameraSpeed = vcam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed;
            vcam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = newCameraSpeed;
            vcam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = newCameraSpeed;

            // Toggle camera distance
            DOTween.To(() => targetCameraDistance, x => targetCameraDistance = x, newCameraDistance, 0.5f).SetEase(Ease.InOutQuad);
        }
        else if (isInteracted)
        {
            // Toggle isInteracted
            isInteracted = !isInteracted;

            // Toggle camera target
            Transform newTarget = player.cameraUtility.cameraTarget;
            vcam.Follow = newTarget;
            vcam.LookAt = newTarget;

            // Toggle camera rotate speed
            vcam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = originalCameraSpeed;
            vcam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = originalCameraSpeed;
            player.playerInput.InputActions.Enable();

            // Toggle camera distance
            DOTween.To(() => targetCameraDistance, x => targetCameraDistance = x, originalCameraDistance, 0.5f).SetEase(Ease.InOutQuad);
        }
    }


}
