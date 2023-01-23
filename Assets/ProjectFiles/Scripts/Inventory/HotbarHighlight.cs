using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarHighlight : MonoBehaviour
{
    Player player;

    public Transform hotbarParent;
    [SerializeField] TextMeshProUGUI itemNameText;
    InventorySlotUI[] slots;
    int currentHighlight = 0;
    
    void Start()
    {
        player = Player.instance;
        slots = hotbarParent.GetComponentsInChildren<InventorySlotUI>();

        ChangeItemName();

        player.onHotbarScrollCallback += ChangeHotbarSlot;
        player.onHotbarScrollCallback += ChangeItemName;

        player.onItemChangedCallback += ChangeHotbarSlot;
        player.onItemChangedCallback += ChangeItemName;
    }

    void ChangeItemName()
    {
        if(GetCurrentlyEquippedItem() != null)
        {
            itemNameText.gameObject.SetActive(true);
            itemNameText.SetText(GetCurrentlyEquippedItem().ItemName);
        }
        else
        {
            itemNameText.gameObject.SetActive(false);
        }
    }

    private void ChangeHotbarSlot()
    {
        Vector2 scrollDelta = player.controls.Player.Scroll.ReadValue<Vector2>();
        HighlightEnabled(false);

        if (scrollDelta.y > 0)
        {
            if (currentHighlight != 0)
            {
                currentHighlight--;
            }
            else
            {
                currentHighlight = slots.Length - 1;
            }
            
            HighlightEnabled(true);
        }
        else if(scrollDelta.y < 0)
        {
            currentHighlight++;
            if (currentHighlight > slots.Length - 1)
            {
                currentHighlight = 0;
            }

            HighlightEnabled(true);
        }
        //Highlight stayed where it was
        HighlightEnabled(true);
    }

    void HighlightEnabled(bool value)
    {
        for (int i = 0; i < slots[currentHighlight].GetComponent<Transform>().childCount; i++)
        {
            if (slots[currentHighlight].GetComponent<Transform>().GetChild(i).name == "Highlight")
            {
                slots[currentHighlight].GetComponent<Transform>().GetChild(i).GetComponent<Image>().enabled = value;
            }
        }
    }

    public SO_Item GetCurrentlyEquippedItem()
    {
        if(slots[currentHighlight].GetComponent<InventorySlotUI>().GetItem() != null)
        {
            return slots[currentHighlight].GetComponent<InventorySlotUI>().GetItem();
        }
        return null;
    }

    private void OnDisable()
    {
        player.onHotbarScrollCallback -= ChangeHotbarSlot;
        player.onHotbarScrollCallback -= ChangeItemName;

        player.onItemChangedCallback -= ChangeHotbarSlot;
        player.onItemChangedCallback -= ChangeItemName;
    }
}