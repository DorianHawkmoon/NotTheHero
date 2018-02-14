//#define DEBUG_ItemSlot

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Class to control the behaviour of the slots in game inventory
/// </summary>
public class ItemSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    /// <summary>
    /// Data about the item
    /// </summary>
    public Item item;
    /// <summary>
    /// Only one item to be dragged at a time (for all instances)
    /// </summary>
    public static GameObject itemBeingDragged;
    /// <summary>
    /// The sprite to be used while dragged
    /// </summary>
    public static SpriteRenderer rendererDragged;
    /// <summary>
    /// The image to show in menu
    /// </summary>
    private Image imageMenu;
    /// <summary>
    /// If the item is ready to use (TODO when objects are not available yet)
    /// </summary>
    private bool unlocked=true;
    /// <summary>
    /// Layer the item will collide while dragging
    /// </summary>
    private int layerCollision;
    /// <summary>
    /// Layer the item will collide while dragging
    /// </summary>
    private int layerCollisionExtended;
    /// <summary>
    /// The overlap shape of the item
    /// </summary>
    private OverlapItem overlapItem;

    /// <summary>
    /// Start getting important information
    /// </summary>
    public void Start() {
        //in case there is no item, destroy the slot
        if (item == null) {
            #if DEBUG_ItemSlot
            Debug.Log("Slot destroyed.");
            #endif
            //destroy myself
            Destroy(gameObject);
        } else {
            imageMenu = GetComponent<Image>();
            imageMenu.sprite = item.SpriteMenu;

            overlapItem = item.ItemToClone.GetComponent<OverlapItem>();
            
            layerCollision = 0;
            LayerMask[] layers = overlapItem.LayersColliders;
            for (int i = 0; i < layers.Length; ++i) {
                int layer = Utils.ToLayer(layers[i]);
                layerCollision |= (1 << layer);
            }

            layerCollisionExtended = 0;
            layers = overlapItem.LayersCollidersExtended;
            for (int i = 0; i < layers.Length; ++i) {
                int layer = Utils.ToLayer(layers[i]);
                layerCollisionExtended |= (1 << layer);
            }
            
        }
    }

    /// <summary>
    /// Create a dummy object to show while the slot its being dragged
    /// </summary>
    /// <returns>Dummy object similar to item</returns>
    private GameObject DummieItem() {
        GameObject dummie = new GameObject("dragged object");
        //set scale
        dummie.transform.localScale = item.ScaleItem;
        dummie.layer = LayerMask.NameToLayer("Menu Items");
        //add the sprite to be shown
        rendererDragged = dummie.AddComponent<SpriteRenderer>();
        rendererDragged.sprite = item.SpriteItem;

        #if DEBUG_ItemSlot
        Debug.Log("Created dummie item.");
        #endif
        GameControllerTemporal.AddTemporal(dummie);
        return dummie;
    }

    /// <summary>
    /// When it starts dragging
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData) {
        #if DEBUG_ItemSlot
        Debug.Log("OnBeginDrag.");
        #endif
        itemBeingDragged = DummieItem();
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// While its being dragged
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData) {
        #if DEBUG_ItemSlot
        Debug.Log("OnDrag.");
        #endif
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = -1;
        itemBeingDragged.transform.position = position;

        //TODO check collisions and tint it in red
        position.z = 0;
        if (AllowedPlacement(position)) {
            rendererDragged.color = Color.white;
        } else {
            rendererDragged.color = Color.red;
        }
    }

    /// <summary>
    /// When the drag is finished, check if I could create the game object in game and do it
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData) {
        #if DEBUG_ItemSlot
        Debug.Log("OnEndDrag.");
        #endif
        Destroy(itemBeingDragged); //TODO change to a pool of objects
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        OnEndDragSlot(position);
    }

    /// <summary>
    /// Create, if allowed, the object in the game
    /// </summary>
    /// <param name="position"></param>
    private void OnEndDragSlot(Vector3 position) {
        //put the item and substract the counter
        if (unlocked && AllowedPlacement(position)) {
            GameObject itemObject = GameObject.Instantiate(item.ItemToClone);
            itemObject.transform.position = position;
            GameControllerTemporal.AddTemporal(itemObject);
        }
    }
    
    /// <summary>
    /// Tell if the object is allowed to be in the given position
    /// </summary>
    /// <param name="position">position to test</param>
    /// <returns>True if the object is allowed to be in the given position</returns>
    private bool AllowedPlacement(Vector3 position) {
        List<Collider> colliders = new List<Collider>();
        overlapItem.OverlapColliders(position, colliders, layerCollision);
        bool result = (colliders.Count <= 0);
        if (result) {
            overlapItem.OverlapCollidersExtended(position, colliders, layerCollisionExtended);
            result = colliders.Count <= 0;
        }
        
        #if DEBUG_ItemSlot
        if (result) {
            Debug.Log("Item placement allowed.");
        } else {
            Debug.Log("Item placement not allowed.");
        }
        #endif
        return result;
    }
    
}
