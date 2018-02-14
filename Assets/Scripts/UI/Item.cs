//#define DEBUG_Item

using UnityEngine;

/// <summary>
/// Class to define an item in game menu inventory
/// </summary>
public class Item : MonoBehaviour {
    /// <summary>
    /// Sprite for using in menu
    /// </summary>
    [SerializeField]
    private Sprite spriteItemMenu;
    /// <summary>
    /// Main sprite of the item (used while draging the object)
    /// </summary>
    [SerializeField]
    private Sprite spriteItem;
    /// <summary>
    /// The item prefab to clone
    /// </summary>
    [SerializeField]
    private GameObject itemToClone;
    

    public Item(GameObject item, Sprite itemMenu, Sprite itemImage) {
#if DEBUG_Item
        Debug.Log("Item created.");
#endif
        itemToClone = item;
        spriteItem = itemImage;
        spriteItemMenu = itemMenu;
    }

    /// <summary>
    /// The item to be clone and put into game
    /// </summary>
    public GameObject ItemToClone {
        get { return itemToClone; }
    }

    /// <summary>
    /// The scale of the item
    /// </summary>
    public Vector3 ScaleItem {
        get { return itemToClone.transform.localScale; }
    }

    /// <summary>
    /// The sprite used in menu inventory game
    /// </summary>
    public Sprite SpriteMenu {
        get { return spriteItemMenu; }
    }

    /// <summary>
    /// The sprite of the item in game
    /// </summary>
    public Sprite SpriteItem {
        get { return spriteItem; }
    }
    
}

