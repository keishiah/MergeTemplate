using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(GridLayoutGroup))]
public class MergeGrid : MonoBehaviour
{
    [SerializeField]
    public InformationPanel informationPanel;

    private List<Slot> instantiatedSlots = new List<Slot>();

    [SerializeField] 
    private Slot slotPrefab;

    [Inject]
    DiContainer _container;
    [Inject]
    public SlotsManager slotsManager;

    [Inject]
    private InitialItemDrop initialItemDrop;
    [Inject]
    private MergeItemsManager mergeItemsGeneralOpened;

    public List<Slot> InstantiatedSlots { get => instantiatedSlots; }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();

        mergeItemsGeneralOpened.LoadItemGeneralOpened();
        Input.multiTouchEnabled = false;
        LoadInventory();
    }

    private void CreateLayout()
    {
        instantiatedSlots = new List<Slot>();

        int slotsColumns = initialItemDrop.columns;
        int slotsRows = initialItemDrop.rows;

        int allSlots = slotsColumns * slotsRows;
        GridLayoutGroup gridLayoutGroup = GetComponent<GridLayoutGroup>();

        gridLayoutGroup.constraintCount = slotsColumns;

        for (int x = 0; x < allSlots; x++)
        {
            Slot initSlot = _container.InstantiatePrefabForComponent<Slot>(slotPrefab);

            initSlot.addItemEvent += SaveInventory;
            initSlot.removeItemEvent += SaveInventory;

            RectTransform slotRect = initSlot.GetComponent<RectTransform>();

            initSlot.SlotID = x;
            initSlot.name = $"Slot: {x}";
            initSlot.transform.SetParent(transform);

            slotRect.localPosition = Vector3.zero;
            slotRect.localRotation = Quaternion.Euler(Vector3.zero);

            slotRect.localScale = Vector3.one;

            instantiatedSlots.Add(initSlot);
        }
    }

    public void SaveInventory()
    {
        string content = string.Empty;

        for (int i = 0; i < instantiatedSlots.Count; i++)
        {
            Slot slot = instantiatedSlots[i];
            if (!slot.IsEmpty)
            {
                content += i + "-" + slot.CurrentItem.name.ToString() + "-" + (int)slot.SlotState + ";";
            }

        }
        PlayerPrefs.SetString("mergeContent", content);
        PlayerPrefs.Save();
    }

    public void CheckNeighbour(Slot m_Slot)
    {
        int slotID = m_Slot.SlotID;
        if (slotID % 7 != 6)
        {
            if (instantiatedSlots[slotID + 1].SlotState == SlotState.Blocked)
            {
                instantiatedSlots[slotID + 1].ChangeState(SlotState.NonTouchable);
            }
        }

        if (slotID % 7 != 0)
        {
            if (instantiatedSlots[slotID - 1].SlotState == SlotState.Blocked)
            {
                instantiatedSlots[slotID - 1].ChangeState(SlotState.NonTouchable);
            }
        }

        if (slotID - 7 >= 0)
        {
            if (instantiatedSlots[slotID - 7].SlotState == SlotState.Blocked)
            {
                instantiatedSlots[slotID - 7].ChangeState(SlotState.NonTouchable);
            }
        }

        //if (slotID + 7 < slotsColumns)
        //{
        //    if (instantiatedSlots[slotID + 7].SlotState == SlotState.Blocked)
        //    {
        //        instantiatedSlots[slotID + 7].ChangeState(SlotState.NonTouchable);
        //    }
        //}
    }

    public void LoadInventory()
    {
        CreateLayout();
        slotsManager.InitSlots(InstantiatedSlots);

        if (PlayerPrefs.HasKey("mergeContent"))
        {
            string content = PlayerPrefs.GetString("mergeContent");
            try
            {
                string[] splitedContent = content.Split(';');

                for (int i = 0; i < splitedContent.Length - 1; i++)
                {
                    string[] splitedValue = splitedContent[i].Split('-');
                    int index = int.Parse(splitedValue[0]);
                    int slotState = int.Parse(splitedValue[2]);
                    instantiatedSlots[index].ChangeState((SlotState)slotState);
                    instantiatedSlots[index].AddItem(Resources.Load<MergeItem>($"Items/{splitedValue[1]}"));
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Something went wrong");
                Debug.LogException(ex);

                initialItemDrop.InitialItemInstance();
            }
        }
        else
        {
            initialItemDrop.InitialItemInstance();
        }
    }

}


