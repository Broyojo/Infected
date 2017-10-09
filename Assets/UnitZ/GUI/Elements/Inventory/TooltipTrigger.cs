//----------------------------------------------
//      UnitZ : FPS Sandbox Starter Kit
//    Copyright © Hardworker studio 2015 
// by Rachan Neamprasert www.hardworkerstudio.com
//----------------------------------------------
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
 
public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler,IPointerClickHandler
{
	// add this script to any object with GUIItemCollector component attached.
	
	public ItemCollector Item;
	public string Type = "inventory";
	private Vector3 pointerPosition;
	
	public void Start(){
		
	}
	
	public void OnPointerClick (PointerEventData eventData)
	{
		pointerPosition = new Vector3 (eventData.position.x, eventData.position.y - 18f, 0f);
		Select (pointerPosition);

	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		// show TooltipDetails when mouse is over it
		GUIItemCollector guiItem = this.GetComponent<GUIItemCollector> ();
		if (guiItem)
			Item = guiItem.Item;
		
		
		TooltipDetails.Instance.hover = true;
		StartCoroutine (TooltipDetails.Instance.OnHover (eventData,Item));

	}

	public void OnSelect (BaseEventData eventData)
	{
		// no script here
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		TooltipDetails.Instance.hover = false;
	}

	public void OnDeselect (BaseEventData eventData)
	{
		// no script here
	}
 
	void Select (Vector3 position)
	{
		GUIItemCollector guiItem = this.GetComponent<GUIItemCollector> ();
		if (guiItem)
			Item = guiItem.Item;
		
		if (Item != null) {
			TooltipUsing.Instance.ShowTooltip (Item, position,Type);
		}
		TooltipDetails.Instance.HideTooltip();
	}

}
