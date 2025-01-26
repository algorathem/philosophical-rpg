using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class PlayerUIUtility
{
    [field: Header("UI Settings")]
    [field: SerializeField] public Image selectCursor { get; private set; }
    [field: SerializeField] public Image aimCursor { get; private set; }

    public bool isAiming { get; set; } = false;
    public bool isAimCursorEnabled { get; set; } = false;
    public bool isSelecting { get; set; } = false;
    public bool isSelectCursorEnabled { get; set; } = false;

    public void Initialize()
    {
        aimCursor.enabled = false;
        selectCursor.enabled = false;

        // Hide and lock system cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EnableAimCursor()
    {
        isAimCursorEnabled = true;
        aimCursor.enabled = true;
    }

    public void DisableAimCursor()
    {
        isAimCursorEnabled = false;
        aimCursor.enabled = false;
    }

    public void EnableSelectCursor()
    {
        isSelectCursorEnabled = true;
        selectCursor.enabled = true;
    }

    public void DisableSelectCursor()
    {
        isSelectCursorEnabled = false;
        selectCursor.enabled = false;
    }

    public bool ShouldDisplayAimCursor()
    {
        if (isAiming)
        {
            if (isSelecting)
            {
                return false;
            }
            return true;
        }
        return false;

    }

    public bool ShouldDisplaySelectCursor()
    {
        if (isSelecting && isAiming)
        {
            return true;
        }
        return false;
    }
}
