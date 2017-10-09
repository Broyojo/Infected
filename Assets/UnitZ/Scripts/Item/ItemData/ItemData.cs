//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]

public class ItemData : MonoBehaviour
{
	//public Texture2D Image;
	public Sprite ImageSprite;
	public string ItemName;
	public string Description;
	public int Price;
	public bool Stack = true;
	public FPSItemEquipment ItemFPS;
	public ItemEquipment ItemEquip;
	public int Quantity = 1;
	[HideInInspector]
	public int NumTag = -1;
	public AudioClip SoundPickup;
	public string ItemID;
	public NetworkView networkViewer;
	
	public virtual void Pickup (CharacterSystem character)
	{
		if (character != null && character.inventory != null) {
			if (character.inventory.AddItemTest (this, Quantity)) {
				character.inventory.AddItemByItemData (this, Quantity, NumTag, -1);
				if (SoundPickup) {
					AudioSource.PlayClipAtPoint (SoundPickup, this.transform.position);	
				}
				RemoveItem ();
			}
		}
	}
	
	public void RemoveItem ()
	{
		if (Network.isClient) {
			networkViewer.RPC ("removeItem", RPCMode.Server, null);
		} else {
			if (Network.isServer) {
				Network.Destroy (this.gameObject);
				if (networkViewer)
					Network.RemoveRPCs (networkViewer.viewID);
			} else {
				GameObject.Destroy (this.gameObject);
			}
		}
	}
	
	void Awake ()
	{
		DontDestroyOnLoad (this.gameObject.transform.root);
		networkViewer = this.GetComponent<NetworkView> ();
	}
	
	void Start ()
	{
		if (networkViewer && networkViewer.isMine && (Network.isServer || Network.isClient)) {
			networkViewer.RPC ("GetData", RPCMode.Others, NumTag, Quantity);
		}
	}
	
	[RPC]
	void removeItem ()
	{
		Network.Destroy (this.gameObject);
		if (networkViewer)
			Network.RemoveRPCs (networkViewer.viewID);
	}
	
	[RPC]
	void GetData (int numtag, int quantity)
	{
		NumTag = numtag;
		Quantity = quantity;
	}
	
	public void FixedUpdate ()
	{
		ShowInfo = false;	
	}
	
	public void GetInfo ()
	{
		ShowInfo = true;
	}
	
	public bool ShowInfo;
	
	void OnGUI ()
	{
		if (ShowInfo) {
			Vector3 screenPos = Camera.main.WorldToScreenPoint (this.gameObject.transform.position);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label (new Rect (screenPos.x, Screen.height - screenPos.y, 200, 60), "Press F to Pickup\n" + ItemName + " x " + Quantity);
		}
	}
}
