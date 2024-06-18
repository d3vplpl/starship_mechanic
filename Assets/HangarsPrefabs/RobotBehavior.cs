using UnityEngine;
using System.Collections;

public class RobotBehavior : MonoBehaviour
{
    public Transform BaseSegment, Knee1, Knee2;
    private float rotation, poch;
    private bool goUp;

    void Start()
    {

    }

    void Update()
    {
        rotation++;
        BaseSegment.transform.eulerAngles = new Vector3(0, rotation, 0);

        if (goUp)
        {
            poch += 0.5f;
            if (poch > 50) goUp = false;
        }
        else
        {
            poch -= 0.5f;
            if (poch < -50) goUp = true;
        }

        Knee1.transform.eulerAngles = new Vector3(poch, BaseSegment.transform.eulerAngles.y, 90);
        Knee2.transform.eulerAngles = new Vector3(-poch / 2, BaseSegment.transform.eulerAngles.y, 90);
    }
}
