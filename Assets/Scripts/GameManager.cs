using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
public static class FinalStats
{
    public static float timeAlivemin;
    public static float timeAlivesec;
    public static float kill;
    public static float allDamage;
    public static float allDamageTaken;
    public static float level;
    public static float goldCollected;
    public static float itemCollected;
    public static string WinLose;
}
public class ItemBoxData
{
    public List<Sprite> images;
    public List<int> itemsCount;
}

public class CheckLogBooks
{
    public bool[] checkLogBooks; //json¿˙¿Â
}
public class UnLocked
{
    public List<int> index;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerMove player;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI goldText;
    public int gold;
    public int neededGold = 25;
    [SerializeField] Slider expSlider;
    public float totalExp;
    [HideInInspector] public float exp;
    [SerializeField] TextMeshProUGUI lvlTxt;
    public int level=1;
    [HideInInspector] public bool isBossDie;
    public TextMeshProUGUI teleporterGaugeTxt;
    bool isOver;

    [SerializeField] Transform itembox;
    public ItemBoxData itemBoxData;
    [SerializeField] GameObject pauseObject;
    public Sprite useItemImage;

    UnLocked unLocked;

    bool isLogBookObjMove;
    bool isdown;
    [SerializeField] RectTransform logBookObj;
    bool escapeSuccess;
    [HideInInspector] public bool useItem;
    CheckLogBooks checkLogBooks;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        lvlTxt.text = "Lv " + level;
        itemBoxData = new ItemBoxData();
        itemBoxData.images = new List<Sprite>();
        itemBoxData.itemsCount = new List<int>();
        AudioManager.instance.BGMPlayer(AudioManager.BGM.GameScene);
        checkLogBooks = new CheckLogBooks();
        checkLogBooks.checkLogBooks = new bool[PoolManager.instance.prefabDatas[5].pools.Length];

        unLocked = new UnLocked();
        unLocked.index = new List<int>();


