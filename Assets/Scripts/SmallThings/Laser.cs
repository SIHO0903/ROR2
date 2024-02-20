using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Laser : MonoBehaviour
{
    Vector3 currentScale;
    Transform main; // x
    Transform laser; // y
    [SerializeField] float laserRange; 
    [SerializeField] float maxLaserRange;
    [SerializeField] float rotateSpeed;
    float hitTime;
    float curHitTime = 0.1f;
    float curFalseTimer;
    float falseTime= 8f;
    float damage;
    Transform target;

    void Start()
    {
        main = transform.parent;
        laser = transform;
        target = GameManager.instance.player.orientation;
        currentScale = laser.localScale;

    }

    private void OnEnable()
    {
        curFalseTimer = 0;
    }
    void Update()
    {
        curFalseTimer += Time.deltaTime;

        // 플레이어를 천천히 따라가는 레이저
        Vector3 dirM = target.position - main.position;
        Quaternion targetRotation = Quaternion.LookRotation(dirM);
        main.rotation = Quaternion.RotateTowards(main.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        //레이저의 사거리 설정
        laserRange = Mathf.Clamp(Vector3.Distance(main.position, target.position),0f, maxLaserRange);

        //레이저가 늘어날때 플레이어 방향으로만 늘어나게 설정
        laser.localScale = new Vector3(currentScale.x, currentScale.y * laserRange, currentScale.z);
        laser.localPosition = Vector3.forward * laserRange;


        if(curFalseTimer >= falseTime)
            transform.parent.gameObject.SetActive(false);
    }

    public void Damage(float damge)
    {
        this.damage = damge;
    }
    private void OnTriggerStay(Collider other)
    {
        hitTime += Time.deltaTime;
        if(hitTime>= curHitTime && other.CompareTag("Player"))
        {
            GameManager.instance.player.GetHit(damage);
            hitTime = 0;
        }
    }
}
