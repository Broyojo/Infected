using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
 
class EditorSetting : EditorWindow
{
	private GameObject gameManager;
	private GameServer gameServer;
	private UnitZManager unitZmanager;
	
	[MenuItem ("Window/UnitZ/Settings")]
	public static void  ShowSetting ()
	{
		EditorWindow.GetWindow (typeof(EditorSetting));
	}
	
	void loadData ()
	{
		gameManager = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/UnitZ/Game/GameManager.prefab", typeof(GameObject));
		if (gameManager) {
			gameServer = gameManager.GetComponent<GameServer> ();
			unitZmanager = gameManager.GetComponent<UnitZManager> ();
		}	
	}
	
	void OnEnable () {
		loadData ();
	}

	void OnGUI ()
	{
		titleContent.text = "Settings";
		
		if (gameManager == null)
			return;
		
		GUI.contentColor = Color.white;
		EditorGUILayout.Separator ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Master server IP (optional)"); 
		gameServer.MasterServerIP = EditorGUILayout.TextField (gameServer.MasterServerIP);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Server Name"); 
		gameServer.ServerName = EditorGUILayout.TextField (gameServer.ServerName);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Port"); 
		gameServer.Port = EditorGUILayout.IntField (gameServer.Port);
		EditorGUILayout.EndHorizontal ();
	
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Max Player"); 
		gameServer.MaxPlayer = EditorGUILayout.IntField (gameServer.MaxPlayer);
		EditorGUILayout.EndHorizontal ();
		
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField("Key version"); 
		unitZmanager.GameKeyVersion = EditorGUILayout.TextField (unitZmanager.GameKeyVersion);
		EditorGUILayout.EndHorizontal ();
		
		if (GUI.changed) {
			EditorUtility.SetDirty (gameManager);
		}
             
	}

}

