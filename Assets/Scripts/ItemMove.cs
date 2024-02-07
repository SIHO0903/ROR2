using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float launchHeight = 5f;
    [SerializeField] float launchDistance = 5f;
    float lerpTime = 3f;
    Vector3 initialPosition;
    float elapsedTime;
    [SerializeField] float totalTime; 
    float curTime;

    float ranX;
    float ranZ;
    [SerializeField] float speed = 1f;
    [SerializeField] bool isvolcano;
    VolcanoFire volcanoFire;
    public bool isContact;
    private void Start()
    {
        volcanoFire = GetComponent<VolcanoFire>();
    }
    private void OnEnable()
    {
        elapsedTime = 0;
        curTime = 0;
        initialPosition = transform.position;
        ranX = GetRandomRange();
        ranZ = GetRandomRange();
    }
    void Update()
    {
        if (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
        }
        else if (elapsedTime>=totalTime && isvolcano)
        {
            volcanoFire.AreaOfEffect(transform); 
            gameObject.SetActive(false); //사라질때 폭팔하는 파티클잇으면 좋을거같음
        }
        else
            transform.Rotate(Vector3.up * rotationSpeed);

        if (curTime >= lerpTime)
            curTime = lerpTime;
        else
            curTime += Time.deltaTime;


        float xPos = initialPosition.x + launchDistance * ranX * Mathf.Lerp(0f, 1f, curTime / lerpTime) * speed;
        float yPos = initialPosition.y + launchHeight * Mathf.Sin(elapsedTime);
        float zPos = initialPosition.z + launchDistance * ranZ * Mathf.Lerp(0f, 1f, curTime / lerpTime) * speed;

        transform.position = new Vector3(xPos, yPos, zPos);

    }
    float GetRandomRange(){
        return Random.Range(-1f, 1f);
    }

    

}
