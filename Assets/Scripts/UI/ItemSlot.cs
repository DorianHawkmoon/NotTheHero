using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    /// <summary>
    /// Data about the item
    /// </summary>
    public Item item;
    /// <summary>
    /// Only one item to be dragged at a time (for all instances)
    /// </summary>
    public static GameObject itemBeingDragged;
    public static SpriteRenderer rendererDragged;
    /// <summary>
    /// The image to show in menu
    /// </summary>
    private Image imageMenu;
    /// <summary>
    /// If the item is ready to use (TODO)
    /// </summary>
    private bool unlocked=true;


    public void Start() {
        if (item == null) {
            //destroy myself
            Destroy(gameObject);
        } else {
            imageMenu = GetComponent<Image>();
            imageMenu.sprite = item.GetSpriteMenu();
        }
    }

    private GameObject DummieItem() {
        GameObject dummie = new GameObject();
        dummie.name = "draging object";
        dummie.transform.localScale = item.GetScale();

        rendererDragged = dummie.AddComponent<SpriteRenderer>();
        rendererDragged.sprite = item.GetSprite();
        
        GameControllerTemporal.AddTemporal(dummie);
        return dummie;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        itemBeingDragged = DummieItem();
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
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

    public void OnEndDrag(PointerEventData eventData) {
        Destroy(itemBeingDragged); //TODO change to a pool of objects
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0;
        OnEndDragSlot(position);

    }

    private void OnEndDragSlot(Vector3 position) {
        //put the item and substract the counter
        if (unlocked && AllowedPlacement(position)) {
            Debug.Log("OnEndDragSlot.");
            
            GameObject itemObject = GameObject.Instantiate(item.ItemToClone);
            itemObject.transform.position = position;
            GameControllerTemporal.AddTemporal(itemObject);
        }
    }

    //TODO not working
    private bool AllowedPlacement(Vector3 position) {
        //Collider[] colliders = Physics.OverlapSphere(position, 1, layerCollision);
        //Debug.Log(colliders.Length);
        //return colliders.Length > 0;
        return true;
    }
    
}
