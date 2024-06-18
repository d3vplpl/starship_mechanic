using UnityEngine;
using Mechanic;

public class UserInput : MonoBehaviour
{
    private Player player;
    public Camera kamera1;
    public GameObject rotateAround;
    public Camera kamera2;
    private static Vector3 rotDirectLeft = new Vector3(0, 1, 0);
    private static Vector3 rotDirectRight = new Vector3(0, -1, 0);
    private Vector3 rotDirect;
    private Vector3 currentPos;
    private Vector3 _goingTo; //zmienna dla płynnego przejścia kamery
	private Vector3 _goingToAngle; //zmienna dla płynnego przejścia kamery
	private const float _cameraSmoothness = 0.05f; //im mniej tym wolniejsza
	Vector3 originAngle;
	BasePart selectedPart;
	public ParticleSystem EngineFlame1, EngineFlame2, EngineFlame3;

    // Use this for initialization
    void Start()
    {
        //poczatkowa pozycja kamery - najlepiej wrzucic do consta
        _goingTo = new Vector3(14.11857f, 9.629429f, -2f);
        player = transform.root.GetComponent<Player>();
        rotateAround.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isHuman)
        {
            MoveCamera();
        }
        MouseClick();
		KeyboardCheck();
    }
	private void KeyboardCheck()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			EngineFlame1.enableEmission = EngineFlame2.enableEmission = EngineFlame3.enableEmission = true;
		}
		else
		{
			EngineFlame1.enableEmission = EngineFlame2.enableEmission = EngineFlame3.enableEmission = false;
		}
	}

    void MoveCamera()
    {
        kamera2.enabled = false;
        kamera1.enabled = true;
        bool move = false;
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 originAngle = kamera1.transform.eulerAngles;
        Vector3 destinationAngle = originAngle;

        Vector3 rotation = new Vector3(0, 0, 0);
        Vector3 movement = new Vector3(0, 0, 0);

        //horizontal camera movement LEFT
        //float thetaRad = Mathf.PI / 180 * (90 - (Mathf.PI/180 * ResourceManager.RotateAmount));
        if (xpos >= 0 && xpos < ResourceManager.ScrollWidth)
        {
            rotDirect = rotDirectLeft;
            move = true;
            //RIGHT
        }
        else if (xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth)
        {
            rotDirect = rotDirectRight;
            move = true;
        }

        //vertical camera movement
        if (ypos >= 0 && ypos < ResourceManager.ScrollWidth)
        {
            movement.z -= ResourceManager.ScrollSpeed;
            //		rotation.z -= ResourceManager.RotateAmount;

        }
        else if (ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth)
        {
            movement.z += ResourceManager.ScrollSpeed;
            rotation.z += ResourceManager.RotateAmount;
        }

        //make sure movement is in the direction the camera is pointing
        //but ignore the vertical tilt of the camera to get sensible scrolling
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;

        //away from ground movement
        //  movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

        //calculate desired camera position based on received input
        Vector3 origin = kamera1.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;
        destinationAngle.y -= rotation.y;
        destinationAngle.z += rotation.z;

        //limit away from ground movement to be between a minimum and maximum distance
        //if(destination.y > ResourceManager.MaxCameraHeight) {
        //   destination.y = ResourceManager.MaxCameraHeight;
        //} else if(destination.y < ResourceManager.MinCameraHeight) {
        //    destination.y = ResourceManager.MinCameraHeight;
        // }

        //actual move 
        if (move)
        {
            Vector3 v = new Vector3(rotateAround.transform.position.x - 2.3f, rotateAround.transform.position.y,
                rotateAround.transform.position.z - 2.5f);
            kamera1.transform.RotateAround(v, rotDirect, 40f * Time.deltaTime);
            move = false;
            _goingTo = Camera.main.transform.position;
        }

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _goingTo, _cameraSmoothness);

        if (destinationAngle != originAngle)
        {
        }
    }//endof Move

    private void MouseClick()
    {
        if (Input.GetMouseButtonDown(0)) LeftMouseClick();
    }

    private void LeftMouseClick()
    {
        GameObject hitObject = FindHitObject();
        BasePart p = null;
        if (hitObject)
        { // && hitObject.name == "RightLid") {  
            p = hitObject.transform.GetComponent<BasePart>(); //musi być mesh collider
            if (p) currentPos = p.transform.position;
        }
        //}

        Vector3 vRotation = new Vector3(10, 270, 0);
		Vector3 origRotation = Camera.main.transform.eulerAngles;
        if (p && p.isZoomable)
        { //Part can be zoomed
            if (!p.isZoomed)
            { //zooming
                _goingTo = p.zoomCoords;
                p.isZoomed = true;
                
                //zakomentuj ta linijke aby uzyskac maksymalna gladkosc ruchu, ale bez zmiany konta nachylenia kamery co psuje widok na zblizeniu.
                //z odkomentowana linijka, kat sie zmienia ale robi to gwaltownie
                //TODO: animowac rowniez zmiane kata nachylenia.
				vRotation = p.zoomRotation;
            }
            else
            {	
                if (p.isOpenable && p.Ingredients.Length!=0 &&p.isAllIngredientsDisabled())
                {
                    //vRotation = new Vector3(0, 270, 0);
					vRotation = p.zoomRotation;

					//openLid (p);
					if (!p.isOpened) {p.OpenLid();}
					else p.CloseLid();
					selectedPart = p;
                }
				else //unzooming
                {
					vRotation = ResourceManager.CameraInitialRotation;
					_goingTo = ResourceManager.CameraInitialPosition;
                    p.isZoomed = false;
                }
            }
          
			Camera.main.transform.eulerAngles = vRotation;
      
        }
        else if (p && p.isReplacable)
        { //part cannot be zoomed
			if (p.partCategory==ResourceManager.Categories.bolt){
				p.dissapear(p);
			} else p.Ghost();
        }

    }

    private GameObject FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.collider.gameObject;
        return null;
    }

    private Vector3 FindHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return (new Vector3(0, 0, 0)); ;
    }

    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        //detect rotation amount if ALT is being held and the Right mouse button is down
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
            destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
        }
    }
	void openLid (BasePart p) {

		p.transform.Rotate(p.openRotation.x, p.openRotation.y, p.openRotation.z, Space.Self);
		//this.transform.eulerAngles = Vector3.RotateTowards
		//	(this.origRotation, this.openRotation, 200, 200);
		//this.transform.eulerAngles = this.openRotation;
		p.transform.position = Vector3.MoveTowards(p.transform.position, p.openCoords, 1);

	}
	
	void OnGUI(){
		string partRotation ="";
		string partCoords = "";
		if (selectedPart) {
			partRotation = "part_rot_x:" + selectedPart.transform.eulerAngles.x.ToString() + System.Environment.NewLine +
				"part_rot_y:" + selectedPart.transform.eulerAngles.y.ToString() + System.Environment.NewLine +
					"part_rot_z:" + selectedPart.transform.eulerAngles.z.ToString() + System.Environment.NewLine;
			partCoords = "part_x:" + selectedPart.transform.position.x.ToString() + System.Environment.NewLine +
				"part_y:" + selectedPart.transform.position.y.ToString() + System.Environment.NewLine +
					"part_z:" + selectedPart.transform.position.z.ToString() + System.Environment.NewLine;		
		};
		GUI.Box (new Rect (0, 0, 200, 300), "x:" + Camera.main.transform.position.x.ToString () + System.Environment.NewLine +
						"y:" + Camera.main.transform.position.y.ToString () + System.Environment.NewLine +
						"z:" + Camera.main.transform.position.z.ToString () + System.Environment.NewLine +
						"rot_x:" + Camera.main.transform.eulerAngles.x.ToString () + System.Environment.NewLine +
						"rot_y:" + Camera.main.transform.eulerAngles.y.ToString () + System.Environment.NewLine +
						"rot_z:" + Camera.main.transform.eulerAngles.z.ToString () + System.Environment.NewLine +
						"origAng_x:" + originAngle.x.ToString () + System.Environment.NewLine + 
						"origAng_y:" + originAngle.y.ToString () + System.Environment.NewLine + 
						"origAng_z:" + originAngle.z.ToString () + System.Environment.NewLine +
						"destAng_x:" + _goingToAngle.x.ToString () + System.Environment.NewLine + 
						"destAng_y:" + _goingToAngle.y.ToString () + System.Environment.NewLine + 
		         "destAng_z:" + _goingToAngle.z.ToString () + System.Environment.NewLine + partRotation +
		         System.Environment.NewLine + partCoords);
	
	
	}
}
