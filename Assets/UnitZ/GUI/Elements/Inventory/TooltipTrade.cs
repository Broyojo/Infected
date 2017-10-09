using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipTrade : TooltipInstance
{

	public InputField Number;
	private bool trading;
	private static TooltipTrade tooltip;
	private int number = 0;
	void Start ()
	{
		tooltip = this;
		trading = false;
		HideTooltip();
	}
	
	void Update ()
	{
		
	}
	
	public void UpdateNumber(){
		if(Number == null)
			return;
		
		int.TryParse(Number.text,out number);
	}
	
	public void StartTrade (ItemCollector item,DropStockArea stock)
	{
		if(item == null || stock == null || Number == null)
			return;
		
		Number.text = item.Num.ToString();
		trading = true;
		this.gameObject.SetActive (true);
		StartCoroutine (Trading (item,stock));	
	}
	

	public IEnumerator Trading (ItemCollector item,DropStockArea stock)
	{
		while (trading) {
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("Trade "+item.Item.ItemName+" to "+number);
		stock.Trade(number);
		Cancel ();
		
	}
	
	
	public void Ok ()
	{
		trading = false;
	}
	
	public void Cancel ()
	{
		HideTooltip();
	}
	
	public static TooltipTrade Instance {
		get {
			if (tooltip == null)
				tooltip = GameObject.FindObjectOfType<TooltipTrade> ();
			return tooltip;
		}
	}
}
