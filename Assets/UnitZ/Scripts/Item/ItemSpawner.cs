﻿//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
	public bool SpawnOnStart = true;
	public float timeSpawn = 120;
	public ItemData[] Item;
	public int ItemMax = 3;
	public Vector3 Offset = new Vector3 (0, 0.1f, 0);
	private float timeTemp = 0;
	private List<GameObject> itemList = new List<GameObject> ();
	
	void Start ()
	{
		if(SpawnOnStart)
			Spawn ();
	}
	
	void Spawn ()
	{
		ObjectExistCheck ();
		if (ObjectsNumber < ItemMax) {
			if (Item.Length > 0) {
				ItemData itemPick = Item [Random.Range (0, Item.Length)];
				GameObject objitem = null;
				Vector3 spawnPoint = DetectGround (transform.position + new Vector3 (Random.Range (-(int)(this.transform.localScale.x / 2.0f), (int)(this.transform.localScale.x / 2.0f)), 0, Random.Range ((int)(-this.transform.localScale.z / 2.0f), (int)(this.transform.localScale.z / 2.0f))));
				if ((Network.isServer || Network.isClient)) {
					// Only server can spawn an items.
					if(Network.isServer){ 
						objitem = (GameObject)Network.Instantiate (itemPick.gameObject, spawnPoint, Quaternion.identity, 2);
						//Debug.Log("Spawn "+itemPick.gameObject.name);
					}
				} else {
					// Spawn in offline mode.
					objitem = (GameObject)GameObject.Instantiate (itemPick.gameObject, spawnPoint, Quaternion.identity);
					//Debug.Log("Spawn "+itemPick.gameObject.name);
				}
				
				if (objitem)
					itemList.Add (objitem);
			}
			timeTemp = Time.time;
		}
	}
	
	private int ObjectsNumber;

	void ObjectExistCheck ()
	{
		ObjectsNumber = 0;
		foreach (var obj in itemList) {
			if (obj != null)
				ObjectsNumber++;
		}
	}
	
	void Update ()
	{
		if (Time.time > timeTemp + timeSpawn) {
			Spawn ();
		}
	}
	
	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere (transform.position, 0.2f);
		Gizmos.DrawWireCube (transform.position, this.transform.localScale);
	}
	
	Vector3 DetectGround (Vector3 position)
	{
		RaycastHit hit;
		if (Physics.Raycast (position, -Vector3.up, out hit, 1000.0f)) {
			return hit.point + Offset;
		}
		return position;
	}
}
