using Mechanic;
using UnityEngine;
using System.Collections;

public class BasePart : MonoBehaviour
{
    public Vector3 zoomCoords;
	public Vector3 zoomRotation;
    public bool isZoomable;
    public bool isZoomed;
    public bool isReplacable;
    public bool isOpenable;
	public bool isOpened;
    public GameObject[] Ingredients;
	public Vector3 origRotation;
	public Vector3 origCoords;
	public Vector3 openRotation;
	public Vector3 openCoords;

    /// <summary>
    /// Typy części do których pasuje this część
    /// </summary>
    public ResourceManager.PartTypes[] parentingParts;

    /// <summary>
    /// Kategoria sklepowa this części
    /// </summary>
    public ResourceManager.Categories partCategory;

    /// <summary>
    /// Wartość this części - cena
    /// </summary>
    public decimal value;

    void Start()
    {

    }

    void Update()
    {

    }

    public bool isAllIngredientsDisabled()
    {
        foreach (GameObject ingredient in Ingredients)
        {
            if (ingredient.renderer.enabled) return false;
        }
        return true;
    }
    public void dissapear(BasePart part)
    {
        Renderer[] renderers = new Renderer[10];
        renderers = part.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i]) renderers[i].enabled = false;
        }
    }
	public void OpenLid()
	{
		//Quaternion rotation = new Quaternion ();
		//rotation.eulerAngles = this.origRotation;
		//rotation *= Quaternion.Euler(this.openRotation); 
		//this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100f);
		//this.transform.Rotate(this.openRotation.x, this.openRotation.y, this.openRotation.z, Space.Self);
		this.transform.eulerAngles = Vector3.RotateTowards
			(this.origRotation, this.openRotation, 200, 200);
		//this.transform.eulerAngles = this.openRotation;
		this.transform.position = Vector3.MoveTowards(this.origCoords, this.openCoords, 1);
		this.isOpened = true;
	}
	public void CloseLid (){

		this.transform.eulerAngles = Vector3.RotateTowards
			(this.openRotation, this.origRotation, 200, 200);
		this.transform.position = Vector3.MoveTowards(this.openCoords, this.origCoords, 1);
		this.isOpened = false;
	}
	public void Ghost () {
		// Assigns a material named "Assets/Resources/DEV_Orange" to the object.
		Material ghostMat = Resources.Load("Materials/ghostMat", typeof(Material)) as Material;
		this.renderer.material = ghostMat;
		Renderer[] renderers = new Renderer[10];
		renderers = this.GetComponentsInChildren<Renderer>();
		
		for (int i = 0; i < renderers.Length; i++)
		{
			if (renderers[i]) renderers[i].material = ghostMat;
		}
	}



}