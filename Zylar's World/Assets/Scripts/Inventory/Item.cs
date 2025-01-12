using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public abstract string GiveName();
    public virtual int MaxStacks()
    {
        return 30;
    }
    public virtual Sprite GiveItemImage()
    {
        return Resources.Load<Sprite>("Inventory/No Item Image Icon");
    }
}
