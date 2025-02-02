using UnityEngine;

public class WoodItem : Item
{
    public override string GiveName()
    {
        return "Wood";
    }
    public override int MaxStacks()
    {
        return 10;
    }
    public override Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("Inventory/Wood Icon");
    }
}
