using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineBottleItem : Item
{
    // Start is called before the first frame update
    public override string GiveName()
    {
        return "Bottle";
    }
    public override int MaxStacks()
    {
        return 64;
    }
    public override Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("Inventory/Bottle Icon");
    }
}