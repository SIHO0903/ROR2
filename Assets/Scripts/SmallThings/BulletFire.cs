using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    public float damage;
    [SerializeField] bool isRotate;
    [SerializeField] float rotationSpeed;
    [SerializeField] bool isSlerp;
    float disableTime=7f;
    float curDisableTime=4f;
    Vector3 dirPos;

    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

    }
    private void OnEnable()
    {

        rigid.velocity = Vector3.zero;
        curDisableTime = 0;

    }
    public void Dir(Vector3 startPos, Vector3 endPos, float damage)
    {
        dirPos = endPos - startPos;
        this.damage = damage;
    }
    public void Dir(Vector3 startPos, Vector3 endPos,Vector3 midPos, float damage)
    {
        dirPos = endPos - startPos;
        this.damage = damage;
    }

    private void Update()
    {

        if (isRotate)
        {
            float rotationX = Time.deltaTime * rotationSpeed;
            float rotationY = Time.deltaTime * rotationSpeed;

            transform.Rotate(new Vector3(rotationX, rotationY, 0));
        }

        ProjectileDisable();
    }

    private void ProjectileDisable()
    {
        curDisableTime += Time.deltaTime;
        if (curDisableTime >= disableTime)
        {
            curDisableTime = 0;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        rigid.velocity = dirPos.normalized * bulletSpeed * Time.deltaTime;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log("º®¸ÂÀ½");
            gameObject.SetActive(false);
        }
        if(gameObject.tag == "EnemyBullet" && other.CompareTag("Player"))
        {
            GameManager.instance.player.GetHit(damage);
            gameObject.SetActive(false);
        }
    }
}
