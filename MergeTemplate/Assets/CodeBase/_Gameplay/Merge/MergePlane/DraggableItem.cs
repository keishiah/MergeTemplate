using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class
    DraggableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler,
        IDragHandler //, IPointerClickHandler, IPointerExitHandler
{
    public Image image;
    public Slot slot;
    private Transform parentAfterDrag;
    public bool isClicked = false;
    private Camera _camera;
    private Vector3 startMousePosition;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slot.SlotState == SlotState.Draggable)
        {
            isClicked = false;
            if (slot != null)
            {
                slot = GetComponentInParent<Slot>();
            }

            parentAfterDrag = transform.parent;
            if (!slot.IsEmpty)
            {
                slot.OnClick();
                slot.DisableSelected();
                // if (Inp < 2)
                {
                    transform.SetParent(transform.parent.parent.parent);
                    transform.SetAsLastSibling();
                    image.raycastTarget = false;
                    startMousePosition = Input.mousePosition - transform.localPosition;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (slot.SlotState == SlotState.Draggable)
        {
            if (!slot.IsEmpty)
            {
                if (eventData.clickCount < 2)
                {
                    transform.localPosition
                        = Input.mousePosition - startMousePosition;
                    // Vector2 pos;
                    // RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.root as RectTransform,
                    //     eventData.position, _camera, out pos);
                    // transform.position = transform.root.TransformPoint(pos);
                }
            }
        }
    }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     if (slot.SlotState == SlotState.Draggable)
    //     {
    //         if (!slot.IsEmpty)
    //         {
    //             if (eventData.clickCount < 2)
    //             {
    //                 Vector2 pos;
    //                 RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.root as RectTransform,
    //                     eventData.position, _camera, out pos);
    //                 transform.position = transform.root.TransformPoint(pos);
    //             }
    //         }
    //     }
    // }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (slot.SlotState == SlotState.Draggable)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
    }
}