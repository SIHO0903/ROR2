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
    [SerializeField] float hitTimer;
    float falseTimer;
    float damage;
    Transform target;

    void Start()
    {
        main = transform.parent;
        laser = transform;
        target = GameManager.instance.player.orientation;
        currentScale = laser.localScale;

    }
    void Update()
    {
        falseTimer += Time.deltaTime;


        Vector3 dirM = target.position - main.position;
        Quaternion targetRotation = Quaternion.LookRotation(dirM);
        main.rotation = Quaternion.RotateTowards(main.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        laserRange = Mathf.Clamp(Vector3.Distance(main.position, target.position),0f, maxLaserRange);

        laser.localScale = new Vector3(currentScale.x, currentScale.y * laserRange, currentScale.z);
        laser.localPosition = Vector3.forward * laserRange;


        if(falseTimer>=8f)
            transform.parent.gameObject.SetActive(false);


    }

    public void Damage(float damge)
    {
        this.damage = damge;
    }
    private void OnTriggerStay(Collider other)
    {
        hitTimer += Time.deltaTime;
        if(hitTimer>=0.1f && other.CompareTag("Player"))
        {
            GameManager.instance.player.GetHit(damage);
            hitTimer = 0;
            
        }

    }
}
