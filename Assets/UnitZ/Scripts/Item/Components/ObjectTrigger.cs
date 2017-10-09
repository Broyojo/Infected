using UnityEngine;
using System.Collections;

public class ObjectTrigger : MonoBehaviour {
	public string ActiveText = "Interactive";
	protected bool ShowInfo;
	public float DistanceLimit = 2;
	public Vector3 Offset;
	protected CharacterSystem characterTemp;
	
	void Start () {
	
	}

	void Update () {
		UpdateFunction();
	}
	
	protected void UpdateFunction(){
		if(characterTemp){
			if(Vector3.Distance(this.transform.position,characterTemp.transform.position + Offset) > DistanceLimit){
				OnExit();
			}else{
				OnStay();	
			}
		}
	}
	
	public virtual void OnStay(){
		
	}
	
	public virtual void OnExit(){
		characterTemp = null;
		ShowInfo = false;
	}
	
	public virtual void GetInfo(){
		ShowInfo = true;
	}

	public virtual void Pickup (CharacterSystem character)
	{
		characterTemp = character;
	}
	
	public void FixedUpdate ()
	{
		ShowInfo = false;	
	}
	
	void OnGUI ()
	{
		if (ShowInfo) {
			Vector3 screenPos = Camera.main.WorldToScreenPoint (this.gameObject.transform.position + Offset);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.Label (new Rect (screenPos.x, Screen.height - screenPos.y, 200, 60), ActiveText);
		}
	}
	
}
