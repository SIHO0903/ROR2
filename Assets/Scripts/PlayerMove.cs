using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using WarriorAnims;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float finalSpeed;
    public float baseSpeed;
    public float moveSpeed;
    public float groundDrag;
    public bool canMove =true;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump = true;

    [Header("Run")]
    public float runSpeed;
    public bool isRun;

    [Header("Dash")]
    public float dashSpeed;
    public float dashCoolTime;
    float curDashCoolTime=0;
    public bool canDash;
    const int RunAim = 1;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool isGround;

    [Header("AimUI")]
    public GameObject aimUI;

    [Header("Status")]
    public float maxHealth;
    public float curHealth;
    public float curRecoveryHealth;
    float recoveryTime;
    public float curShield;
    public float maxShield;
    public float getHitTime;
    float curGetHitTime;
    [HideInInspector] public float addHealth = 0;

    [Header("Input")]
    public Transform orientation;
    public Transform archerObj;
    float horizontalInput;
    public float verticalInput;

    [Header("SkillUI")]
    [SerializeField] Image dashSkill;
    Image dashSkillCool;
    TextMeshProUGUI dashSkillCollTime;


    [Header("Components")]
    public GameObject healthUI;
    [HideInInspector] public Vector3 moveDirection;
    [HideInInspector] public Rigidbody rigid;
    [HideInInspector] public Animator anim;
    [SerializeField] Image background;
    [SerializeField] Image status;
    TextMeshProUGUI statusTxt;

    Slider healthBar;
    Slider shieldBar;
    TextMeshProUGUI healthTxt;
    Interactable interactable;
    [HideInInspector] public PlayerAttack playerAttack;
    [HideInInspector] public Item item;
    [HideInInspector] public FadeIn fadeIn;
    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        playerAttack = GetComponent<PlayerAttack>();
        item = GetComponentInChildren<Item>();
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
        anim = GetComponentInChildren<Animator>();
        fadeIn = GetComponent<FadeIn>();
        healthBar = healthUI.GetComponentsInChildren<Slider>()[0];
        shieldBar = healthUI.GetComponentsInChildren<Slider>()[1];
        healthTxt = healthUI.GetComponentInChildren<TextMeshProUGUI>();
        statusTxt = status.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        dashSkillCool = dashSkill.transform.GetChild(0).GetComponent<Image>();
        dashSkillCollTime = dashSkill.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        curHealth = maxHealth + addHealth;
    }
    private void Update()
    {
        curDashCoolTime -= Time.deltaTime;
        curGetHitTime += Time.deltaTime;

        anim.SetFloat("Moving", moveDirection.magnitude);
        GroundCheck();
        if(canMove) InputController();
        SpeedControl();
        RunControl();
        Health();
        LookInventory();
        interactable.InteractableObj();
        playerAttack.CoolTimeUI(curDashCoolTime, dashSkillCool, dashSkillCollTime);
    }
    #region Move
    private void GroundCheck()
    {
        isGround = Physics.Raycast(orientation.position, Vector3.down, playerHeight * 0.5f + 0.4f, groundLayer);
        anim.SetBool("isGround", isGround);
    }

    private void FixedUpdate()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (canDash)
            rigid.AddForce(playerAttack.myCamera.transform.forward.normalized * finalSpeed * 10f, ForceMode.Force); //플레이어오브젝트가 보고잇는 방향 (화면상보이는방향이아님)
        else
            rigid.AddForce(moveDirection.normalized * finalSpeed * 10f, ForceMode.Force);

    }
    void InputController()
    {
        bool wKey = Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.UP]);
        bool sKey = Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.DOWN]);
        bool aKey = Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.LEFT]);
        bool dKey = Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.RIGHT]);

        horizontalInput = (aKey ? -1f : 0f) + (dKey ? 1f : 0f);
        verticalInput = (sKey ? -1f : 0f) + (wKey ? 1f : 0f);
        anim.SetFloat("VelocityY", rigid.velocity.y);

        if (Input.GetKeyDown(KeySoundSetManager.instance.keyValues[KeyAction.JUMP]) && readyToJump && isGround)
        {
            readyToJump = false;

            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(KeySoundSetManager.instance.keyValues[KeyAction.DASH]) && curDashCoolTime <= 0)
        {
            Debug.Log("대쉬온");
            curDashCoolTime = dashCoolTime;
            canDash = true;
            StopCoroutine(DashOverTimeCoroutine());
            StartCoroutine(DashOverTimeCoroutine());
            anim.SetTrigger("Dash");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isRun = !isRun;
        }

    }

    IEnumerator DashOverTimeCoroutine()
    {
        yield return new WaitForSeconds(1f);
        canDash = false;
    }
    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        if(flatVel.magnitude > finalSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * finalSpeed;
            rigid.velocity = new Vector3(limitedVel.x, rigid.velocity.y, limitedVel.z);
        }

        if ((isRun || !isRun) && canDash)
            finalSpeed = dashSpeed;
        else if (isRun && !canDash)
            finalSpeed = runSpeed;
        else
            finalSpeed = moveSpeed;

        MovementSpeedCal();
    }
    void RunControl()
    {
        if (isRun)
            isRun = verticalInput == 1 ? true : false;
    }
    void Jump()
    {
        anim.SetTrigger("Jumping");
        //reset y velocity
        rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);

        if(isGround)
            rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        else
            rigid.AddForce(transform.up * jumpForce * airMultiplier, ForceMode.Impulse);

        if (isGround)
            rigid.drag = groundDrag;
        else
            rigid.drag = 0;



    }
    void ResetJump()
    {
        readyToJump = true;
    }
    #endregion
    void MovementSpeedCal()
    {
        moveSpeed = baseSpeed + baseSpeed * (item.coffeeMoveSpeed * item.coffeeLvl);
        runSpeed = moveSpeed * 1.4f;
        dashSpeed = moveSpeed * 2f;
    }
    void Health()
    {
        ShieldGenerator();
        healthBar.value = curHealth / MaxHealth();
        healthTxt.text = string.Format("{0:F0}", curHealth + curShield) + " / " + MaxHealth();
        shieldBar.value = curShield / MaxHealth();

        HealthRecovery();

        if (curHealth <= MaxHealth() * 0.25f && item.delicateWatchLvl > 0)
        {
            item.DelicateWatchBroken(MaxHealth(), curHealth);
        }
        if (curHealth <= MaxHealth() * 0.25f && item.healingPotionLvl > 0)
        {
            item.HealingPotionUse(MaxHealth(), curHealth);
        }
        Die();
    }
    void Die()
    {
        if (curHealth <= 0)
        {
            GameManager.instance.ItemBoxSave("Lose...");
            AudioManager.instance.BGMPlayerBossDie();
        }
    }
    private void HealthRecovery()
    {
        if (curHealth < MaxHealth())
        {
            recoveryTime += Time.deltaTime;
            if (recoveryTime >= 0.5f)
            {
                curHealth += curRecoveryHealth;
                recoveryTime = 0;
            }
        }
        else
        {
            curHealth = MaxHealth();
        }
    }
    void LookInventory()
    {
        if (Input.GetKeyDown(KeySoundSetManager.instance.keyValues[KeyAction.INVENTORY]))
        {
            background.gameObject.SetActive(true);
            status.gameObject.SetActive(true);
            statusTxt.text = string.Format("Level : {0}\nHealth : {1}\nHpRen : {2}\nShield : {3}\nDamage : {4}\nCritical : {5}%\nAtkSpeed : {6:F1}\nSpeed : {7}\ndashSpeed : {8}",
                                           GameManager.instance.level,maxHealth,curRecoveryHealth,maxShield,playerAttack.damage,playerAttack.criticalChance*100,
                                           playerAttack.finalAttackRate,moveSpeed,runSpeed);
            CameraMovement.CursorOnOff(false);

            //Time.timeScale = 0f;
        }
        else if (Input.GetKeyUp(KeySoundSetManager.instance.keyValues[KeyAction.INVENTORY]))
        {
            background.gameObject.SetActive(false);
            status.gameObject.SetActive(false);
            CameraMovement.CursorOnOff(true);
            Time.timeScale = 1f;
        }


    }

    void ShieldGenerator()
    {
        maxShield = Mathf.RoundToInt(MaxHealth() * item.shieldGeneratorLvl * item.shieldAmont);

        if (curGetHitTime >= 3f && item.shieldGeneratorLvl>0 && maxShield>=curShield)
        {
            curShield += Time.deltaTime*3f;
        }

    }
    float MaxHealth()
    {
        return maxHealth + addHealth;
    }
    public void GetHit(float damage)
    {
        curGetHitTime = 0;
        float damageReduce = item.armorPlateDamageReduce * item.armorPlateLvl;
        if (damage - damageReduce <= 0)
        {
            damage = 1;
        }
        else
        {
            damage = damage - damageReduce;
        }
        FinalStats.allDamageTaken += damage;
        if(curShield>0)
        {
            curShield -= damage;
            if (curShield < 0)
                curHealth -= curShield;
        }
        else 
            curHealth -= damage;
    }
    //0 : base, 1 : run, 2 : specialSkill
    public void AimUIController(int num)
    {
        for (int i = 0; i < aimUI.transform.childCount; i++)
        {
            aimUI.transform.GetChild(i).gameObject.SetActive(false);
        }
        aimUI.transform.GetChild(num).gameObject.SetActive(true);
    }
}
