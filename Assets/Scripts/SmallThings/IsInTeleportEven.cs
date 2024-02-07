using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInTeleportEven : MonoBehaviour
{
    static public bool inTeleport;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            inTeleport = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            inTeleport = false;
    }
}
