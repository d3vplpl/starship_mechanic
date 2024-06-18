using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PartsMagazineScript : MonoBehaviour
{
    public AudioClip sound;
    private int timeIntervalCounter;
    private bool wait, visible;
    private ICollection<Transform> parts;
    private float hiddenY;

    void Start()
    {
        hiddenY = transform.position.y;

        parts = new Collection<Transform>();

        float w = 0.26f, x = transform.position.x - 2.38f;

        for (float y = transform.position.y + 0.14f; y > transform.position.y - w * 4; y -= w)
        {
            x += 0.052f; //hack

            for (float z = transform.position.z - 3 * w; z <= transform.position.z + 3 * w; z += w)
            {
                PartsFactory partsFactory = GameObject.Find("Game").GetComponent<PartsFactory>();
                if (partsFactory == null) Debug.Log("Factory is null!");

                Transform partTransform = null;
                switch (Random.Range(0, 3))
                {
                    case 0:
                        {
                            partTransform = partsFactory.CreatePartTransform(Mechanic.ResourceManager.PartTypes.TrippleSemiConductorWithFazeShifter, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                        } break;
                    case 1:
                        {
                            partTransform = partsFactory.CreatePartTransform(Mechanic.ResourceManager.PartTypes.Radiator, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                        } break;
                    case 2:
                        {
                            partTransform = partsFactory.CreatePartTransform(Mechanic.ResourceManager.PartTypes.MainBoard, new Vector3(x, y, z), Quaternion.Euler(0, 0, 0));
                            partTransform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
                        } break;
                }

                partTransform.parent = transform;
                parts.Add(partTransform);
            }
        }

        foreach (Transform t in parts)
        {
            t.eulerAngles = Random.rotation.eulerAngles;
        }
    }

    void Update()
    {
        Transform hit = FindHitObject();
        foreach (Transform t in parts)
        {
            if (hit != null && hit.IsChildOf(t) || hit == t)
            {
                t.Rotate(new Vector3(0.2f, 0.2f, 1.0f));
            }
        }

        if (wait)
        {
            timeIntervalCounter++;
            if (timeIntervalCounter > 20)
            {
                wait = false;
                timeIntervalCounter = 0;
            }
        }

        if (Input.GetKey(KeyCode.I) && !wait)
        {
            visible = !visible;
            wait = true;
            audio.PlayOneShot(sound); //Nie mogłem znaleść nic fajnego /:
        }

        if (visible)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hiddenY - 2, transform.position.z), 0.2f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, hiddenY, transform.position.z), 0.2f);
        }
    }

    private Transform FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.collider.gameObject.transform;
        return null;
    }
}
