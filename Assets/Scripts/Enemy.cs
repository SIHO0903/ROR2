using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float jumpPower;
    public LayerMask groundLayer;
    bool isWalk;
    //float yDistance;

    [Header("Status")]
    public float maxHealth;
    public float health;
    [HideInInspector] public bool isBoss;
    int bossRock;

    [Header("Attack")]
    [SerializeField] float meleeDistance;
    [SerializeField] float rangeDistance;
    [SerializeField] float AttackRate;
    public float damage;
    [SerializeField] bool canRangeAttack;
    [SerializeField] bool isTwoMeleeAttack;
    [HideInInspector] public bool isAttacking;

    float DotCount;

    float curDamagedTime;

    bool isDot;
    float curAttackRate;
    float curDistance;
    [HideInInspector] public int hitCount=1;

    public Transform rightHand;
    GameObject rock;
    //Components
    [HideInInspector] public Slider healthBar;
    Transform target;
    Animator anim;
    NavMeshAgent enemyAI;
    Vector3 destination;
    SkinnedMeshRenderer skinnedMesh;
    MeshCollider meshCollider;
    Mesh mesh;
    int popCount;

    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAI = GetComponent<NavMeshAgent>();
        healthBar = GetComponentInChildren<Slider>();
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        mesh = new Mesh();
    }
    private void OnEnable()
    {
        meshCollider.enabled = true;
        gameObject.layer = 8;
        destination = enemyAI.destination;
        curAttackRate = 0f;
        health = maxHealth;
        target = GameManager.instance.player.orientation;
        enemyAI.stoppingDistance = meleeDistance;
        enemyAI.enabled = true;
        popCount = 3;
        isWalk = true;

    }


    private void Update()
    {
        curAttackRate += Time.deltaTime;
        healthBar.value = health / maxHealth;
        GenerateCollider();

        Die();
        StopDuringAttack();

        //Check curDistance between player and enemy
        curDistance = Vector3.Distance(transform.position, target.position);
        //enemy Move
        if (isWalk)
        {
            //if (enemyAI != null && enemyAI.isActiveAndEnabled)
            //enemyAI.destination = target.position;
            if (enemyAI.enabled)
            {
                destination = target.position;
                enemyAI.destination = destination;
            }


        }

        //enemyAI.destination = new Vector3(target.position.x, transform.position.y, target.position.z);

        TryMeleeAttack();
        if(canRangeAttack)
            TryRangeAttack();

        ExtraDamaged();

    }


    private void Die()
    {
        if (health <= 0)
        {
            meshCollider.enabled = false;
            gameObject.layer = 0;
            if (GameManager.instance.CheckAnimationPlay(anim,"Base Layer.Die", 0.99f, false))
            {
                FinalStats.kill += 1;
                AudioManager.instance.SFXPlayer(AudioManager.SFX.EnemyDead);
                gameObject.SetActive(false);
            }
            else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Die"))
            {
                isWalk = false;
                enemyAI.stoppingDistance = 999f;

                anim.SetTrigger("Die");
                SpoilsPopUp(); //코인,경험치 드랍
            }
        }
        else
        {
            anim.SetBool("Walk", isWalk);
        }

    }

    private void SpoilsPopUp()
    {

        while (popCount>0)
        {
            transform.GetChild(0).Rotate(Random.Range(-20, 20), 0f, Random.Range(-20, 20));
            GameObject coins = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 1);
            coins.transform.position = transform.GetChild(0).position;
            coins.transform.rotation = transform.GetChild(0).rotation;
            coins.GetComponent<Rigidbody>().velocity = coins.transform.up * 10f;
            coins.GetComponent<CoinExpPop>().money = 10; //나중에 로직써서 바꾸기
            FinalStats.goldCollected += 10;

            GameObject exp = PoolManager.instance.Get(PoolManager.PrefabType.Environment, 2);
            exp.transform.position = transform.GetChild(0).position;
            //exp.transform.rotation = transform.GetChild(0).rotation;
            exp.transform.Rotate(Random.Range(-25, 25), 0f, Random.Range(-25, 25));
            exp.GetComponent<Rigidbody>().velocity = coins.transform.up * 10f;
            exp.GetComponent<CoinExpPop>().exp = 15f;


            popCount--;
        }


    }

    void GenerateCollider()
    {

        skinnedMesh.BakeMesh(mesh);
        meshCollider.sharedMesh = mesh;
        meshCollider.transform.position = skinnedMesh.transform.position;
        meshCollider.transform.rotation = skinnedMesh.transform.rotation;
        skinnedMesh.BakeMesh(meshCollider.sharedMesh);
    }

    #region Attack


    private void TryMeleeAttack()
    {
        if (curDistance <= meleeDistance)
        {
            enemyAI.stoppingDistance = meleeDistance;
            isWalk = false;
            MeleeAttack();
        }

    }

    private void StopDuringAttack()
    {
        isAttacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.MeleeAttack") ||
                           anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.MeleeAttack1") ||
                           anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.RangeAttack");
        if (!isAttacking && health>0)
        {
            isWalk = true;
            enemyAI.stoppingDistance = 0f;
        }
    }



    void MeleeAttack()
    {
        if (curAttackRate > AttackRate)
        {
            if (isTwoMeleeAttack)
            {
                int rndInt;
                rndInt = Random.Range(0, 2);
                if (rndInt == 0)    anim.SetTrigger("MeleeAttack");
                else                anim.SetTrigger("MeleeAttack1");
            }
            else
                anim.SetTrigger("MeleeAttack");


            hitCount = 1;
            curAttackRate = 0;
        }
    }
    private void TryRangeAttack()
    {
        if (curDistance <= rangeDistance)
        {
            RangeAttack();
        }
    }
    void RangeAttack()
    {
        if (curAttackRate/2f > AttackRate)
        {
            rock = PoolManager.instance.Get(PoolManager.PrefabType.EnemyBullet, bossRock = isBoss == true ? 1 : 0);
            Debug.Log(bossRock + " / " + isBoss);
            rock.transform.position = rightHand.position;
            anim.SetTrigger("RangeAttack");
            hitCount = 1;
            curAttackRate = 0;
            isWalk = false;

        }
        if (GameManager.instance.CheckAnimationPlay(anim,"Base Layer.RangeAttack", 0.58f,false))
        {
            rock.GetComponent<BulletFire>().Dir(rightHand.transform.position, target.position, damage);
            AudioManager.instance.SFXPlayer(AudioManager.SFX.EnemyThrow);
            curAttackRate = 0;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.RangeAttack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.58f)
        {
            enemyAI.stoppingDistance = rangeDistance;
            if (rock != null)
                rock.transform.position = rightHand.position;
        }

    }
    #endregion

    void ExtraDamaged()
    {
        if(DotCount>0)
        {
            curDamagedTime += Time.deltaTime;
            if (curDamagedTime >= 1f)
            {
                health -= GameManager.instance.player.playerAttack.DotDamage();
                DamageText.Create(transform.GetChild(0).position, GameManager.instance.player.playerAttack.DotDamage(), null, Color.red);
                DotCount--;
                curDamagedTime = 0;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //적중시
        if (other.CompareTag("Bullet"))
        {
            float hitDamage;
            if (other.GetComponent<BulletFire>() == null)
                hitDamage = other.GetComponent<GlaiveFire>().damage;
            else
                hitDamage = other.GetComponent<BulletFire>().damage;
            health -= hitDamage;
            Debug.Log("적중함");
            AudioManager.instance.SFXPlayer(AudioManager.SFX.EnemyHit);

            DamageText.Create(transform.GetChild(0).position, hitDamage,null, GameManager.instance.player.playerAttack.isCritical ? Color.yellow : Color.white);
            //DamageText.Create(transform.GetChild(0).position, hitDamage,transform,Color.white);
            FinalStats.allDamage += hitDamage;
            isDot = GameManager.instance.player.playerAttack.DotChance();
            if (isDot)
                DotCount = 3;
            GameManager.instance.player.curHealth += GameManager.instance.player.item.seedLvl;
            //if(health>0)
            //    anim.SetTrigger("GetHit");
            if (other.GetComponent<BulletFire>() != null)
                other.GetComponent<BulletFire>().gameObject.SetActive(false);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && hitCount>=1)
        {
            hitCount--;
            GameManager.instance.player.GetHit(damage);
            //GameManager.instance.player.curHealth -= damage;
        }
    }

}
