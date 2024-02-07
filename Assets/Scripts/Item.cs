using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

//플레이어 자식오브젝트에잇음
public class Item : MonoBehaviour
{
    [SerializeField] GameObject itemToolTip;
    [Header("UI")]
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDesc;
    [SerializeField] GameObject itemBox;
    float toolTipTimer;
    public static bool timerLock =true;
    [Header("Used or Broken")]
    [SerializeField] Sprite delicateWatchBroken;
    [SerializeField] Sprite healingpotionUsed;
    [SerializeField] TMP_FontAsset fontAsset;

    [SerializeField] GameObject[] item;
    Dictionary<string, GameObject> itemMesh;
    [HideInInspector] public int steakLvl, syringeLvl, wolfPeltLvl, seedLvl, glassesLvl, coffeeLvl,
                                 delicateWatchLvl, armorPlateLvl, healingPotionLvl, shieldGeneratorLvl,
                                 triTipLvl, missileLauncherLvl;

    bool useableItem;
    string curItemName;
    float coolTime;
    [HideInInspector] public float curCoolTime;
    [SerializeField] Image useItem;
    //ForeignFruit
    [HideInInspector] public float fruitCoolTime = 45f;
    //OcularHUD
    [HideInInspector] public float ocularHUDCoolTime = 60f;
    [HideInInspector] public float originCriticalChance;
    [HideInInspector] public float ocularHUDDuration = 8f;
    bool isOcularHUDOn;
    // Steak
    [HideInInspector] public float steakHealth=25f;

    // Syringe
    [HideInInspector] public float syringeAttackRate;

    // WolfPelt
    [HideInInspector] public float wolfpeltTime = 0f;
    [HideInInspector] public int wolfpeltMaxStack =0;
    [HideInInspector] public int wolfpeltStack = 0;
    [HideInInspector] public float wolfpeltAttackRate = 0.12f;

    // Coffee
    [HideInInspector] public float coffeeAttackRate = 0.075f;
    [HideInInspector] public float coffeeMoveSpeed = 0.075f;

    // DelicateWatch
    [HideInInspector] public float delicateWatch;

    // ArmorPlate
    [HideInInspector] public float armorPlateDamageReduce;

    // ShieldGenerator
    [HideInInspector] public float shieldAmont;

    //TriTip
    [HideInInspector] public float tritipChance;
    [HideInInspector] public float tritipDamage;

    //MissileLauncher
    [HideInInspector] public float missileLauncherChance;
    [HideInInspector] public float missileLauncherDamageMultiple;


