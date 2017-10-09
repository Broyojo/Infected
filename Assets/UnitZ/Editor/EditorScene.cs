using UnityEngine;
using UnityEditor;
using System.Collections;
 
class EditorScene : EditorWindow
{
	private GameObject gameManager;
	private SceneManager sceneManager;
	
	[MenuItem ("Window/UnitZ/Scene Manager")]
	public static void ShowSceneManager ()
	{
		EditorWindow.GetWindow (typeof(EditorScene));
	}
	
	void loadData ()
	{
		gameManager = (GameObject)AssetDatabase.LoadAssetAtPath ("Assets/UnitZ/Game/GameManager.prefab", typeof(GameObject));
		if (gameManager) {
			sceneManager = gameManager.GetComponent<SceneManager> ();
		}	
	}
	
	void OnEnable () {
		loadData ();
	}

	LevelPreset levelAdding;
	Vector2 scrollPos;
	int indexRemoving = -1;
	
	string titleName = "";
	string sceneName = "";
	string sceneDescription = "";
	Texture2D Image;
	
	void Rest(){
		titleName = "";
		sceneName = "";
		sceneDescription = "";
		Image = null;
		levelAdding = null;
	}
	
	void OnGUI ()
	{
		titleContent.text = "Scene Manager";
		
		if (sceneManager == null)
			return;
		
		GUI.contentColor = Color.yellow;
		EditorGUILayout.LabelField("Add scene information here. and click 'Add Scene'"); 
		
		GUI.contentColor = Color.white;
		Image = (Texture2D)EditorGUILayout.ObjectField (Image, typeof(Texture2D), true);
		
		GUI.contentColor = Color.green;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label("Scene Name");
		GUILayout.FlexibleSpace();
		sceneName = (string)EditorGUILayout.TextField (sceneName);
		EditorGUILayout.EndHorizontal ();
		
		GUI.contentColor = Color.white;
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label("Title");
		GUILayout.FlexibleSpace();
		titleName = (string)EditorGUILayout.TextField (titleName);
		EditorGUILayout.EndHorizontal ();
		
		
		
		
		GUILayout.Label("Description");
		sceneDescription = (string)EditorGUILayout.TextArea (sceneDescription);
		
		
		
		
		if (GUILayout.Button ("Add Scene", GUILayout.Width (position.width - 5), GUILayout.Height (30))) {
			
			levelAdding = new LevelPreset();
			levelAdding.Detail = sceneDescription;
			levelAdding.Icon = Image;
			levelAdding.LevelName = titleName;
			levelAdding.SceneName = sceneName;
			System.Array.Resize (ref sceneManager.LevelPresets, sceneManager.LevelPresets.Length + 1);
			sceneManager.LevelPresets [sceneManager.LevelPresets.Length - 1] = levelAdding;
			levelAdding = null;
			
			
			Rest();
		}
		
		EditorGUILayout.LabelField("Game Scenes"); 
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos, GUILayout.Width (position.width), GUILayout.Height (position.height - 170));
					
		for (int i=0; i<sceneManager.LevelPresets.Length; i++) {
			

			EditorGUILayout.Separator ();
			
			sceneManager.LevelPresets[i].Icon = (Texture2D)EditorGUILayout.ObjectField (sceneManager.LevelPresets[i].Icon, typeof(Texture2D), true);
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label(sceneManager.LevelPresets[i].Icon,GUILayout.Width(300),GUILayout.Height(100));
			EditorGUILayout.EndHorizontal ();
			
			GUI.contentColor = Color.green;
			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label("Scene Name");
			GUILayout.FlexibleSpace();
			sceneManager.LevelPresets[i].SceneName = (string)EditorGUILayout.TextField (sceneManager.LevelPresets[i].SceneName);
			EditorGUILayout.EndHorizontal ();
			GUI.contentColor = Color.white;

			EditorGUILayout.BeginHorizontal ();
			GUILayout.Label("Title");
			GUILayout.FlexibleSpace();
			sceneManager.LevelPresets[i].LevelName = (string)EditorGUILayout.TextField (sceneManager.LevelPresets[i].LevelName);
			EditorGUILayout.EndHorizontal ();
			
			
			GUILayout.Label("Description");
			sceneManager.LevelPresets[i].Detail = (string)EditorGUILayout.TextArea (sceneManager.LevelPresets[i].Detail);
			
						
			EditorGUILayout.Separator ();
			
			

			if (indexRemoving != -1) {
				if (EditorUtility.DisplayDialog ("Remove scene","Do you want to remove this scene ?", "Remove", "Cancel")) {
					RemoveSceneAt (indexRemoving);
					indexRemoving = -1;
				}else{
					indexRemoving = -1;	
				}
			}
			EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button ("Remove", GUILayout.Width (60.0f))) {
				indexRemoving = i;
			}
			EditorGUILayout.EndHorizontal ();
		}
             
		EditorGUILayout.EndScrollView ();
		if (GUI.changed) {
			EditorUtility.SetDirty (gameManager);
		}
             
	}

	
	void RemoveSceneAt (int index)
	{
		LevelPreset[] levels = new LevelPreset[sceneManager.LevelPresets.Length - 1];
		int count = 0;
		for (int i=0; i<sceneManager.LevelPresets.Length; i++) {
			if (i != index) {
				levels [count] = sceneManager.LevelPresets [i];
				count++;
			}
			
		}
		
		sceneManager.LevelPresets = (LevelPreset[])levels.Clone ();
		
	}
    
}

