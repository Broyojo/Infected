//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelsManager : MonoBehaviour
{
	public PanelInstance[] Pages;
	public PanelInstance currentPanel;

	void Start ()
	{
		// add PanelInstance component to every panels in the list.
		for (int i=0; i<Pages.Length; i++) {
			Pages [i].gameObject.AddComponent<PanelInstance> ();
		}
	}
	
	void Awake ()
	{
		if (Pages.Length <= 0)
			return;
		
		// open first panels at start.
		OpenPanel (Pages [0]);	
	}
	
	// use this function when you need to close all panels in the list normally.
	public void CloseAllPanels ()
	{
		if (Pages.Length <= 0)
			return;
		
		for (int i=0; i<Pages.Length; i++) {
			Animator anim = Pages [i].GetComponent<Animator> ();
			if (anim !=null && anim.isActiveAndEnabled) {
				anim.SetBool ("Open", false);
			}
			if(Pages [i].isActiveAndEnabled){
				StartCoroutine (DisablePanelDeleyed (Pages [i]));
			}
		}
	}
	
	// use this function when you need to disable all panels in the list directly.
	public void DisableAllPanels ()
	{
		if (Pages.Length <= 0)
			return;
		
		for (int i=0; i<Pages.Length; i++) {
			Pages[i].gameObject.SetActive(false);
		}
	}
	
	// use this function when you need to close all the panels in the scene, even they are not in the lists.
	public void CloseAllPanelsInTheScene ()
	{
		PanelsManager[] panelsManage = (PanelsManager[])GameObject.FindObjectsOfType (typeof(PanelsManager));
		if (panelsManage.Length <= 0)
			return;
		
		for (int i=0; i<panelsManage.Length; i++) {
			panelsManage [i].CloseAllPanels ();	
		}
	}
	
	// open panel by name.
	public void OpenPanelByName (string name)
	{
		PanelInstance page = null;
		for (int i=0; i<Pages.Length; i++) {
			if (Pages [i].name == name) {
				page = Pages [i];
				break;
			}
		}
		if (page == null)
			return;
		
		page.PanelBefore = currentPanel;
		currentPanel = page;
		
		CloseAllPanels ();
		Animator anim = page.GetComponent<Animator> ();
		if (anim && anim.isActiveAndEnabled) {
			anim.SetBool ("Open", true);
		}
		page.gameObject.SetActive (true);
		
	}
	
	// for checking if the panel <name> is opened
	public bool IsPanelOpened (string name)
	{
		for (int i=0; i<Pages.Length; i++) {
			if (Pages [i].name == name) {
				return Pages [i].gameObject.activeSelf;
			}
		}
		return false;
	}
	
	// use this function when you need to open and close in the same way.
	public bool TogglePanelByName (string name)
	{
		PanelInstance page = null;
		for (int i=0; i<Pages.Length; i++) {
			if (Pages [i].name == name) {
				page = Pages [i];
				break;
			}
		}
		if (page == null)
			return false;
		
		if (currentPanel == page) {
			ClosePanel (page);
			return false;
		} else {
			page.PanelBefore = currentPanel;
			currentPanel = page;
		
			CloseAllPanels ();
			Animator anim = page.GetComponent<Animator> ();
			if (anim && anim.isActiveAndEnabled) {
				anim.SetBool ("Open", true);
			}
			page.gameObject.SetActive (true);
			return true;
		}
		
	}
	
	// close panel by name.
	public void ClosePanelByName (string name)
	{
		PanelInstance page = null;
		for (int i=0; i<Pages.Length; i++) {
			if (Pages [i].name == name) {
				page = Pages [i];
				break;
			}
		}
		if (page == null)
			return;
		
		currentPanel = null;
		Animator anim = page.GetComponent<Animator> ();
		if (anim && anim.isActiveAndEnabled) {
			anim.SetBool ("Open", false);
		}
		StartCoroutine (DisablePanelDeleyed (page));
		
	}
	
	// close panel by object PanelInstance
	public void ClosePanel (PanelInstance page)
	{
		if (page == null)
			return;
		
		currentPanel = null;
		Animator anim = page.GetComponent<Animator> ();
		if (anim && anim.isActiveAndEnabled) {
			anim.SetBool ("Open", false);
		}
		StartCoroutine (DisablePanelDeleyed (page));
		
	}
	
	// open panel by object PanelInstance
	public void OpenPanel (PanelInstance page)
	{
		if (page == null)
			return;
		
		page.PanelBefore = currentPanel;
		currentPanel = page;
		
		CloseAllPanels ();
		Animator anim = page.GetComponent<Animator> ();
		if (anim && anim.isActiveAndEnabled) {
			anim.SetBool ("Open", true);
		}
		page.gameObject.SetActive (true);

	}
	
	// use this function when you need to open previous panel
	public void OpenPreviousPanel ()
	{
		if (currentPanel && currentPanel.PanelBefore) {

			CloseAllPanels ();
			Animator anim = currentPanel.PanelBefore.GetComponent<Animator> ();
			if (anim && anim.isActiveAndEnabled) {
				anim.SetBool ("Open", true);
			}
			currentPanel.PanelBefore.gameObject.SetActive (true);
			currentPanel = currentPanel.PanelBefore;
		}
	}
	
	// use this function when you need to open panel without saving a previous.
	// so you can't use OpenPreviousPanel to open a previous panel again.
	public void OpenPanelByNameNoPreviousSave (string name)
	{
		PanelInstance page = null;
		for (int i=0; i<Pages.Length; i++) {
			if (Pages [i].name == name) {
				page = Pages [i];
				break;
			}
		}
		if (page == null)
			return;
		
		currentPanel = page;
		
		CloseAllPanels ();
		Animator anim = page.GetComponent<Animator> ();
		if (anim && anim.isActiveAndEnabled) {
			anim.SetBool ("Open", true);
		}
		page.gameObject.SetActive (true);
		
	}
	
	IEnumerator DisablePanelDeleyed (PanelInstance page)
	{

		bool closedStateReached = false;
		bool wantToClose = true;
		Animator anim = page.GetComponent<Animator> ();
		if (anim && anim.enabled) {

			while (!closedStateReached && wantToClose) {
				if (anim.isActiveAndEnabled && !anim.IsInTransition (0)) {
					closedStateReached = anim.GetCurrentAnimatorStateInfo (0).IsName ("Closing");
				}
				yield return new WaitForEndOfFrame();
			}

			if (wantToClose) {
				anim.gameObject.SetActive (false);
			}
			
		} else {
			page.gameObject.SetActive (false);		
		}
		
	}
}
