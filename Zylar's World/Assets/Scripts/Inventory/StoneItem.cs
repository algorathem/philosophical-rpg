using UnityEngine;

public class StoneItem : Item
{
    public override string GiveName()
    {
        return "Stone";
    }
    public override int MaxStacks()
    {
        return 5;
    }
    public override Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("Inventory/Stone Icon");
    }
}