    private void Start()
    {
        itemMesh = new Dictionary<string, GameObject>();
        for (int i = 0; i < item.Length; i++)
        {
            itemMesh.Add(item[i].name, item[i]);
        }

    }
    private void Update()
    {
        if (wolfpeltStack > 0)
        {
            wolfpeltTime -= Time.deltaTime;
            if (wolfpeltTime < 0)
                wolfpeltStack = 0;
        }

        if (itemToolTip.activeSelf)
        {
            if (timerLock)
            {
                toolTipTimer += Time.deltaTime;
                if (toolTipTimer >= 2f)
                    itemToolTip.SetActive(false);
            }

        }
        UseItem();
        ItemDurationTime();
    }
    void UseItem()
    {
        curCoolTime -= Time.deltaTime;
        if (Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.USEITEM])&& curCoolTime <= 0 && curItemName !=null)
        {
            GameManager.instance.useItem = true;
            switch (curItemName)
            {
                case "ForeignFruit":
                    ForeignFruit();
                    break;
                case "OcularHUD":
                    OcularHUD();
                    break;
            }
            curCoolTime = coolTime;
        }
    }
    void ItemDurationTime()
    {
        if (isOcularHUDOn)
        {
            ocularHUDDuration-=Time.deltaTime;
            if (ocularHUDDuration <= 0)
            {
                GameManager.instance.player.playerAttack.criticalChance = originCriticalChance;
                isOcularHUDOn= false;
                ocularHUDDuration = 8f;
            }
        }
    }
    public void ItemMeshOn(string name,Sprite sprite)
    {
        useableItem = false;
        switch (name)
        {
            case "Steak":
                steakLvl++;
                Steak();
                break;
            case "Syringe":
                syringeLvl++;
                Syringe();
                break;
            case "WolfPelt":
                wolfPeltLvl++;
                WolfPelt();
                break;
            case "Seed":
                seedLvl++;
                Seed();
                break;
            case "Glasses":
                glassesLvl++;
                Glasses();
                break;
            case "Coffee":
                coffeeLvl++;
                Coffee();
                break;
            case "DelicateWatch":
                delicateWatchLvl++;
                DelicateWatch();
                break;
            case "ArmorPlate":
                armorPlateLvl++;
                ArmorPlate();
                break;
            case "HealingPotion":
                healingPotionLvl++;
                HealingPotion();
                break;
            case "ShieldGenerator":
                shieldGeneratorLvl++;
                ShieldGenerator();
                break;
            case "TriTip":
                triTipLvl++;
                TriTip();
                break;
            case "MissileLauncher":
                missileLauncherLvl++;
                MissileLauncher();
                break;
            case "ForeignFruit":
                useableItem = true;
                coolTime = fruitCoolTime;
                itemDesc.text = "사용시 최대체력의 절반을 즉시 회복합니다";
                break;
            case "OcularHUD":
                useableItem = true;
                coolTime = ocularHUDCoolTime;
                itemDesc.text = "사용시 8초간 치명타확률이 100퍼센트가 됩니다";
                break;

        }

        if (useableItem)
        {
            useItem.sprite = sprite;
            useItem.color = Color.white;
            curItemName = name;
            GameManager.instance.useItemImage = useItem.sprite;
        }
        else
        {
            if (!itemMesh[name].activeSelf)
            {

                itemMesh[name].SetActive(true); //3D오브젝트 ON
                UIADD(name, sprite); // UI에서 이미지 ON
                if (name == "DelicateWatch" && delicateWatchLvl == 0 && itemBox.transform.Find("DelicateWatchBroken") != null)
                {
                    Destroy(itemBox.transform.Find("DelicateWatchBroken").gameObject);
                }
            }
            else
            {
                TextMeshProUGUI text = itemBox.transform.Find(name).GetChild(0).GetComponent<TextMeshProUGUI>();
                text.text = (int.Parse(text.text) + 1).ToString();
            }
        }


        itemToolTip.SetActive(true);
        itemImage.sprite = sprite;
        itemName.text = name;
        toolTipTimer = 0;
        FinalStats.itemCollected += 1f;
    }
    #region ItemInfo
    void Steak()
    {
        GameManager.instance.player.addHealth = steakHealth * steakLvl;
        GameManager.instance.player.curHealth += steakHealth;
        itemDesc.text = "최대체력을 " + steakHealth + "만큼 올립니다";
    }
    void Syringe()
    {
        syringeAttackRate = 0.12f *syringeLvl;
        itemDesc.text = "공격속도를 " + 0.12f*100 + "% 만큼 올립니다";
    }
    void WolfPelt()
    {
        wolfpeltMaxStack = 1 + 2 * wolfPeltLvl;
        wolfpeltAttackRate = 0.12f;
        itemDesc.text = "치명타 적중시 마다(최대 " + wolfpeltMaxStack + "번 )공격속도를 " + wolfpeltAttackRate*100 + "% 만큼 올립니다";
    }
    public void Seed()
    {
        itemDesc.text = "적중시 현재 체력 " + seedLvl + "흡혈합니다";
    }
    void Glasses()
    {
        GameManager.instance.player.playerAttack.criticalChance = 0.1f * glassesLvl;
        itemDesc.text = "치명타 확률이 " + 0.1f*100 + "% 만큼 상승합니다";
    }
    void Coffee()
    {
        coffeeAttackRate = 0.075f * coffeeLvl;
        coffeeMoveSpeed = 0.075f * coffeeLvl;
        itemDesc.text = "공격속도와 이동속도가 각각" + 0.075f * 100 + "%   만큼 상승합니다";
    }
    void DelicateWatch()
    {
        delicateWatch = 0.2f;
        itemDesc.text = "공격력이" + 0.2f * 100 + "% 만큼 상승합니다. 체력이 25%이하로 떨어질시 효과는 사라집니다";
    }
    void ArmorPlate()
    {
        armorPlateDamageReduce = 5;
        itemDesc.text = "피격시 데미지가" + 5 + " 만큼 감소합니다. (이 수치는 1이하로 떨어지지않습니다)";
    }
    void ShieldGenerator()
    {
        shieldAmont = 0.08f;
        itemDesc.text = "최대체력의 " + 8 + "%만큼 실드가 생깁니다"; 
    }
    void TriTip()
    {
        tritipChance = 0.1f;
        tritipDamage = 2.4f;
        itemDesc.text = "중첩당 " +10 + "% 확률로 " + 240 + "%만큼 출혈데미지를 입힙니다"; //지속시간 3초
    }
    void MissileLauncher()
    {
        missileLauncherChance = 0.8f;
        missileLauncherDamageMultiple = 3f;
        itemDesc.text = "공격시 중첩당" + 10 + "% 확률로" + 300 + "% 데미지의 미사일을 발사합니다";
    }
    void ForeignFruit()
    {
        GameManager.instance.player.curHealth += GameManager.instance.player.maxHealth * 0.5f;
    }
    void OcularHUD()
    {
        originCriticalChance = GameManager.instance.player.playerAttack.criticalChance;
        GameManager.instance.player.playerAttack.criticalChance = 1f;
        isOcularHUDOn = true;
    }
    #endregion
    public void DelicateWatchBroken(float maxHealth, float curHealth)
    {
        if (curHealth <= maxHealth * 0.25f)
        {
            delicateWatchLvl = 0;
            // 3D오브젝트파괴 setactive false
            // UI에서 없애고 부서진이미지로 대체
            // 텍스트로 부셔졋다고 알려주기
            itemMesh["DelicateWatch"].SetActive(false);
            Destroy(itemBox.transform.Find("DelicateWatch").gameObject);

            UIADD("DelicateWatchBroken", delicateWatchBroken);

            itemToolTip.SetActive(true);
            toolTipTimer = 0;
            itemImage.sprite = delicateWatchBroken;
            itemName.text = "DelicateWatchBroken";
            itemDesc.text = "아이템이 부셔졌습니다";
        }
    }
    void HealingPotion()
    {
        itemDesc.text = "최대체력이 25%이하로 떨어지면 모든체력을 회복합니다.";
    }
    public void HealingPotionUse(float maxHealth, float curHealth)
    {
        if(curHealth <= maxHealth * 0.25f)
        {
            healingPotionLvl--;
            itemMesh["HealingPotion"].SetActive(false);
            Destroy(itemBox.transform.Find("HealingPotion").gameObject);

            UIADD("HealingPotionUsed", healingpotionUsed);

            itemToolTip.SetActive(true);
            toolTipTimer = 0;
            itemImage.sprite = healingpotionUsed;
            itemName.text = "HealingPotionUsed";
            itemDesc.text = "아이템을 사용했습니다";
            GameManager.instance.player.curHealth = maxHealth;
        }
    }
    void UIADD(string name, Sprite sprite)
    {

        GameObject gameObject = new GameObject(name);
        gameObject.transform.parent = itemBox.transform;
        gameObject.AddComponent<Image>();
        gameObject.GetComponent<Image>().sprite = sprite;

        gameObject.AddComponent<ItemToolTip>();
        gameObject.GetComponent<ItemToolTip>().description = itemDesc.text;
        gameObject.GetComponent<ItemToolTip>().itemToolTip = itemToolTip;

        gameObject.AddComponent<BoxCollider2D>();
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.transform.localScale = Vector3.one;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        rectTransform.anchoredPosition3D = Vector3.zero;

        if(gameObject.transform.childCount == 0)
        {
            GameObject textGameObject = new GameObject(name + "Lvl");
            textGameObject.transform.parent = gameObject.transform;

            textGameObject.AddComponent<RectTransform>();
            RectTransform textRectTransform = textGameObject.GetComponent<RectTransform>();
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.anchorMin = Vector2.one;
            textRectTransform.localScale = Vector3.one;
            textRectTransform.rotation = new Quaternion(0, 0, 0, 0);
            textRectTransform.anchoredPosition3D = Vector3.zero;
            textRectTransform.sizeDelta = new Vector2(10f,10f);

            textGameObject.AddComponent<TextMeshProUGUI>();
            TextMeshProUGUI text = textGameObject.GetComponent<TextMeshProUGUI>();
            text.fontStyle = FontStyles.Bold;
            text.text = "1";
            text.color = Color.black;
            text.font = fontAsset;
        }

    }



}
