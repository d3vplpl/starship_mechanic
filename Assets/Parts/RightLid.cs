using UnityEngine;
using System.Collections;

public class RightLid : BasePart {

	// Use this for initialization
	void Start () {
		this.origRotation = new Vector3(270, 270, 0);
		this.openRotation = new Vector3(384, 258, -4.5f);
		this.openCoords = new Vector3(-1.76f, 6.67f, -0.26f);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*void OpenLid () {
		Vector3 origRotation = this.transform.eulerAngles;
		Vector3 rotationOpen = new Vector3 (0,0,90);
		this.transform.eulerAngles = Vector3.RotateTowards
					(origRotation, rotationOpen, 200,200);
	}*/
	/* public override void OpenLid(Vector3 currentPos)
	{
		Vector3 origRotation = new Vector3(270, 270, 0);
		Vector3 rotationOpen = new Vector3(384, 258, -4.5f);
		Vector3 coordsOpen = new Vector3(-1.58f, 6.67f, -0.26f);//rotX384 rotY257 rotZ-4.3 posX-1.5 posY6.6 posZ-0.3 hh
		transform.eulerAngles = Vector3.RotateTowards
			(origRotation, rotationOpen, 200, 200);
		transform.position = Vector3.MoveTowards(currentPos, coordsOpen, 1);
	}*/
}
