using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NearestTarget : MonoBehaviour
{
    public Transform nearestTarget;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float missileRange;
    [SerializeField] float range;
    [SerializeField] float boxCastSize; //에임보정 : 수치가 클수록 타겟과 멀어져도 타겟팅하고잇음

    Transform cameraTr;
    private void Start()
    {
        cameraTr = Camera.main.transform;
    }

    //화면 중앙기준 가장 가까운 타겟설정
    public Transform NearestVisibleTarget()
    {
        //startPos, boxcastSize, dir, raycastHit, Quaternion, maxDistance, layer
        if (Physics.BoxCast(cameraTr.position, transform.lossyScale * boxCastSize, cameraTr.forward, out RaycastHit hitInfo, Quaternion.identity, range, targetLayer))
        {
            nearestTarget = hitInfo.transform;
            nearestTarget = nearestTarget.GetChild(0).transform;
            return nearestTarget;
        }
        else
        {
            return nearestTarget = null;
        }
    }
}
