using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class GameResult : MonoBehaviour
{
    //Text¹­À½
    [SerializeField] GameObject statsDesc;
    [SerializeField] GameObject statsDescPts;
    [SerializeField] TextMeshProUGUI WinLoseTxt;
    //µñ¼Å³Ê¸®·Î Á¤¸®
    enum StatName
    {
        TimeAlive,
        Kill,
        Damage,
        DamageTaken,
        Level,
        GoldCollected,
        ItemCollected,
        Total
    }
    Dictionary<StatName, TextMeshProUGUI> stats;
    Dictionary<StatName, TextMeshProUGUI> statsPts;
    float[] pts;

    ItemBoxData itemBoxData;

    [SerializeField] GameObject itemBox;
    [SerializeField] TMP_FontAsset fontAsset;

    UnLocked unLocked;
    [SerializeField] Transform unLockedTransform;
 
    [SerializeField] Image fadein;
    Color color;

    [SerializeField] Image stageLvlImg;
    [SerializeField] Sprite easyImage;
    [SerializeField] Sprite normalImage;
    [SerializeField] Sprite hardImage;


    private void Start()
    {
        switch (Stage.speed)
        {
            case 0.5f:
                stageLvlImg.sprite = easyImage;
                break;
            case 0.625f:
                stageLvlImg.sprite = normalImage;
                break;
            case 0.8333f:
                stageLvlImg.sprite = hardImage;
                break;

        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StatsUI();
        itemBoxData = new ItemBoxData();
        itemBoxData.images = new List<Sprite>();
        itemBoxData.itemsCount = new List<int>();
        string path = Path.Combine(Application.dataPath, "ItemBoxData.json");
        string jsonData = File.ReadAllText(path);
        itemBoxData = JsonUtility.FromJson<ItemBoxData>(jsonData);
        Debug.Log(jsonData);
        Debug.Log(itemBoxData.itemsCount.Count);

        AudioManager.instance.BGMPlayer(AudioManager.BGM.ResultScene);
        AudioManager.instance.BGMPlayerBossDie();


        if (itemBoxData.images.Count != 0)
        {
            for (int i = 0; i < itemBoxData.images.Count; i++)
            {
                CreateGameObject(i);
            }
        }

        unLocked = new UnLocked();
        unLocked.index = new List<int>();
        string unLockPath = Path.Combine(Application.dataPath, "unLockedData.json");
        string unlockPath = File.ReadAllText(unLockPath);
        unLocked = JsonUtility.FromJson<UnLocked>(unlockPath);


        for (int i = 0; i < unLocked.index.Count; i++)
        {
            unLockedTransform.GetChild(unLocked.index[i]).gameObject.SetActive(true);
        }

        if (File.Exists(unlockPath)) File.Delete(unlockPath);

    }
    
    private void StatsUI()
    {
        WinLoseTxt.text = FinalStats.WinLose;
        stats = new Dictionary<StatName, TextMeshProUGUI>();
        statsPts = new Dictionary<StatName, TextMeshProUGUI>();
        pts = new float[statsDescPts.transform.childCount];
        for (int i = 0; i < statsDesc.transform.childCount; i++)
        {
            stats.Add((StatName)i, statsDesc.transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        }
        for (int i = 0; i < statsDescPts.transform.childCount; i++)
        {
            statsPts.Add((StatName)i, statsDescPts.transform.GetChild(i).GetComponent<TextMeshProUGUI>());
        }

        stats[StatName.TimeAlive].text = "TimeAlive : <color=yellow>" + FinalStats.timeAlivemin + ":" + FinalStats.timeAlivesec + "</color>";
        stats[StatName.Kill].text = "Kill : <color=yellow>" + FinalStats.kill.ToString() + "</color>";
        stats[StatName.Damage].text = "Damage : <color=yellow>" + FinalStats.allDamage.ToString() + "</color>";
        stats[StatName.DamageTaken].text = "DamageTaken : <color=yellow>" + FinalStats.allDamageTaken.ToString() + "</color>";
        stats[StatName.Level].text = "Level : <color=yellow>" + FinalStats.level.ToString() + "</color>";
        stats[StatName.GoldCollected].text = "GoldCollected : <color=yellow>" + FinalStats.goldCollected.ToString() + "</color>";
        stats[StatName.ItemCollected].text = "ItemCollected : <color=yellow>" + FinalStats.itemCollected.ToString() + "</color>";

        pts[(int)StatName.TimeAlive] = FinalStats.timeAlivemin * 60f + FinalStats.timeAlivesec;
        pts[(int)StatName.Kill] = FinalStats.kill * 3f;
        pts[(int)StatName.Damage] = FinalStats.allDamage / 10f;
        pts[(int)StatName.DamageTaken] = FinalStats.allDamageTaken / 10f;
        pts[(int)StatName.Level] = FinalStats.level * 100f;
        pts[(int)StatName.GoldCollected] = FinalStats.goldCollected * 5f;
        pts[(int)StatName.ItemCollected] = FinalStats.itemCollected * 100f;
        for (int i = 0; i < pts.Length - 1; i++)
        {
            pts[pts.Length - 1] += pts[i];
        }
        for (int i = 0; i < statsPts.Count; i++)
        {
            statsPts[(StatName)i].text = "<color=yellow>" + pts[i] + "</color>pts.";
        }
    }


    void CreateGameObject(int i)
    {
        GameObject gameObject = new GameObject(itemBoxData.images[i].name);
        gameObject.transform.parent = itemBox.transform;
        gameObject.AddComponent<Image>();
        gameObject.GetComponent<Image>().sprite = itemBoxData.images[i];
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        rectTransform.anchoredPosition3D = Vector3.zero;

        GameObject textGameObject = new GameObject(name + "Lvl");
        textGameObject.transform.parent = gameObject.transform;

        textGameObject.AddComponent<RectTransform>();
        RectTransform textRectTransform = textGameObject.GetComponent<RectTransform>();
        textRectTransform.anchorMax = Vector2.one;
        textRectTransform.anchorMin = Vector2.one;
        textRectTransform.localScale = Vector3.one;
        textRectTransform.rotation = new Quaternion(0, 0, 0, 0);
        textRectTransform.anchoredPosition3D = Vector3.zero;
        textRectTransform.sizeDelta = new Vector2(10f, 10f);

        textGameObject.AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI text = textGameObject.GetComponent<TextMeshProUGUI>();
        text.fontStyle = FontStyles.Bold;
        text.text = "1";
        text.color = Color.white;
        text.font = fontAsset;

    }
    private void OnEnable()
    {
        color = Color.black;
        color.a = 0f;
        fadein.color = color;
    }
}