        string path = Path.Combine(Application.dataPath, "LogBookData.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            checkLogBooks = JsonUtility.FromJson<CheckLogBooks>(jsonData);
        }
    }
    private void Update()
    {
        LogBook();
        CoinGet();
        ExpGet();
        LevelUP();
        ChargedTeleporter();
        GamePause();
        if (isLogBookObjMove)
        {
            if (isdown)
                StartCoroutine(LogBookMoveDown());
            else
                StartCoroutine(LogBookMoveUp());
        }
    }
    void CoinGet()
    {
        goldText.text = gold.ToString();
        
    }
    void ExpGet()
    {
        expSlider.value = exp / totalExp;

    }
    void LevelUP()
    {
        if (exp >= totalExp)
        {
            exp = 0;
            level++;
            totalExp = 100 * Mathf.Pow(1.15f, level);

            player.maxHealth += 10;
            player.curHealth += 10;
            player.curRecoveryHealth += 0.1f;
            player.playerAttack.damage += 2;
            lvlTxt.text = "Lv " + level;
            FinalStats.level = level;
            AudioManager.instance.SFXPlayer(AudioManager.SFX.LevelUp);
        }
    }


    public bool CheckAnimationPlay(Animator anim,string name, float time,bool less)
    {
        if(less) 
            return anim.GetCurrentAnimatorStateInfo(0).IsName(name) &&
                   anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= time;
        else
            return anim.GetCurrentAnimatorStateInfo(0).IsName(name) &&
                   anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= time;
    }
    void ChargedTeleporter()
    {          
        if (((SpawnManager.teleportGauge >= 100 && isBossDie) || (Input.GetKeyDown(KeyCode.O) && Input.GetKeyDown(KeyCode.P))) && !isOver)
        {
            escapeSuccess = true;
            Achievement(escapeSuccess, 3);
            ItemBoxSave("Victory!");
        }
    }
    void GamePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseObject.activeSelf)
            {
                Debug.Log("∞‘¿”æ»∏ÿ√„");
                CameraMovement.CursorOnOff(true);
                pauseObject.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                Debug.Log("∞‘¿”∏ÿ√„");
                CameraMovement.CursorOnOff(false);
                pauseObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
    public void ItemBoxSave(string winLose)
    {

        for (int i = 0; i < itembox.childCount; i++)
        {
            itemBoxData.images.Add(itembox.transform.GetChild(i).GetComponent<Image>().sprite);
            itemBoxData.itemsCount.Add(int.Parse(itembox.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text));
        }
        if (useItemImage != null)
            itemBoxData.images.Add(useItemImage);
        FinalStats.WinLose = winLose;
        string jsonData = JsonUtility.ToJson(itemBoxData);
        string path = Path.Combine(Application.dataPath, "ItemBoxData.json");
        if (File.Exists(path)) File.Delete(path);
        File.WriteAllText(path, jsonData);

        string logBookData = JsonUtility.ToJson(checkLogBooks);
        string logBookPath = Path.Combine(Application.dataPath, "LogBookData.json");
        if (File.Exists(logBookPath)) File.Delete(logBookPath);
        File.WriteAllText(logBookPath, logBookData);

        Debug.Log("unLocked.index.Count : " + unLocked.index.Count);
        Debug.Log("unLocked.index(0) : " + unLocked.index[0]);


        string unlockData = JsonUtility.ToJson(unLocked);
        string unlockPath = Path.Combine(Application.dataPath, "unLockedData.json");
        if (File.Exists(unlockPath)) File.Delete(unlockPath);
        File.WriteAllText(unlockPath, unlockData);

        isBossDie = false;
        GameManager.instance.player.fadeIn.SceneChange();
        isOver = true;
    }
    void LogBook()
    {
        Achievement(FinalStats.kill, 1, 0);
        Achievement(FinalStats.kill, 10, 1);
        Achievement(isBossDie, 2);
        Achievement(escapeSuccess, 3);
        Achievement(FinalStats.itemCollected, 3, 4);
        Achievement(useItem, 5);
        Achievement(player.playerAttack.finalDamage,30, 6);
        Achievement(FinalStats.timeAlivemin,3, 7);
    }
    void Achievement(float request,float requestValue,int index)
    {
        if(request>= requestValue && !checkLogBooks.checkLogBooks[index])
        {
            GameObject logBook = PoolManager.instance.Get(PoolManager.PrefabType.LogBooks, index, logBookObj);

            unLocked.index.Add(index);

            checkLogBooks.checkLogBooks[index] = true;
            isLogBookObjMove = true;
            isdown = true;
            Debug.Log(request + " / " + requestValue + " / " + index);
        }
    }
    void Achievement(bool request, int index)
    {
        if (request && !checkLogBooks.checkLogBooks[index])
        {
            GameObject logBook = PoolManager.instance.Get(PoolManager.PrefabType.LogBooks, index, logBookObj);

            unLocked.index.Add(index);

            checkLogBooks.checkLogBooks[index] = true;
            isLogBookObjMove = true;
            isdown = true;
            Debug.Log(request + " / " + index);
        }
    }
    IEnumerator LogBookMoveDown()
    {
        yield return null;
        Vector3 curPos = logBookObj.anchoredPosition;
        curPos.y -= 97.5f * Time.deltaTime;
        logBookObj.anchoredPosition = curPos;
        yield return new WaitForSeconds(1.5f);
        isdown = false;
    }
    IEnumerator LogBookMoveUp()
    {
        yield return new WaitForSeconds(1f);
        Vector3 curPos = logBookObj.anchoredPosition;
        curPos.y += 97.5f * Time.deltaTime;
        logBookObj.anchoredPosition = curPos;
        yield return new WaitForSeconds(1.5f);
        curPos.y = 130f;
        logBookObj.anchoredPosition = curPos;
        isLogBookObjMove = false;
        if(logBookObj.childCount>0)
            Destroy(logBookObj.GetChild(0).gameObject);

    }
}
