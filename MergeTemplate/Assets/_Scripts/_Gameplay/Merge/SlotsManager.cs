using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SlotsManager //: MonoBehaviour
{
    //[Inject]
    //private ItemsCatalogue itemsCatalogue;
    [SerializeField]
    private List<Slot> slots = new List<Slot>();


    [SerializeField]
    private int emptySlotsCount = 0;

    public int EmptySlotsCount { get { return slots.Count; } }

    public List<Slot> Slots { get { return slots; } }

    public void InitSlots(List<Slot> initiatedSlots)
    {
        foreach (Slot slot in initiatedSlots)
        {
            if (slot.IsEmpty) emptySlotsCount++;

            slot.addItemEvent += () =>
            {
                AddItemEvent();
                MergeItem item = slot.CurrentItem;
                //itemsCatalogue.AddItem(item, slot);
            };
            slot.removeItemEvent += () =>
            {
                RemoveItemEvent();
                MergeItem item = slot.CurrentItem;
                //itemsCatalogue.RemoveItem(item);
            };

            slots.Add(slot);


        }
    }

    //Test Func, Destroy On Release
    public void AddItemToEmptySlot(MergeItem mergeItem)
    {
        if (emptySlotsCount == 0)
        {
            Debug.Log("No empty slots");
            return;
        }

        List<Slot> m_slotsList = Slots.FindAll(slot => slot.IsEmpty);

        Slot m_slot = m_slotsList[Random.Range(0, m_slotsList.Count)];

        if (CheckSlotIsEmpty(m_slot))
        {
            m_slot.AddItem(mergeItem);
            return;
        }

    }

    public bool CheckSlotIsEmpty(Slot slot)
    {
        return slot.IsEmpty;
    }

    public void RemoveItem(MergeItem item)
    {
        Slot slot = slots.Find(x => x.CurrentItem == item);
        slot.RemoveItem();
    }

    public void RemoveAllItems()
    {
        foreach (Slot slot in slots)
        {
            slot.RemoveItem();
            slot.ChangeState(SlotState.Draggable);
        }
    }

    private void AddItemEvent()
    {
        emptySlotsCount--;

    }

    private void RemoveItemEvent()
    {
        emptySlotsCount++;

    }
}
