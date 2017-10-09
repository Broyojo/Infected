using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorCreator : MonoBehaviour
{

	
	[MenuItem("Window/UnitZ/Component/Character/Player")]
	static void CreatePlayer ()
	{
		if (Selection.activeGameObject != null) {
			if (Selection.activeGameObject.GetComponent<Animator> () == null) {
				Debug.LogWarning ("The model must have 'Animator' component");
			}
			
			Selection.activeGameObject.AddComponent<PlayerCharacter> ();
			
			Object prefab = AssetDatabase.LoadAssetAtPath ("Assets/UnitZ/Editor/Prefabs/FPScamera.prefab", typeof(GameObject));
			GameObject clone = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			clone.transform.SetParent (Selection.activeGameObject.transform);
			clone.transform.position = Vector3.zero;
			
			Selection.activeGameObject.GetComponent<CharacterInventory> ().FPSItemView = clone.gameObject.GetComponent<FPSCamera> ().FPSItemView.GetComponent<ItemSticker> ();
		}
	}

	[MenuItem("Window/UnitZ/Component/Character/Humanoid")]
	static void CreateHuman ()
	{
		if (Selection.activeGameObject != null) {
			
			if (Selection.activeGameObject.GetComponent<Animator> () == null) {
				Debug.LogWarning ("The model must have 'Animator' component");
			}
			
			Selection.activeGameObject.AddComponent<HumanCharacter> ();
		}
	}

	[MenuItem("Window/UnitZ/Component/Character/Animal")]
	static void CreateAnimal ()
	{
		if (Selection.activeGameObject.GetComponent<Animator> () == null) {
			Debug.LogWarning ("The model must have 'Animator' component");
		}
		if (Selection.activeGameObject != null) {
			Selection.activeGameObject.AddComponent<AnimalCharacter> ();
		}
	}
	
	[MenuItem("Window/UnitZ/Component/AI/Normal AI")]
	static void AddAI ()
	{
		if (Selection.activeGameObject.GetComponent<CharacterSystem> () == null) {
			Debug.LogWarning ("Need Chracter System component!.");
			return;
		}
		if (Selection.activeGameObject != null) {
			Selection.activeGameObject.AddComponent<AICharacterController> ();
		}
	}
	
	[MenuItem("Window/UnitZ/Component/Item/Item Data")]
	static void CreateItem ()
	{
		if (Selection.activeGameObject.GetComponent<ItemData> () != null) {
			return;
		}
		if (Selection.activeGameObject != null) {
			Selection.activeGameObject.AddComponent<ItemData> ();
		}
	}
	
	[MenuItem("Window/UnitZ/Component/Item/Item FPS (Weapon)")]
	static void CreateItemFPSWeapon ()
	{
		if (Selection.activeGameObject.GetComponent<FPSWeaponEquipment> () != null) {
			return;
		}
		if (Selection.activeGameObject != null) {
			Selection.activeGameObject.AddComponent<FPSWeaponEquipment> ();
		}
	}
	
	[MenuItem("Window/UnitZ/Component/Item/Item FPS (Consumable)")]
	static void CreateItemFPSUsing ()
	{
		if (Selection.activeGameObject.GetComponent<FPSItemUsing> () != null) {
			return;
		}
		if (Selection.activeGameObject != null) {
			Selection.activeGameObject.AddComponent<FPSItemUsing> ();
		}
	}
	
	[MenuItem("Window/UnitZ/Component/Item/Item FPS (Placing)")]
	static void CreateItemFPSPlacing ()
	{
		if (Selection.activeGameObject.GetComponent<FPSItemPlacing> () != null) {
			return;
		}
		if (Selection.activeGameObject != null) {
			Selection.activeGameObject.AddComponent<FPSItemPlacing> ();
		}
	}
	
	[MenuItem("Window/UnitZ/Component/Item/Item Equip")]
	static void CreateItemEquip ()
	{
		if (Selection.activeGameObject.GetComponent<ItemWeaponEquipment> () != null) {
			return;
		}
		if (Selection.activeGameObject != null) {
			Selection.activeGameObject.AddComponent<ItemWeaponEquipment> ();
		}
	}
	
	[MenuItem("Window/UnitZ/Play")]
	public static void Run ()
	{
		if (EditorApplication.isPlaying == true) {
			EditorApplication.isPlaying = false;
			return;
		}
		
		EditorApplication.SaveCurrentSceneIfUserWantsTo ();
		EditorApplication.OpenScene ("Assets/UnitZ/mainmenu.unity");
		EditorApplication.isPlaying = true;
	}
}