using UnityEngine;

public class AccessCardItem : Item
{
    public override string GiveName()
    {
        return "Access Card";
    }
    public override int MaxStacks()
    {
        return 1;
    }
    public override Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("Inventory/Access Card Icon");
    }
}
