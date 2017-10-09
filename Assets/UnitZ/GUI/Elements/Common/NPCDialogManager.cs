using UnityEngine;
using System.Collections;

public class NPCDialogManager : MonoBehaviour {

	public GUIPlayerItemLoader PlayerItemLoader;
	public GUIIStockItemLoader SecondItemLoader;
	public CharacterHUDCanvas HUD;
	
	void Start () {
		HUD = (CharacterHUDCanvas)GameObject.FindObjectOfType(typeof(CharacterHUDCanvas));
		if(HUD){
			HUD.OpenPanelByName("InventoryStock");	
		}
	}
	
	public void OpenStock(ItemStocker stock){
		SecondItemLoader.OpenInventory(stock.inventory,"Stock");
		if(HUD)
		HUD.OpenPanelByName("InventoryStock");	
	}
	
	public void CloseStock(){
		if(HUD)
		HUD.ClosePanelByName("InventoryStock");	
	}
	
}
