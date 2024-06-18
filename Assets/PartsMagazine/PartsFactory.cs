using UnityEngine;

public class PartsFactory : MonoBehaviour
{
    public Transform TrippleSemiConductorWithFazeShifterPart;
    public Transform RadiatorPart;
    public Transform MainBoard;

    public Transform CreatePartTransform(Mechanic.ResourceManager.PartTypes partType, Vector3 position, Quaternion quaternion)
    {
        Transform returnTransform = null;

        switch (partType)
        {
            case Mechanic.ResourceManager.PartTypes.NullPartType:
                {
                } break;
            case Mechanic.ResourceManager.PartTypes.TrippleSemiConductorWithFazeShifter:
                {
                    returnTransform = Instantiate(TrippleSemiConductorWithFazeShifterPart, position, quaternion) as Transform;
                } break;
            case Mechanic.ResourceManager.PartTypes.Radiator:
                {
                    returnTransform = Instantiate(RadiatorPart, position, quaternion) as Transform;
                } break;
            case Mechanic.ResourceManager.PartTypes.MainBoard:
                {
                    returnTransform = Instantiate(MainBoard, position, quaternion) as Transform;
                } break;
        }

        return returnTransform;
    }
}