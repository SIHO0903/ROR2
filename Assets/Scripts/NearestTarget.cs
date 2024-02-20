using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NearestTarget : MonoBehaviour
{
    public Transform nearestTarget;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float missileRange;
    [SerializeField] float range;
    [SerializeField] float boxCastSize; //���Ӻ��� : ��ġ�� Ŭ���� Ÿ�ٰ� �־����� Ÿ�����ϰ�����

    Transform cameraTr;
    private void Start()
    {
        cameraTr = Camera.main.transform;
    }

    //ȭ�� �߾ӱ��� ���� ����� Ÿ�ټ���
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
