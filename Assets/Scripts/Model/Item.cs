using UnityEngine;

//monobehaviour temporal to use in inspector
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
    /// <summary>
    /// Aproximately radius of the item
    /// </summary>
    //public float radiusGameObject;
    /// <summary>
    /// The original scale of the item
    /// </summary>
    //private Vector3 originalScaleItem = Vector3.one;

    public GameObject ItemToClone {
        get { return itemToClone; }
        set {
            ItemToClone = value;
            //originalScaleItem = ItemToClone.transform.localScale;
        }
    }

    //public Vector3 ScaleItem {
    //    get { return originalScaleItem; }
    //    set { originalScaleItem = value; }
    //}

    public Vector3 GetScale() {
        return itemToClone.transform.localScale;
    }

    public Sprite GetSprite() {
        return spriteItem;
    }

    public Sprite GetSpriteMenu() {
        return spriteItemMenu;
    }
    
}

