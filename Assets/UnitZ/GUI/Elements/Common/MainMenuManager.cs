//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : PanelsManager
{
	public string SceneStart = "zombieland";
	public CharacterCreatorCanvas characterCreator;
	public Text CharacterName;
	public GameObject Preloader;

	void Start ()
	{
		// setup all necessary parameters.
		Application.targetFrameRate = 140;
		characterCreator = (CharacterCreatorCanvas)GameObject.FindObjectOfType (typeof(CharacterCreatorCanvas));
		
		// load latest scene played
		if (PlayerPrefs.GetString ("StartScene") != "") {
			SceneStart = PlayerPrefs.GetString ("StartScene");
		}
	}
	
	void Update ()
	{
		if (CharacterName && UnitZ.gameManager) {
			CharacterName.text = UnitZ.gameManager.UserName;
		}
		// show preloader if the game is connecting
		if (UnitZ.gameClient) {
			if (UnitZ.gameClient.isConnecting) {
				if (Preloader) {
					Preloader.SetActive (UnitZ.gameClient.isConnecting);
				}
			}
		}
	}

	public void LevelSelected (string name)
	{
		SceneStart = name;
		// save selected scene for the next round.
		PlayerPrefs.SetString ("StartScene", SceneStart);
	}
	
	public void ConnectIP ()
	{
		OpenPanelByName ("LoadCharacter");
	}
	
	public void HostGame ()
	{
		if (UnitZ.gameManager) {
			UnitZ.gameManager.CreateGame (SceneStart,true);
		}
	}
	
	public void UseMasterServer (bool masterserver)
	{
		if (UnitZ.gameServer)
			UnitZ.gameServer.LanOnly = !masterserver;
	}
	
	public void StartSinglePlayer ()
	{
		if (UnitZ.gameManager) {
			UnitZ.gameManager.CreateGame (SceneStart,false);
			OpenPanelByName ("LoadCharacter");
		}
	}
	
	public void StartNetworkGame ()
	{
		if (UnitZ.gameManager) {
			UnitZ.gameManager.CreateGame (SceneStart,true);
			OpenPanelByName ("LoadCharacter");
		}		
	}

	public void EnterWorld ()
	{
		if (UnitZ.gameManager) {
			if (characterCreator){
				characterCreator.SetCharacter ();
			}
			UnitZ.gameManager.StartGame (SceneStart);
		}
		OpenPanelByName ("Connecting");
	}
	
	public void ConnectingDeny ()
	{
		UnitZ.gameManager.ConnectingDeny ();
	}
	
	public void ExitGame ()
	{
		Application.Quit ();	
	}
}
