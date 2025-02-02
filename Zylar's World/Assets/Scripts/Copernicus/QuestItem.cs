using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [SerializeField] float maxDistance = 5;
    [SerializeField] float distance;
    Player player;
    TempParent tempParent;
    private bool isHovered = false;
    public float outlineWidth = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        tempParent = TempParent.Instance;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
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

    private void OnMouseEnter()
    {
        if (tempParent != null)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= maxDistance)
            {
                player.uiUtility.isMessaging = true;
                player.uiUtility.DisableAimCursor();

                isHovered = true;
            }
        }
    }

    private void OnMouseExit()
    {
        player.uiUtility.DisableMessageCursor();
        isHovered = false;
    }


}
