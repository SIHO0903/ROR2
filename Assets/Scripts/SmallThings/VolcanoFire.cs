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
    //���� ��ƼŬ����Ʈ
    public void ExplosionEffect(Transform transform)
    {
        explosion = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 4);
        explosion.transform.position = transform.position;
        explosion.GetComponent<ParticleSystem>().Play();
    }
    //������ ��ƼŬ����Ʈ
    public void FireEffect(Transform transform)
    {
        fire = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 5); //������ ��ƼŬ ����

        //��������� ���� ��ġ �缳��
        Physics.Raycast(transform.position, Vector3.down, out raycastHit);
        newPos = new Vector3(transform.position.x, raycastHit.transform.position.y+ yOffset, transform.position.z);
        fire.transform.position = newPos; 
        fire.GetComponent<ParticleIsPlaying>().Damage(fireDamage); //������ ������

        //������ ũ�� ����
        fire.transform.localScale = fireRange;
        fire.transform.GetChild(0).localScale = fireRange;

        fire.GetComponent<ParticleSystem>().Play();

        //�������� ũ�⸦ �ð������� ���̰��ϴ� �� ���� ����
        aoeRangeindicator = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 6,fire.transform); 
        aoeRangeindicator.transform.localPosition = Vector3.zero; // ��ġ �ʱ�ȭ

    }
    public void Damage(float damage,float fireDamage)
    {
        this.damage = damage;
        this.fireDamage = fireDamage;
    }
    void OnTriggerEnter(Collider other)
    {
        //������� ��Ÿ�µ��� �¾������� ��������Ʈ�� ����ǰ� ����ü�� �����
        if (other.CompareTag("Player")) 
        {
            GameManager.instance.player.GetHit(damage);
            ExplosionEffect(other.transform);
            gameObject.SetActive(false);
        }
    }
}
