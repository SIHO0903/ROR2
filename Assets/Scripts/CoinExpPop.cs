using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinExpPop : MonoBehaviour
{
    Rigidbody rigid;
    SphereCollider sphereCollider;
    [SerializeField] float rotateSpeed;
    [HideInInspector] public int money;
    [HideInInspector] public float exp;
    float falseTime=3f;
    float curFalseTime;
    [SerializeField] bool isCoin;
    WaitForSeconds movedelay;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
            return;

        movedelay = new WaitForSeconds(0.5f);
    }
    private void Update()
    {

        curFalseTime += Time.deltaTime;
        if (curFalseTime > falseTime)
        {
            curFalseTime = 0;
            ConOrExp();
            AudioManager.instance.SFXPlayer(AudioManager.SFX.GetExp);
            gameObject.SetActive(false);
        }
        transform.Rotate(Vector3.one * rotateSpeed);
    }
    void ConOrExp()
    {
        if(isCoin)
            GameManager.instance.gold += money;
        else
            GameManager.instance.exp += exp;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isCoin && LayerMask.LayerToName(collision.gameObject.layer) == "Ground")
        {
            sphereCollider.isTrigger = true;
            rigid.isKinematic = true;
            StartCoroutine(ExptoPlayer());
        }
    }
    IEnumerator ExptoPlayer()
    {
        yield return movedelay;
        Vector3 dir = GameManager.instance.player.orientation.position - transform.position;
        rigid.isKinematic = false;
        rigid.velocity = dir * 3f;
    }
}
