using UnityEngine;
using System.Collections;
[RequireComponent(typeof(NetworkView))]

public class DoorFrame : MonoBehaviour {

	public Animator animator;
	public bool IsOpen;
	private float timeTemp;
	public float Cooldown = 0.5f;
	public string DoorKey = "";
	private NetworkView networkViewer;

	void Start ()
	{
		networkViewer = this.GetComponent<NetworkView>();
		if (animator == null)
			animator = this.GetComponent<Animator> ();
	}
	
	public void Access (CharacterSystem character)
	{
		if (!Network.isClient && !Network.isServer) {
			AccessDoor (DoorKey);
		} else {
			if (Network.isClient) {
				if (networkViewer)
					networkViewer.RPC ("accessDoor", RPCMode.Server, DoorKey);
			}
			if (Network.isServer) {
				AccessDoor (DoorKey);
			}
				
		}
	}
	
	private void AccessDoor (string key)
	{
		if (key == DoorKey) {
			if (Time.time > timeTemp + Cooldown) {
				IsOpen = !IsOpen;
				timeTemp = Time.time;
			}
		}
	}
	
	void Update ()
	{
		if (animator) {
			animator.SetBool ("IsOpen", IsOpen);	
		}

		if (networkViewer && Network.isServer) {
			int doorstate = 0;
			
			if (IsOpen) {
				doorstate = 1;
			}
			
			networkViewer.RPC ("doorUpdate", RPCMode.Others, doorstate);	
		}
	}
	
	[RPC]
	void accessDoor (string key)
	{
		AccessDoor (key);
	}
	
	[RPC]
	void doorUpdate (int doorstate)
	{
		switch (doorstate) {
		case 0:
			IsOpen = false;
			break;
		case 1:
			IsOpen = true;
			break;	
		}
	}
}
