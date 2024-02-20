using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolcanoFire : MonoBehaviour
{
    float damage;
    float fireDamage;
    GameObject explosion; //4
    GameObject fire; //5
    GameObject aoeRangeindicator; //6

    Vector3 newPos;
    Vector2 fireRange = Vector2.one * 3f;
    float yOffset = 1f;

    RaycastHit raycastHit;
    public void AreaOfEffect(Transform transform)
    {
        ExplosionEffect(transform);
        FireEffect(transform);
    }
    //폭발 파티클이펙트
    public void ExplosionEffect(Transform transform)
    {
        explosion = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 4);
        explosion.transform.position = transform.position;
        explosion.GetComponent<ParticleSystem>().Play();
    }
    //불장판 파티클이펙트
    public void FireEffect(Transform transform)
    {
        fire = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 5); //불장판 파티클 생성

        //지면생성을 위한 위치 재설정
        Physics.Raycast(transform.position, Vector3.down, out raycastHit);
        newPos = new Vector3(transform.position.x, raycastHit.transform.position.y+ yOffset, transform.position.z);
        fire.transform.position = newPos; 
        fire.GetComponent<ParticleIsPlaying>().Damage(fireDamage); //불장판 데미지

        //불장판 크기 설정
        fire.transform.localScale = fireRange;
        fire.transform.GetChild(0).localScale = fireRange;

        fire.GetComponent<ParticleSystem>().Play();

        //불장판의 크기를 시각적으로 보이게하는 원 범위 생성
        aoeRangeindicator = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 6,fire.transform); 
        aoeRangeindicator.transform.localPosition = Vector3.zero; // 위치 초기화

    }
    public void Damage(float damage,float fireDamage)
    {
        this.damage = damage;
        this.fireDamage = fireDamage;
    }
    void OnTriggerEnter(Collider other)
    {
        //날라오는 불타는돌을 맞았을때는 폭발이펙트만 재생되고 투사체는 사라짐
        if (other.CompareTag("Player")) 
        {
            GameManager.instance.player.GetHit(damage);
            ExplosionEffect(other.transform);
            gameObject.SetActive(false);
        }
    }
}
