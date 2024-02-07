using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    TextMeshProUGUI stageTxt;
    Image stageImage;
    TextMeshProUGUI stageTime;
    TextMeshProUGUI stageMSec;
    [Header("StageSprite")]
    [SerializeField] Sprite easy;
    [SerializeField] Sprite normal;
    [SerializeField] Sprite hard;
    [Header("StageFlow")]
    [SerializeField] GameObject stageBar;
    RectTransform flowManger;

    //easy : 0.5(300초) //normal : 0.625(240초) // hard : 0.8333(180초)
    const float easyLvl = 0.5f;
    const float normalLvl = 0.625f;
    const float hardLvl = 0.8333f;

    public static float speed;
    [HideInInspector] public int min;
    int second;
    float mSec;

    void Awake()
    {
        stageTxt = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        stageImage = transform.GetChild(1).GetComponent<Image>();
        stageTime = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        stageMSec = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        flowManger = stageBar.transform.GetChild(0).GetComponent<RectTransform>();
    }
    private void Start()
    {
        Debug.Log(speed);
        switch (speed)
        {
            case easyLvl:
                stageImage.sprite = easy;
                break;
            case normalLvl:
                stageImage.sprite = normal;
                break;
            case hardLvl:
                stageImage.sprite = hard;
                break;

        }
    }
    void Update()
    {
        TimeFlow();
    }
    private void TimeFlow()
    {
        mSec += Time.deltaTime * 100;
        if (mSec >= 100)
        {
            mSec = 0;
            second++;
        }
        if (second >= 60)
        {
            second = 0;
            min++;
        }
        stageTxt.text = "stage\n1";
        stageTime.text = string.Format("{0:D2}:{1:D2}", min, second);
        stageMSec.text = string.Format("{0:D2}", (int)(mSec));
        FinalStats.timeAlivemin = min;
        FinalStats.timeAlivesec = second;
        Vector3 curPos = flowManger.anchoredPosition;
        curPos.x -= speed * Time.deltaTime;
        flowManger.anchoredPosition = curPos;
    }
    public void LevelManager(float maxhealth, float damage)
    {
        //해당난이도에맞는 시간마다
        //체력 20 ,공격력증가5
        switch(speed)
        {
            case easyLvl:
                if(min != 0 && min % 2 == 0)
                {
                    EnemyStatusUp(maxhealth, damage);
                }
                break;
            case normalLvl:
                if (min != 0 && min % 4 == 0)
                {
                    EnemyStatusUp(maxhealth, damage);
                }
                break;
            case hardLvl:
                if (min != 0 && min % 3 == 0)
                {
                    EnemyStatusUp(maxhealth, damage);
                }
                break;

        }
    }
    void EnemyStatusUp(float maxhealth,float damage)
    {
        maxhealth += 20;
        damage += 5;
    }

}

