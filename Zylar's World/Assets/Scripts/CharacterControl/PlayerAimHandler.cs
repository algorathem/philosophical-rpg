using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimHandler : MonoBehaviour
{
    private PlayerInput playerInput; // Automatically created by the PlayerInput component
    private InputAction aimAction;

    private void Awake()
    {
        // Get the PlayerInput component
        playerInput = GetComponent<PlayerInput>();

        // Get the "Aim" action
        aimAction = playerInput.PlayerActions.Aim;

        // Subscribe to the action callbacks
        aimAction.performed += OnAimPerformed;
        aimAction.canceled += OnAimCanceled;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the action callbacks
        aimAction.performed -= OnAimPerformed;
        aimAction.canceled -= OnAimCanceled;
    }

    private void OnAimPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Right mouse button pressed (Aim started)");
    }

    private void OnAimCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Right mouse button released (Aim canceled)");
    }
}
