using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "SO/Item/Consumable")]
public class SO_Consumable : SO_Item
{
    public int RestoredHealthValue;
    public int RestoredEnduranceValue;
    private void Awake()
    {
        TypeOfItem = ItemType.Consumable;
        CanBeSold = true;
    }
}
