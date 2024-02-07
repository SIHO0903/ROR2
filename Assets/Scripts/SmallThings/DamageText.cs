using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float alphaSpeed;
    [SerializeField] float disappearSpeed;

    TextMeshPro text;
    Color alpha;
    void Awake()
    {
        text= GetComponent<TextMeshPro>();
        alpha = text.color;
    }
    private void OnEnable()
    {
        transform.position = Vector3.zero;
        alpha.a = 1;
        disappearSpeed = 0f;
    }
    void Update()
    {
        disappearSpeed += Time.deltaTime;
        transform.Translate(new Vector3(0,moveSpeed*Time.deltaTime,0));
        //alpha.a = Mathf.Lerp(alpha.a, 0, alphaSpeed * Time.deltaTime);
        text.color = alpha;
        //if(alpha.a<= disappearSpeed)
        if(disappearSpeed>=1f)
            gameObject.SetActive(false);
    }
    void SetUp(float damage,Color color)
    {
        text.text = damage.ToString();
        alpha = color;
    }
    public static DamageText Create(Vector3 pos, float damage,Transform ObejctPos,Color color)
    {
        GameObject damageTextObj = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 0, ObejctPos);
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        damageTextObj.transform.position = new Vector3(Random.Range(-2f,2f)+pos.x,pos.y,pos.z) + Vector3.up * 3f;
        damageText.SetUp(damage,color);
        Debug.Log("데미지텍스트출력");
        return damageText;
    }
}
