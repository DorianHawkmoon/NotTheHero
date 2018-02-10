using UnityEngine;

public class Inventory : MonoBehaviour {

    public GameObject[] itemsInventory;

    public GameObject slotPrefab;

	// Use this for initialization
	void Start () {
        //iterate all items and create slots for them
        foreach (GameObject itemObject in itemsInventory) {
            Item item = itemObject.GetComponent<Item>();
            if (item == null) continue;

            GameObject slot = Instantiate<GameObject>(slotPrefab, transform);
            ItemSlot itemSlot = slot.GetComponentInChildren<ItemSlot>();
            itemSlot.item = item;
        }
	}
}