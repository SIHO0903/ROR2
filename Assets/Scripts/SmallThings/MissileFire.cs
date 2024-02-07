using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFire : MonoBehaviour
{
    //rotate x,z
    //transform up (y)
    Vector3 dirPos;

    public Transform nearestTarget;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float missileSpeed;
    [SerializeField] float missileRotateSpeed;
    Transform target; 
    float damage;
    float curNearestDistance = 30;
    public bool firsetLaunch;
    public float timer;

    private void OnEnable()
    {
        transform.rotation= Quaternion.identity;
        firsetLaunch = true;
        timer = 0f;
    }
    public void Dir(Vector3 startPos,Transform target,float damage)
    {
        dirPos = target.position - startPos;
        this.target = target;
        this.damage = damage;
    }
    void Update()
    {
        if (firsetLaunch)
        {
            timer += Time.deltaTime;
            transform.position += transform.up * missileSpeed * Time.deltaTime;
            if(timer>=2f)
                firsetLaunch = false;
        }
        else
        {
            transform.position += transform.up * missileSpeed * Time.deltaTime;
            transform.LookAt(target.position);
            //if (Vector3.Distance(transform.position,dirPos)>=0.01f)
            //{
            //    //Quaternion targetRotation = Quaternion.LookRotation(target.position);
            //    //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, missileRotateSpeed);


            //}
            //else
            //{
            //    transform.position += transform.up * missileSpeed * Time.deltaTime;
            //    transform.LookAt(target.position);
            //}

        }

    }
    // 오토 타겟팅 미사일
    public Transform SearchTarget()
    {
        RaycastHit[] targets = Physics.SphereCastAll(transform.position, Mathf.Infinity, Vector3.zero, 0f, targetLayer);

        if (targets.Length == 0)
            transform.position += transform.up * missileSpeed;

        curNearestDistance = 30;
        foreach (RaycastHit target in targets)
        {
            if (curNearestDistance > target.distance)
            {
                curNearestDistance = target.distance;
                nearestTarget = target.transform;
            }
        }

        nearestTarget = nearestTarget.GetChild(0).transform;
        return nearestTarget;
    }
}
