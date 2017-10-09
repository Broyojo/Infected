//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]

public class ObjectPlacing : MonoBehaviour {
	
	public string ItemID = "";
	public string ItemData = "";
	public string ItemUID = "";
	
	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}
	
	public void SetItemID(string id){
		ItemID = id;
	}
	public void SetItemData(string data){
		ItemData = data;	
	}
	public void SetItemUID(string uid){
		ItemUID = uid;
	}
	void Start () {
		
	}
	


}
