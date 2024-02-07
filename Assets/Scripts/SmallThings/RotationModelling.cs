using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationModelling : MonoBehaviour
{
    void OnEnable()
    {
        transform.Rotate(new Vector3(0, 0, 180));
    }


}
