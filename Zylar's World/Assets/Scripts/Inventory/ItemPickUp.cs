using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    public string itemToDrop;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Inventory playerInventory = other.GetComponentInChildren<Inventory>();
            if (playerInventory == null) Debug.Log("NULL");
            if (playerInventory != null ) PickUpItem(playerInventory);
        }
    }

    public void PickUpItem(Inventory playerInventory)
    {
        Debug.Log("Command Thru");
        amount = playerInventory.AddItem(itemToDrop, amount);
        if (amount < 1) Destroy(this.gameObject);
    }
}
