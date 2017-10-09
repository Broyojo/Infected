//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipDetails : TooltipInstance {
	
	// gui elements, need to assign them to these parameter.
	public Text Header;
	public Text Content;
	private static TooltipDetails tooltip;
	
	void Start () {
		tooltip = this;
		HideTooltip();
	}

	
	public static TooltipDetails Instance {
		get {
			if (tooltip == null)
				tooltip = GameObject.FindObjectOfType<TooltipDetails> ();
			return tooltip;
		}
	}

	public override void ShowTooltip (ItemCollector itemCol, Vector3 pos)
	{
		if(itemCol == null || itemCol.Item == null || MouseLock.MouseLocked)
			return;
		
		// update GUI elements with name and description of ItemCollector
		
		if (Header)
			Header.text = itemCol.Item.ItemName;
		if (Content)
			Content.text = itemCol.Item.Description;
		
		if(TooltipUsing.Instance.gameObject.activeSelf)
			hover = false;
		
		base.ShowTooltip (itemCol, pos);
	}
}
