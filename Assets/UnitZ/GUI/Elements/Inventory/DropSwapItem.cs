//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSwapItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public GUIItemCollector GUIItem;
	
	public void Start ()
	{
		if (GUIItem == null)
			GUIItem = this.GetComponent<GUIItemCollector> ();
	}
	
	public void OnDrop (PointerEventData data)
	{
		if (GUIItem == null || GUIItem.Item == null)
			return;
		
		if (GUIItem.currentInventory) {
			GUIItemCollector itemDrop = GetDropSprite (data);
			if (itemDrop != null && itemDrop.Item != null) {
				
				ItemCollector tmp = new ItemCollector ();
				GUIItem.currentInventory.CopyCollector (tmp, GUIItem.Item);
				if ((GUIItem.Type == "Stock" || GUIItem.Type == "Inventory") && itemDrop.Type != "Shop") {
					if (Network.isClient || Network.isServer) {
						// Is Networking.
						if (GUIItem.currentInventory != itemDrop.currentInventory) {
							// Difference Inventory
						
							if (GUIItem.currentInventory.character && GUIItem.currentInventory.character.IsMine) {
								// swap to me
								GUIItem.currentInventory.PutCollector (itemDrop.Item, GUIItem.Item.InventoryIndex);
								itemDrop.currentInventory.PutCollectorSync (tmp, itemDrop.Item.InventoryIndex);
							} else {
								// swap to another
								GUIItem.currentInventory.PutCollectorSync (itemDrop.Item, GUIItem.Item.InventoryIndex);
								itemDrop.currentInventory.PutCollector (tmp, itemDrop.Item.InventoryIndex);
							}
						
						} else {
							// Same Inventory
						
							if (GUIItem.currentInventory.character && GUIItem.currentInventory.character.IsMine) {
								// is mine
								GUIItem.currentInventory.SwarpCollector (GUIItem.Item, itemDrop.Item);
							} else {
								// is other
								GUIItem.currentInventory.PutCollectorSync (itemDrop.Item, GUIItem.Item.InventoryIndex);
								itemDrop.currentInventory.PutCollectorSync (tmp, itemDrop.Item.InventoryIndex);
							}
						}
					
					} else {
						// Is Single Player
						GUIItem.currentInventory.SwarpCollector (GUIItem.Item, itemDrop.Item);
					}
				}
			}
		}
	}

	public void OnPointerEnter (PointerEventData data)
	{

	}

	public void OnPointerExit (PointerEventData data)
	{

	}
	
	private GUIItemCollector GetDropSprite (PointerEventData data)
	{
		var originalObj = data.pointerDrag;
		
		if (originalObj == null) {
			return null;
		}
		
		if (originalObj.GetComponent<GUIItemCollector> ()) {
			return originalObj.GetComponent<GUIItemCollector> ();
		}
		return null;
	}

}