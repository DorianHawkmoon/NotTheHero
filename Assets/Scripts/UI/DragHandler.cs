using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    //only one item at time
    public static GameObject itemBeingDragged;


    private Vector3 startPosition;

    private Action<Vector3> onEndDragCallback;


    public void OnBeginDrag(PointerEventData eventData) {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        itemBeingDragged = null;
        transform.position = startPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        if (onEndDragCallback != null) {
            onEndDragCallback(position);
        }
    }


    public void RegisterOnEndDrag(Action<Vector3> callback) {
        onEndDragCallback += callback;
    }

    public void UnregisterOnEndDrag(Action<Vector3> callback) {
        onEndDragCallback -= callback;
    }
}
