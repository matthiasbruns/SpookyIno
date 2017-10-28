using UnityEngine;

[System.Serializable]    
public class ItemDrop : HasInventoryItem
{

    [Range(0.1f, 1.0f)]
    public float chance = 0.5f;

	public int amount = 1;
    public int Amount => amount;

	public int item;
    public int ItemId => item;
}