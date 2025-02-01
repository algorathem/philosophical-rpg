using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class PlayerUIUtility
{
    [field: Header("UI Settings")]
    [field: SerializeField] public Image selectCursor { get; private set; }
    [field: SerializeField] public Image aimCursor { get; private set; }
    [field: SerializeField] public Image hoverCursor { get; private set; }
    [field: SerializeField] public GameObject messageCursor { get; private set; }


    public bool isAiming { get; set; } = false;
    public bool isAimCursorEnabled { get; set; } = false;
    public bool isSelecting { get; set; } = false;
    public bool isSelectCursorEnabled { get; set; } = false;
    public bool isHovering { get; set; } = false;
    public bool isMessaging { get; set; } = false;

    public void Initialize()
    {
        aimCursor.enabled = false;
        selectCursor.enabled = false;
        hoverCursor.enabled = false;
        messageCursor.SetActive(false);

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

    public void EnableHoverCursor()
    {
        isHovering = true;
        hoverCursor.enabled = true;
    }

    public void DisableHoverCursor()
    {
        isHovering = false;
        hoverCursor.enabled = false;
    }

    public void EnableMessageCursor()
    {
        isMessaging = true;
        messageCursor.SetActive(true);
    }

    public void DisableMessageCursor()
    {
        isMessaging = false;
        messageCursor.SetActive(false);
    }

    public bool ShouldDisplayAimCursor()
    {
        if (isAiming)
        {
            if (isSelecting || isHovering || isMessaging)
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

    public bool ShouldDisplayHoverCursor()
    {
        // should only be shown when in aiming mode
        if (isAiming)
        {
            if (isHovering && !isSelecting)
            {
                return true;
            }
        }
        return false;
    }

    public bool ShouldDisplayMessageCursor()
    {
        // should only be shown when in aiming mode
        if (isAiming)
        {
            if (isMessaging)
            {
                return true;
            }
        }
        return false;
    }

}
