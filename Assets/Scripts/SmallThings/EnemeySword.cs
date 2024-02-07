using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeySword : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    private void OnTriggerEnter(Collider other)
    {
        if(enemy.isAttacking && other.CompareTag("Player") && enemy.hitCount>=1)
        {
            enemy.hitCount--;
            GameManager.instance.player.GetHit(enemy.damage);
        }

    }
}
