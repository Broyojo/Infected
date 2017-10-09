using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
[RequireComponent(typeof(CharacterInventory))]
public class ItemStocker : ObjectTrigger {

	public string StockID = "mybox";
	public CharacterInventory inventory;
	
	private int updateTemp = 0;
	private NetworkView networkViewer;
	private bool stockLoaded = false;
	private ObjectPlacing placing;
	

	void Start ()
	{
		DontDestroyOnLoad(this.gameObject);
		networkViewer = this.GetComponent<NetworkView> ();
		inventory = this.GetComponent<CharacterInventory> ();
		placing = this.GetComponent<ObjectPlacing> ();
		if(placing)
			StockID = placing.ItemUID;
		
	}
	
	public void OpenStock ()
	{
		if (Network.isServer || (!Network.isClient && !Network.isServer)) {
			LoadStock ();
		}
		GetSyncStock ();
	}
	
	public override void OnExit ()
	{
		UnitZ.Hud.CloseSecondInventory();
		base.OnExit ();
	}
	
	public void SaveItem (ItemCollector item)
	{
		inventory.AddItemByCollector (item);
	}
	
	void Update ()
	{
		if (inventory == null)
			return;
		
		if (updateTemp != inventory.UpdateCount && stockLoaded) {
			SyncStock ();
			SaveStock ();
			updateTemp = inventory.UpdateCount;
		}
		
		UpdateFunction();
	}

	public override void Pickup (CharacterSystem character)
	{
		if (character && character.IsMine) {
			OpenStock ();
			UnitZ.Hud.OpenSecondInventory(inventory,"Stock");
		}
		base.Pickup (character);
	}

	void SaveStock ()
	{
		if (inventory == null)
			return;
		
		string datatext = inventory.GetItemDataText ();
		PlayerPrefs.SetString (StockID, datatext);
	}
	
	void LoadStock ()
	{
		if (inventory == null)
			return;
		
		if (PlayerPrefs.HasKey (StockID)) {
			inventory.SetItemsFromText (PlayerPrefs.GetString (StockID));
			stockLoaded = true;
		} else {
			stockLoaded = true;
			SaveStock ();	
		}
	}
	
	void SyncStock ()
	{
		if (Network.isServer) {
			if (networkViewer) {
				string datatext = inventory.GetItemDataText ();
				networkViewer.RPC ("getStockData", RPCMode.Others, datatext);	
			}
		}
	}
	
	[RPC]
	void getStockData (string text)
	{
		if (inventory == null)
			return;
		inventory.SetItemsFromText (text);
	}
	
	public void GetSyncStock ()
	{
		if (networkViewer && Network.isClient) {
			networkViewer.RPC ("getSyncStock", RPCMode.Server, null);	
		}
	}
	
	[RPC]
	public void getSyncStock (NetworkMessageInfo messageInfo)
	{
		if(!stockLoaded){
			LoadStock();	
		}
		if (networkViewer && inventory) {
			string datatext = inventory.GetItemDataText ();
			networkViewer.RPC ("getStockData", messageInfo.sender, datatext);	
		}
	}
	


}
