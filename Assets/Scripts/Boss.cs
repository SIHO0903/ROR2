using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [Header("Volcano")]
    [SerializeField] int volcanoCount = 10;
    public float damage = 50f;
    public float fireDamage = 2f;
    GameObject[] volcano;
    WaitForSeconds wfs;

    [Header("Laser")]
    Laser laser;
    [SerializeField] float laserDamage = 1f;

    [Header("Etc")]
    [SerializeField] float attackTimer;
    [SerializeField] float bossHealth = 50f; //test
    int attackType;
    Enemy enemy;
    TextMeshProUGUI healTxt;
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        healTxt = enemy.healthBar.GetComponentInChildren<TextMeshProUGUI>();
        volcano = new GameObject[volcanoCount];
        wfs = new WaitForSeconds(0.1f);
        laser = GetComponentInChildren<Laser>(true);
    }
    private void OnEnable()
    {
        enemy.maxHealth = bossHealth;
        enemy.health = bossHealth;
        GameManager.instance.isBossDie = false;
    }
    void Update()
    {
        attackTimer += Time.deltaTime;
        healTxt.text = enemy.maxHealth + " / " + enemy.health;
        BossRogic();
        BossDie();
    }
    #region BossRogic
    private void BossRogic()
    {

        if(attackTimer > 5f)
        {
            Attack();
            attackTimer = 0;
        }
    }
    void Attack()
    {
        attackType = Random.Range(0, 2);
        switch (attackType)
        {
            case 0:
                StartCoroutine(Volcano());
                break;
            case 1:
                Laser();
                break;
        }
    }
    IEnumerator Volcano()
    {
        for (int i = 0; i < volcanoCount; i++)
        {
            volcano[i] = PoolManager.instance.Get(PoolManager.PrefabType.EnemyBullet, 2);
            volcano[i].GetComponent<VolcanoFire>().Damage(damage, fireDamage);
            volcano[i].SetActive(false);
            volcano[i].transform.position = transform.position;
            volcano[i].SetActive(true);
            AudioManager.instance.SFXPlayer(AudioManager.SFX.Volcano);
            yield return wfs;
        }
    }
    void Laser()
    {
        laser.transform.parent.gameObject.SetActive(true);
        laser.Damage(laserDamage);
    }
    #endregion
    void BossDie()
    {
        if (enemy.health <= 0)
        {
            GameManager.instance.isBossDie = true;
            AudioManager.instance.BGMPlayerBossDie();
        }
    }
}
