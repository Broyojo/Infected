using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUICharacterPreset : MonoBehaviour {

	public RawImage GUIImage;
	public Text Name;
	public string Description;
	public int Index;

	void Start () {
	
	}
	
	public void Select(){
		MainMenuManager menu = (MainMenuManager)GameObject.FindObjectOfType(typeof(MainMenuManager));
		if(menu)
			menu.OpenPanelByName("NewCharacter");
		
		CharacterCreatorCanvas creator = (CharacterCreatorCanvas)GameObject.FindObjectOfType(typeof(CharacterCreatorCanvas));
		if(creator)
			creator.SelectCreateCharacter(Index);
		
	}
	
	public void Hover(){
		GUICharacterPresetLoader loader = (GUICharacterPresetLoader)GameObject.FindObjectOfType(typeof(GUICharacterPresetLoader));	
		if(loader)
			loader.Description.text = Description;
	}
	

	void Update () {
	
	}
}
