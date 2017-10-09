//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIGameRoom : MonoBehaviour {
	
	public Text RoomName;
	public HostData hostData;
	
	void Start () {
	}

	public void JoinRoom(){
		if(UnitZ.gameClient){
			UnitZ.gameClient.GameSelected (hostData);
			PanelsManager panelsManage = (PanelsManager)GameObject.FindObjectOfType (typeof(PanelsManager));
			if(panelsManage){
				panelsManage.OpenPanelByName("LoadCharacter");
			}
		}
	}
}
