//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIItemCollector : MonoBehaviour
{
	
	public ItemCollector Item;
	public Image Icon;
	public Text Num;
	public bool UpdateEnabled;
	[HideInInspector]
	public CharacterInventory currentInventory;
	public string Type = "";
	
	void Start ()
	{
		// no script here
	}
	
	public void SetItemCollector (ItemCollector item)
	{
		Item = item;
	}
	
	void FixedUpdate ()
	{
		// just update a GUI elements
		if(UpdateEnabled == Num)
			return;
		
		if (Item != null && Num != null && Icon != Num) {
			Icon.enabled = true;	
			Num.enabled = true;
			Num.text = Item.Num.ToString ();
			Icon.sprite = Item.Item.ImageSprite;
		}
		if (Item == null || (Item != null && Item.Num <= 0)) {
			Icon.enabled = false;
			if (Num != null)
				Num.enabled = false;
		}
	}
}
