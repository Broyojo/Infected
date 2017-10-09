//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------
using UnityEngine;
using System.Collections;

public class CharacterHUDCanvas : PanelsManager
{
	public GUIIStockItemLoader SecondItemLoader;
	private CharacterLiving living;
	public GameObject Canvas;
	public ValueBar HPbar, FoodBar, WaterBar;
	
	void Start ()
	{
		// no script here.
	}

	void Awake ()
	{
		// make sure every panels are closed.
		if (Pages.Length > 0)
			ClosePanel (Pages [0]);
	}
	
	void InputController ()
	{
		// This is a GUI trigger function 
		// you can press E to open Inventroy or ESC to open mainmenu or etc.. here.
		
		if (Input.GetKeyDown (KeyCode.E)) {
			MouseLock.MouseLocked = !TogglePanelByName ("Inventory");	
		}
		
		if (Input.GetKeyDown (KeyCode.Escape)) {
			MouseLock.MouseLocked = !TogglePanelByName ("InGameMenu");	
		}
		
		if (Input.GetKeyDown (KeyCode.Tab)) {
			TogglePanelByName ("Scoreboard");	
		}
		
		// Show mouse cursor only when Game Menu , Inventory , Craft are still showing.
		if (IsPanelOpened ("InGameMenu") || IsPanelOpened ("Inventory") || IsPanelOpened ("Craft") || IsPanelOpened ("InventoryTrade")) {
			MouseLock.MouseLocked = false;
		} else {
			MouseLock.MouseLocked = true;
		}

	}
	
	void Update ()
	{
		if (UnitZ.playerManager == null || Canvas == null)
			return;

		if (UnitZ.playerManager.playingCharacter == null) {
			// if main character is die or unable to play
			living = null;
			Canvas.gameObject.SetActive (false); 
			
		} else {
			
			Canvas.gameObject.SetActive (true); 
			InputController ();
			
			// Update all GUI ValueBar here.
			// ValueBar working by assign the current value and max value.
			
			if (HPbar) {
				HPbar.Value = UnitZ.playerManager.playingCharacter.HP;	
				HPbar.ValueMax = UnitZ.playerManager.playingCharacter.HPmax;	
			}
			if (FoodBar && living) {
				FoodBar.Value = living.Hungry;	
				FoodBar.ValueMax = living.HungryMax;	
			}
			if (WaterBar && living) {
				WaterBar.Value = living.Water;	
				WaterBar.ValueMax = living.WaterMax;	
			}
			
			if (living == null)
				living = UnitZ.playerManager.playingCharacter.GetComponent<CharacterLiving> ();
		} 
	}
	
	public void OpenSecondInventory(CharacterInventory inventory,string type){
		if(IsPanelOpened("InventoryTrade")){
			ClosePanelByName("InventoryTrade");
		}else{
			SecondItemLoader.OpenInventory(inventory,type);
			OpenPanelByName("InventoryTrade");
		}
		
	}
	
	public void CloseSecondInventory(){
		ClosePanelByName("InventoryTrade");
	}
	
}
