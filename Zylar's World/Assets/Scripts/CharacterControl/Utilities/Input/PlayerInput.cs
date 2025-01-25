using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; }

    private void Awake()
    {
        InputActions = new PlayerInputActions();  // Create a new instance of the PlayerInputActions class
        PlayerActions = InputActions.Player;  // Get the PlayerActions from the PlayerInputActions class
    }

    private void OnEnable()
    {
        InputActions.Enable();  // Enable the input actions
    }

    private void OnDisable()
    {
        InputActions.Disable();  // Disable the input actions
    }

    public void DisableActionFor(InputAction action, float seconds)
    {
        StartCoroutine(DisableAction(action, seconds));
    }

    private IEnumerator DisableAction(InputAction action, float seconds)
    {
        action.Disable();

        yield return new WaitForSeconds(seconds);

        action.Enable();
    }
}
