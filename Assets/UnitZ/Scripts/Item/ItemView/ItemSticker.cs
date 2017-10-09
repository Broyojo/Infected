//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum EquipType{
	None,Weapon,Head,Armor,BackPack,Foot,FPSItemView
}

public class ItemSticker : MonoBehaviour {
	public EquipType equipType;
	public bool Primary;
	public int ItemIndex;
	public Vector3 RotationOffset;
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	void OnDrawGizmos ()
	{
		#if UNITY_EDITOR

		Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
		Gizmos.matrix = rotationMatrix;
		Gizmos.color = Color.green;
		Gizmos.DrawSphere (Vector3.zero, 0.1f);
		Gizmos.DrawWireCube (Vector3.zero, this.transform.localScale);
		Handles.Label(transform.position, this.name);
		#endif
	}
}
