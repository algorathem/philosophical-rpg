using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour
{
    [SerializeField] float maxDistance = 5;
    [SerializeField] float distance;
    Player player;
    TempParent tempParent;
    // Start is called before the first frame update
    void Start()
    {
        tempParent = TempParent.Instance;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

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
            }
        }
    }

    private void OnMouseExit()
    {
        player.uiUtility.DisableMessageCursor();
    }


}
