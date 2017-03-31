using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper2D 
{

    public static Vector3 getMousePosInWorldSpace ()
    {
        Vector3 mousePositionInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mousePositionInWorldSpace.z = 0;

        return mousePositionInWorldSpace;
    }

}
