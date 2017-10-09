using UnityEngine;
using System.Collections;

public class GUIPlayerItemLoader : GUIItemLoader {

	public DropStockArea dropArea;
	
	void Start () {
		dropArea = this.GetComponent<DropStockArea>();
	}
	
	void Update () {
		if (UnitZ.playerManager == null || UnitZ.playerManager.playingCharacter == null)
			return;
		
		currentInventory = UnitZ.playerManager.playingCharacter.inventory;
		UpdateFunction();

	}
}
