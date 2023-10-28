using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSign : MonoBehaviour
{
    public void SetPosition(Road road)
    {
        transform.position = road.transform.position;
        transform.right = road.greenZone.transform.right;
        transform.Rotate(Vector3.forward, -road.GetRelativeAngle() / 2);
        transform.position += transform.right * (road.roadDiameter / 2 + 0.5f);
        transform.eulerAngles = Vector3.zero;
    }
}
