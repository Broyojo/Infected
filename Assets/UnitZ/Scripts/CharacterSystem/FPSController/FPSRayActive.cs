//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct casthit
{
	public int index;
	public float distance;
	public string name;
}

public class FPSRayActive : MonoBehaviour
{
	public bool Sorting;
	public string[] IgnoreTag = {"Player"};
	public string[] DestroyerTag = {"scene"};

	void Start ()
	{	
	}

	public void ShootRayOnce (Vector3 origin, Vector3 direction, string id, string team)
	{
		// Normal Damage ray cast.
		RaycastHit hit;
		if (Physics.Raycast (origin, direction, out hit, 100.0f)) {
			// if hit
			if (hit.collider.gameObject != this.gameObject) {
				// create damage package.
				DamagePackage dm;
				dm.Damage = 50;
				dm.Normal = hit.normal;
				dm.Direction = direction;
				dm.Position = hit.point;
				dm.ID = id;
				dm.Team = team;
				
				// send Damage Package through OnHit function
				hit.collider.SendMessage ("OnHit", dm, SendMessageOptions.DontRequireReceiver);
				
			}
		}
	}

	public void CheckingRay (Vector3 origin, Vector3 direction)
	{
		float raySize = 3;
		// Ray cast to any object to getting an object info
		RaycastHit[] casterhits = Physics.RaycastAll (origin, direction, raySize);
		for (int i=0; i<casterhits.Length; i++) {
			if (casterhits [i].collider) {
				RaycastHit hit = casterhits [i];
				// get an object info via GetInfo function
				hit.collider.SendMessage ("GetInfo", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public void ActiveRay (Vector3 origin, Vector3 direction)
	{
		// Ray cast to Interactive and Pickup 
		float raySize = 3;
		RaycastHit[] casterhits = Physics.RaycastAll (origin, direction, raySize);
		for (int i=0; i<casterhits.Length; i++) {
			if (casterhits [i].collider) {
				// interactive through Pickup funtion
				casterhits [i].collider.SendMessage ("Pickup",this.GetComponent<CharacterSystem> (), SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	public bool ShootRay (Vector3 origin, Vector3[] direction, int damage, float size, int hitmax, string id, string team)
	{
		// Multi piercing Damage Ray. e.g you can shoot through in many layer
		bool res = false;
		for (int b=0; b<direction.Length; b++) {
			int damages = damage;
			int hitcount = 0;
			int castcount = 0;
			RaycastHit[] hits;
			List<casthit> castHits = new List<casthit> ();
			float raySize = size;

			// Cast all objects.
			RaycastHit[] casterhits = Physics.RaycastAll (origin, direction [b], raySize);
			for (int i=0; i<casterhits.Length; i++) {
				if (casterhits [i].collider) {
					if (tagCheck (casterhits [i].collider.gameObject) && 
					casterhits [i].collider.gameObject != this.gameObject && 
					((casterhits [i].collider.transform.root && 
						casterhits [i].collider.transform.root != this.gameObject.transform.root &&
						casterhits [i].collider.transform.root.gameObject != this.gameObject) || 
						casterhits [i].collider.transform.root == null)) {
						castcount++;
						casthit casted = new casthit ();
						casted.distance = Vector3.Distance (origin, casterhits [i].point);
						casted.index = i;
						casted.name = casterhits [i].collider.name;
						castHits.Add (casted);
					}
				}
				
			}
			
			// Sort them by distance.
			hits = new RaycastHit[castcount];
			castHits.Sort ((x,y) => x.distance.CompareTo (y.distance));

			for (int i=0; i<castHits.Count; i++) {
				hits [i] = casterhits [castHits [i].index];
			}
			
			// so now you know which one is the first or last
			for (var i = 0; i < hits.Length && hitcount < hitmax; i++) {
				RaycastHit hit = hits [i];
				
				// Create Damage package
				DamagePackage dm;
				dm.Damage = damage;
				dm.Normal = hit.normal;
				dm.Direction = direction [b];
				dm.Position = hit.point;
				dm.ID = id;
				dm.Team = team;
				
				// send Damage Package through OnHit function
				hit.collider.SendMessage ("OnHit", dm, SendMessageOptions.DontRequireReceiver);
				res = true;
				
				// counting hit until max
				hitcount++;
				if (hitcount >= hitmax || tagDestroyerCheck (hit.collider.gameObject)) {
					break;
				}
				// damage reduced every hit
				int damageReduce = (int)((float)damages * 0.75f);
				damages = damageReduce;
			}
		}
		return res;
	}
	
	public bool ShootRayTest (Vector3 origin, Vector3[] direction, int damage, float size, int hitmax, string id, string team)
	{
		// Ray test. not for damage or interactive, just for checking.
		bool res = false;
		for (int b=0; b<direction.Length; b++) {
			int hitcount = 0;
			int castcount = 0;
			RaycastHit[] hits;
			List<casthit> castHits = new List<casthit> ();
			float raySize = size;

		
			RaycastHit[] casterhits = Physics.RaycastAll (origin, direction [b], raySize);
			for (int i=0; i<casterhits.Length; i++) {
				if (casterhits [i].collider) {
					if (tagCheck (casterhits [i].collider.gameObject) && 
					casterhits [i].collider.gameObject != this.gameObject && 
					((casterhits [i].collider.transform.root && 
						casterhits [i].collider.transform.root.gameObject != this.gameObject) || 
						casterhits [i].collider.transform.root == null)) {
						castcount++;
						casthit casted = new casthit ();
						casted.distance = Vector3.Distance (origin, casterhits [i].point);
						casted.index = i;
						casted.name = casterhits [i].collider.name;
						castHits.Add (casted);
					}
				}
				
			}

			hits = new RaycastHit[castcount];
			castHits.Sort ((x,y) => x.distance.CompareTo (y.distance));

			for (int i=0; i<castHits.Count; i++) {
				hits [i] = casterhits [castHits [i].index];
			}

			for (var i = 0; i < hits.Length && hitcount < hitmax; i++) {
				RaycastHit hit = hits [i];
				
				res = true;
				hitcount++;
				if (hitcount >= hitmax || tagDestroyerCheck (hit.collider.gameObject)) {
					break;
				}
				
			}
		}
		return res;
	}
	
	public bool Overlap (Vector3 origin, Vector3 forward, int damage, float size, float dot, string id, string team)
	{
		// overlap damage is not a ray it's just create a damage area, e.g. using for Melee damage
		bool res = false;
		var colliders = Physics.OverlapSphere (origin, size);
		
		foreach (var hit in colliders) {
			if (hit && hit.gameObject != this.gameObject && hit.gameObject.transform.root != this.gameObject.transform) {
				Debug.Log (hit.gameObject.transform.root.name);
				var dir = (hit.transform.position - origin).normalized;
				var direction = Vector3.Dot (dir, forward);	
			
				if (direction >= dot) {
					DamagePackage dm;
					dm.Damage = damage;
					dm.Normal = dir;
					dm.Direction = forward;
					dm.Position = hit.gameObject.transform.position;
					dm.ID = id;
					dm.Team = team;
					hit.GetComponent<Collider>().SendMessage ("OnHit", dm, SendMessageOptions.DontRequireReceiver);
					
					res = true;
				}
			}
		}
		return res;
	}

	public bool OverlapTest (Vector3 origin, Vector3 forward, int damage, float size, float dot, string id, string team)
	{
		// overlap test is just for test.
		bool res = false;
		var colliders = Physics.OverlapSphere (origin, size);
		
		foreach (var hit in colliders) {
			if (hit && hit.gameObject != this.gameObject && hit.gameObject.transform.root != this.gameObject) {
				var dir = (hit.transform.position - origin).normalized;
				var direction = Vector3.Dot (dir, forward);	
			
				if (direction >= dot) {
					res = true;
				}
			}
		}
		
		return res;
	}
	
	private bool tagDestroyerCheck (GameObject obj)
	{
		for (int i=0; i<DestroyerTag.Length; i++) {
			if (obj.CompareTag(DestroyerTag [i])) {
				return true;	
			}
		}
		return false;
	}

	private bool tagCheck (GameObject obj)
	{
		for (int i=0; i<IgnoreTag.Length; i++) {
			if (obj.CompareTag(IgnoreTag [i])) {
				return false;	
			}
		}
		return true;
	}
}
