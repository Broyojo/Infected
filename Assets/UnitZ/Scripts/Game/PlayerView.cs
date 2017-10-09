//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------
using UnityEngine;
using System.Collections;

public class PlayerView : MonoBehaviour
{

	public FPSCamera FPScamera;
	public GameObject[] PlayerObjects;
	private CharacterSystem character;

	void Start ()
	{
		SetActive ();
	}

	void Awake ()
	{
		character = this.GetComponent<CharacterSystem> ();
		FPScamera = GetComponentInChildren<FPSCamera>();
		//if (fpscam)
			//FPScamera = fpscam.GetComponent<FPSCamera> ();
	}
	
	void Update ()
	{
		SetActive ();
	}
	
	public void SetActive ()
	{
		if (character && character.IsMine) {
			foreach (GameObject go in PlayerObjects) {
				//Hide (go.transform, false);
				go.SetActive(false);
			}
			FPScamera.gameObject.SetActive (true);
		} else {
			FPScamera.gameObject.SetActive (false);
			foreach (GameObject go in PlayerObjects) {
				//Hide (go.transform, true);
				go.SetActive(true);
			}
		}
	}
	
	public void Hide (Transform trans, bool hide)
	{
		foreach (Transform ob in trans) {
			ob.gameObject.SetActive (hide);	
		}
		trans.gameObject.SetActive (hide);
	}
	
}
