using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    //only one item at time
    public static GameObject itemBeingDragged;
    private Image image;


    private Vector3 startPosition;

    private Action<Vector3> onEndDragCallback;
    private Action onStartDragCallback;

    public void Start() {
        image = GetComponent<Image>();
    }

    private GameObject DummieItem() {
        GameObject dummie = new GameObject();
        dummie.name = "draging object";
        SpriteRenderer spriteRenderer = dummie.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = image.sprite;
        GameControllerTemporal.AddTemporal(dummie);
        return dummie;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        itemBeingDragged = DummieItem();
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (onStartDragCallback != null) {
            onStartDragCallback();
        }
    }

    public void OnDrag(PointerEventData eventData) {
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        itemBeingDragged.transform.position = position;

        //TODO check collisions and tint it in red
    }

    public void OnEndDrag(PointerEventData eventData) {
        Destroy(itemBeingDragged); //TODO change to a pool of objects
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //TODO check if it's a good place
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

    public void RegisterOnStartDrag(Action callback) {
        onStartDragCallback += callback;
    }

    public void UnregisterOnStartDrag(Action callback) {
        onStartDragCallback -= callback;
    }
}
