using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleIsPlaying : MonoBehaviour
{
    ParticleSystem particle;
    [SerializeField] bool damageCheck;
    float timer;
    public float damage;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        timer = 0.2f;
    }
    void Update()
    {
        if(!particle.isPlaying)
            gameObject.SetActive(false);
    }
    public void Damage(float damage)
    {
        this.damage = damage;
    }
    private void OnTriggerStay(Collider other)
    {
        if(damageCheck && other.CompareTag("Player") && GameManager.instance.player.isGround)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                GameManager.instance.player.curHealth -= damage;
                timer = 0;
            }
        }
    }
}
