using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
