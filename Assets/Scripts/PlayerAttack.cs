using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [Header("Info")]
    public float finalDamage;
    public float damage;
    public float criticalChance=0f;

    [Header("NormalAttack")]
    public float attackRate;
    public float finalAttackRate;
    float curAttackRate;
    public Transform bulletPos;
    bool isAttack;
    [HideInInspector] public bool isCritical;


    [Header("Glaive")]
    public float glaiveAtkRate;
    float curGlaiveAtkRate=0;
    public Transform glaivePos;
    bool canGlaive = true;

    [Header("SpecialAttack")]
    public float specAtkRate;
    float curSpecAtkRate=0;
    Vector3 curPos;
    Vector3 endPos;
    public Transform specialAtkPos;
    bool changePos;
    const int baseAim = 0;
    const int specialAim = 2;
    float curTime;
    [SerializeField] float lerpTime;
    bool tripleShot;
    int specialSkillCount = 3;
    RaycastHit hitInfo;
    [Header("SkillUI")]
    [SerializeField] Image subSkill;
    [SerializeField] Image specialSkill;
    [SerializeField] Image itemSkill;

    Image subSkillCool;
    TextMeshProUGUI subSkillCoolTime;
    Image specialSkillCool;
    TextMeshProUGUI specialSkillCoolTime;
    Image itemSkillCool;
    TextMeshProUGUI itemSkillCoolTime;



    [HideInInspector] public bool useRootMotion;
    GameObject bullet;
    GameObject glaive;
    GameObject specBullet;
    PlayerMove playerMove;
    NearestTarget nT;
    Item item;
    [HideInInspector] public Camera myCamera;
    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        nT = GetComponent<NearestTarget>();
        item = GetComponentInChildren<Item>();

        subSkillCool = subSkill.transform.GetChild(0).GetComponent<Image>();
        subSkillCoolTime = subSkill.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        specialSkillCool = specialSkill.transform.GetChild(0).GetComponent<Image>();
        specialSkillCoolTime = specialSkill.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        itemSkillCool = itemSkill.transform.GetChild(1).GetComponent<Image>();
        itemSkillCoolTime = itemSkill.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        myCamera = Camera.main;
    }

    private void Update()
    {
        curAttackRate += Time.deltaTime;
        curGlaiveAtkRate -= Time.deltaTime;
        curSpecAtkRate -= Time.deltaTime;
        RangeAttack();
        if(canGlaive)
            GlaiveAttack();
        if (changePos)
            ChangePos();
        SpecialAttack();
        SkillUI();
    }
    #region Skill
    private void RangeAttack()
    {
        if (Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.ATTACK]) && curAttackRate >= 1 / AttackRateCal() && !tripleShot && nT.NearestVisibleTarget() != null)
        {

            playerMove.anim.SetTrigger("Attack");
            isAttack = true;
            curAttackRate = 0;
            MissileChance(nT.NearestVisibleTarget(), DamageCal());
        }
        else if (Input.GetKeyDown(KeySoundSetManager.instance.keyValues[KeyAction.ATTACK]) && tripleShot && specialSkillCount > 0)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            hitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo))
            {
                specBullet = PoolManager.instance.Get(PoolManager.PrefabType.Player, 4);
                specBullet.gameObject.SetActive(false);
                specBullet.transform.position = bulletPos.position;
                specBullet.gameObject.SetActive(true);
                specBullet.GetComponent<BulletFire>().Dir(bulletPos.position, hitInfo.point, DamageCal()*5f);
                playerMove.anim.SetTrigger("SpecAttack");
                specialSkillCount--;
                curAttackRate = 0;
            }
        }

        if (isAttack)
        {
            bullet = PoolManager.instance.Get(PoolManager.PrefabType.Player, 0);
            bullet.transform.position = bulletPos.position;
            bullet.SetActive(false);
            isAttack = false;
        }


        if (GameManager.instance.CheckAnimationPlay(playerMove.anim, "Base Layer.StandShot", 0.42f, true) ||
            GameManager.instance.CheckAnimationPlay(playerMove.anim, "Base Layer.MovingShot", 0.38f, true))
        {
            bullet.transform.position = bulletPos.position;
            bullet.SetActive(true);
            bullet.GetComponent<BulletFire>().Dir(bulletPos.position, nT.nearestTarget.position, DamageCal());
        }

        playerMove.anim.SetFloat("AttackSpeed", AttackRateCal() / attackRate);
    }
    private void GlaiveAttack()
    {
        //useRootMotion = playerMove.anim.GetBool("GlaiveAtk") ? true : false;

        if (Input.GetKeyDown(KeySoundSetManager.instance.keyValues[KeyAction.SKILL1]) && curGlaiveAtkRate <=0 && !tripleShot && nT.NearestVisibleTarget() != null)
        {
            glaive = PoolManager.instance.Get(PoolManager.PrefabType.Player, 2);
            glaive.transform.position = glaivePos.position;
            glaive.gameObject.SetActive(false);
            playerMove.anim.SetTrigger("GlaiveAtk");
            curGlaiveAtkRate = glaiveAtkRate;
            MissileChance(nT.NearestVisibleTarget(), DamageCal());
        }
        if (GameManager.instance.CheckAnimationPlay(playerMove.anim, "Base Layer.GlaiveAttack", 0.24f, true))
        {
            glaive.gameObject.SetActive(true);
            glaive.transform.position = glaivePos.position;
            glaive.GetComponent<GlaiveFire>().Dir(glaivePos.position, nT.nearestTarget.position, DamageCal() * 3f);

        }
    }
    void SpecialAttack()
    {
        if (Input.GetKeyDown(KeySoundSetManager.instance.keyValues[KeyAction.SKILL2]) && curSpecAtkRate <=0)
        {
            curTime = 0;
            changePos = true;
            tripleShot = true;
            playerMove.anim.SetBool("tripleShot", tripleShot);
            playerMove.anim.SetTrigger("ReadySpecAttack");
            curPos = transform.position;
            endPos = specialAtkPos.position;
            transform.GetComponent<CapsuleCollider>().enabled = false;
            playerMove.anim.SetBool("isGround", true);

            playerMove.AimUIController(specialAim);

            playerMove.finalSpeed = 0;
            playerMove.canMove = false;
            playerMove.readyToJump = false;
            canGlaive = false;
        }
        if(tripleShot)
        {
            //상체가 위아래로 움직이게하는 애니메이션
            playerMove.anim.SetFloat("SpecZ", -myCamera.transform.rotation.x); //up,down
            playerMove.archerObj.rotation = playerMove.orientation.rotation;

        }
        //3발다쏜후 쏘기전상태로 초기화
        if (specialSkillCount <= 0)
        {
            tripleShot = false;
            playerMove.anim.SetBool("tripleShot", tripleShot);
            playerMove.anim.SetBool("isGround", false);
            specialSkillCount = 3;
            playerMove.rigid.useGravity = true;
            playerMove.finalSpeed = playerMove.moveSpeed;
            transform.GetComponent<CapsuleCollider>().enabled = true;
            playerMove.canMove = true;
            playerMove.readyToJump = true;
            canGlaive = true;
            transform.localScale = Vector3.one;

            playerMove.AimUIController(baseAim);
            curSpecAtkRate = specAtkRate;

        }
    }

    void ChangePos()
    {
        curTime += Time.deltaTime;
        if (curTime >= lerpTime)
        {
            curTime = lerpTime;
            changePos = false;
            playerMove.rigid.useGravity = false;
            transform.localScale = Vector3.zero;
        }
        transform.position = Vector3.Lerp(curPos, endPos , curTime / lerpTime);
    }
    #endregion 
    void SkillUI()
    {
        CoolTimeUI(curGlaiveAtkRate, subSkillCool, subSkillCoolTime);
        CoolTimeUI(curSpecAtkRate, specialSkillCool, specialSkillCoolTime);
        CoolTimeUI(item.curCoolTime, itemSkillCool, itemSkillCoolTime);
    }
    public void CoolTimeUI(float curCoolTime,Image coolimage,TextMeshProUGUI coolTimeTxt)
    {
        if (curCoolTime > 0)
        {
            coolimage.gameObject.SetActive(true);
            coolTimeTxt.gameObject.SetActive(true);
            coolTimeTxt.text = string.Format("{0:F0}", curCoolTime);
        }
        else
        {
            coolimage.gameObject.SetActive(false);
            coolTimeTxt.gameObject.SetActive(false);
        }
    }
    float DamageCal()
    {
        if (Random.value < criticalChance)
        {
            item.wolfpeltTime = 3f;
            if(item.wolfpeltStack < item.wolfpeltMaxStack)
                item.wolfpeltStack++;
            finalDamage = damage * ExtraDamage() * 2;
            isCritical =true;
        }
        else
        {
            isCritical = false;
            finalDamage = damage * ExtraDamage();
        }
        return finalDamage;
    }
    public bool DotChance()
    {
        Debug.Log("DotChacneCal...");
        if(item.triTipLvl==0)
            return false;
        else if(item.triTipLvl >= 10)
            return true;

        if (Random.value <= item.tritipChance * item.triTipLvl)
            return true;
        else
            return false;
    }
    public float DotDamage()
    {
        FinalStats.allDamage += finalDamage * item.tritipDamage;
        return finalDamage * item.tritipDamage;
    }

    float ExtraDamage()
    {
        float extraDamage;
        extraDamage = (1 + item.delicateWatch * item.delicateWatchLvl);
        return extraDamage;
    }
    float AttackRateCal()
    {
        float coffee   = item.coffeeAttackRate;
        float syringe  = item.syringeAttackRate;
        float wolfpelt = item.wolfpeltAttackRate * item.wolfpeltStack;

        finalAttackRate = attackRate * (1 + coffee + syringe + wolfpelt);
        return finalAttackRate;
    }

    void MissileChance(Transform dirPos,float damage)
    {
        //if (item.missileLauncherLvl == 0)
        //    return;
        //if (Random.value <= item.missileLauncherChance)
        //{
        //    GameManager.instance.MissileLauncher(transform.position, dirPos, damage);
        //}
    }
}

