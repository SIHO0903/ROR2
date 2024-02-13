using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour, IIBoss
{
    [Header("SpawnData")]
    [SerializeField] float spawnRate;
    float curSpawnRate;
    [SerializeField] float spwanRange;
    [SerializeField] float maxYPos;
    [SerializeField] float curYPosOffset;
    float curYPos;
    float distance;

    [Header("BossData")]
    [SerializeField] GameObject teleporterEventSphere;
    [SerializeField] float sphereSize;
    [SerializeField] float sphereTime;
    [HideInInspector] public static float teleportGauge=0f;

    [Header("Box")]
    [SerializeField] int boxCount;
    GameObject[] boxs;
    GameObject goldBox;
    Vector3 goldBoxPos = new Vector3(69f, 1f, 35.6f);
    Quaternion goldBoxRot = Quaternion.Euler(-90f, 0f, 90f);
    string goldBoxName = "GoldChest";

    RaycastHit raycastHit;
    GameObject enemy;
    GameObject boss;
    [SerializeField] Stage stage;

    private void Start()
    {
        boxs = new GameObject[boxCount];
        SpawnBox();

    }
    void Update()
    {
        curSpawnRate += Time.deltaTime;
        SpawnEnemy();
        TeleporterEventInProgress();
    }

    private void SpawnEnemy()
    {
        if (curSpawnRate >= spawnRate)
        {
            enemy = PoolManager.instance.Get(PoolManager.PrefabType.Enemy, Random.Range(0, 4));
            float maxhealth = enemy.GetComponent<Enemy>().maxHealth;
            float damage = enemy.GetComponent<Enemy>().damage;
            stage.LevelManager(maxhealth, damage);
            enemy.transform.position = RandomPos();
            enemy.SetActive(true);
            curSpawnRate = 0;
        }
    }

    Vector3 RandomPos()
    {
        Vector3 randomPos = Vector3.zero;
        Vector3 playerPos = GameManager.instance.player.orientation.position;
        curYPos = maxYPos;

        float xRandomPos = Random.Range(-spwanRange, spwanRange);
        float zRandomPos = Random.Range(-spwanRange, spwanRange);

        float xPos = playerPos.x  + xRandomPos;
        float zPos = playerPos.z  + zRandomPos;

        randomPos = new Vector3(xPos, maxYPos, zPos);
        Physics.Raycast(randomPos, Vector3.down, out raycastHit);
        curYPos = raycastHit.point.y + curYPosOffset;

        randomPos = new Vector3(xPos, curYPos, zPos);
        distance = Vector3.Distance(randomPos, playerPos);

        if (distance > 10 && distance < 30)
            return randomPos;
        else
            return RandomPos();
    }
    public void SpawnBoss()
    {
        //Random.Range(0, 5)
        boss = PoolManager.instance.Get(PoolManager.PrefabType.Enemy,4);
        boss.transform.position = RandomPos();
        boss.GetComponent<Enemy>().isBoss = true;
        boss.AddComponent<Boss>();
        boss.SetActive(true);
        TeleporterEvent();
        AudioManager.instance.BGMPlayerBossSpawn(AudioManager.BGM.Boss);
    }
    void TeleporterEvent()
    {
        teleporterEventSphere.SetActive(true);
        teleporterEventSphere.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * sphereSize, sphereTime);
    }
    void TeleporterEventInProgress()
    {
        if(teleporterEventSphere.activeSelf && IsInTeleportEven.inTeleport)
        {
            //텔레포터게이지가참
            if (teleportGauge <= 100)
            {
                teleportGauge += Time.deltaTime*4f;
                GameManager.instance.teleporterGaugeTxt.text = string.Format("Charge The Teleporter ({0:F0}%)",teleportGauge);

            }

        }
    }
    void SpawnBox()
    {
        // x : -10~130 / y : 0.5~0.7 / z:-80~80 
        
        for (int i = 0; i < boxCount; i++)
        {
            boxs[i] = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 3);
            boxs[i].transform.position = new Vector3(Random.Range(-10f, 130f), Random.Range(0.3f, 0.5f), Random.Range(-80f, 80f));
        }

        goldBox = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 3);
        goldBox.transform.position = goldBoxPos;
        goldBox.transform.rotation = goldBoxRot;
        goldBox.name = goldBoxName;
        for (int i = 0; i < 2; i++)
        {
            goldBox.GetComponentsInChildren<MeshRenderer>()[i].material.color = Color.yellow;
        }

    }
}
