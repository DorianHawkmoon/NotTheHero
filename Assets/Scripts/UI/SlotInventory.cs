﻿using UnityEngine;
using UnityEngine.UI;

public class SlotInventory : MonoBehaviour {
    //data about the item
    public Item item;
    //Gameobjet background containing the image and other details like the count
    public GameObject background;
    //Gameobject dragable
    public GameObject dragable;

    //number of items left
    private int numberItems = 5;
    
    


	// Use this for initialization
	void Start () {
        Sprite itemSprite = item.GetSprite();
        background.GetComponent<Image>().sprite = itemSprite;
        dragable.GetComponent<Image>().sprite = itemSprite;
        dragable.GetComponent<DragHandler>().RegisterOnEndDrag(OnEndDragSlot);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnEndDragSlot(Vector3 position) {
        //put the item and substract the counter
        if (numberItems > 0) {
            Debug.Log("OnEndDragSlot.");
            
            //should ask if inside area in another class, for now, just put it
            GameObject itemObject = GameObject.Instantiate(item.gameObject);
            itemObject.transform.position = position;
            GameControllerTemporal.AddTemporal(itemObject);
        }
    }
}