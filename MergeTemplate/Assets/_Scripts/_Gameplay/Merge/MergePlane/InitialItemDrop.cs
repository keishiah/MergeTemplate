using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InitialItemDrop : MonoBehaviour
{
    public bool showBoard = true;
    public int columns = 10;
    public int rows = 0;
    public List<ItemDropSlot> allDropSlots;

    [Inject]
    SlotsManager slotsManager;

    public void InitialItemInstance()
    {
        for (int i = 0; i < slotsManager.Slots.Count; i++)
        {
            if (allDropSlots[i].mergeItem != null)
            {
               
                slotsManager.Slots[i].ChangeState(allDropSlots[i].slotState);
                slotsManager.Slots[i].AddItem(allDropSlots[i].mergeItem);
            }
        }
    }
}

[Serializable]
public class ItemDropSlot
{
    public int id;
    public bool showBoard;
    public MergeItem mergeItem;
    public SlotState slotState = SlotState.Blocked;
}
