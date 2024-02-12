using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

// Slot Script

public enum SlotState
{
    Draggable,
    NonTouchable,
    Blocked,
}


public class Slot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Image endLevelItemImage;
    [SerializeField] private int slotID;
    [SerializeField] private SlotState slotState;

    [SerializeField] private MergeItem item;
    [SerializeField] private int count;

    [Space, Header("State Images")]
    public Image blockedImage;
    public Image nontouchableImage;

    public int SlotID { get => slotID; set => slotID = value; }
    public SlotState SlotState { get => slotState; }
    //Delegates
    public delegate void SlotAddItemDelegate();
    public SlotAddItemDelegate addItemEvent;

    public delegate void SlotRemoveItemDelegate();
    public SlotRemoveItemDelegate removeItemEvent;

    public bool IsEmpty
    {
        get { return item == null; }
    }

    public MergeItem CurrentItem
    {
        get { return item; }
    }

    public void AddItem(MergeItem newItem)
    {
        item = newItem;
        addItemEvent?.Invoke();
        itemImage.sprite = item.itemSprite;
        itemImage.color = Color.white;

        if (!item.nextItem)
        {
            endLevelItemImage.enabled = true;
        }

    }

    public void RemoveItem()
    {
        item = null;
        itemImage.sprite = null;
        endLevelItemImage.enabled = false;
        itemImage.color = Color.clear;
        removeItemEvent?.Invoke();

    }

    private void UpgradeItem()
    {
        MergeItem newItem = item.nextItem;
        transform.parent.GetComponent<MergeGrid>().CheckNeighbour(this);
        RemoveItem();
        AddItem(newItem);
       
    }
    public DraggableItem currentDraggableItem { get => GetComponentInChildren<DraggableItem>(); }
    public void OnDrop(PointerEventData eventData)
    {
        if (slotState == SlotState.Blocked)
        {
            return;
        }

        GameObject dropped = eventData.pointerDrag;

        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        if (draggableItem.slot == this || draggableItem.slot.IsEmpty || draggableItem.slot.slotState != SlotState.Draggable)
        {
            return;
        }
        //  MergeItem tmpTo = CurrentItem;
        Slot tmpFrom = draggableItem.slot;

        if (CurrentItem == tmpFrom.CurrentItem)
        {
            //Debug.Log("merge");
            MergeItems(tmpFrom, this);
            ChangeState(SlotState.Draggable);
        }
        else
        {
            if (slotState == SlotState.Draggable)
            {
                if (!IsEmpty)
                {

                    MergeItem toItem = CurrentItem;
                    RemoveItem();
                    AddItem(tmpFrom.CurrentItem);

                    tmpFrom.RemoveItem();
                    tmpFrom.AddItem(toItem);

                }
                else
                {
                    MergeItem toItem = tmpFrom.CurrentItem;
                    tmpFrom.RemoveItem();

                    AddItem(toItem);
                }
            }
        }
        OnClick();
        currentDraggableItem.isClicked = true;
    }

    private void MergeItems(Slot slotFrom, Slot slotTo)
    {
        if (slotFrom.CurrentItem.nextItem == null)
        {
            //Debug.Log("Thats Max");
            return;
        }
        if (slotFrom.CurrentItem == slotTo.CurrentItem)
        {
            slotFrom.RemoveItem();
            slotTo.UpgradeItem();
        }
    }
    //bool isClicked = false;

    public void OnClick()
    {
        if (!IsEmpty)
        {
            if (SlotState == SlotState.Blocked)
            {
                return;
            }
            transform.parent.GetComponent<MergeGrid>().informationPanel.ConfigPanel(this);

            if (currentDraggableItem.isClicked)
            {
                CurrentItem.UseItem();
            }
            else
            {
                currentDraggableItem.isClicked = true;
            }
        }
        else
        {
            transform.parent.GetComponent<MergeGrid>().informationPanel.ActivateInfromPanel(false);
            currentDraggableItem.isClicked = false;
        }
    }

    public void ChangeState(SlotState m_slotState)
    {
        blockedImage.enabled = false;
        nontouchableImage.enabled = false;
        slotState = m_slotState;
        switch (slotState)
        {
            case SlotState.Blocked:
                blockedImage.enabled = true;
                break;
            case SlotState.Draggable:
                break;
            case SlotState.NonTouchable:
                nontouchableImage.enabled = true;
                break;
        }
    }

    public void DisableSelected()
    {
        //transform.parent.GetComponent<MergeGrid>().informationPanel.selectedItem.gameObject.SetActive(false);
        currentDraggableItem.isClicked = false;
    }

    public void UseItemInside()
    {
        if (!IsEmpty)
        {
            CurrentItem.UseItem();
        }
    }
}
