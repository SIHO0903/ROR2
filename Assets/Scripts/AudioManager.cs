using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AudioManager;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    TextMeshProUGUI bgmTxt;
    TextMeshProUGUI sfxTxt;


    [SerializeField] AudioClip[] bgmClip;
    [SerializeField] AudioClip[] sfxClip;

    public enum BGM
    {
        Boss, ResultScene, GameScene, MainMenuScene
    }
    public enum SFX 
    { 
        BtnEnter,BtnClick, EnemyDead, EnemyHit, EnemyThrow, GetExp, GolemMove,
        Laser,LevelUp, SlimeMove,Volcano
    };


    AudioSource[] bgmSource;
    AudioSource[] audioSources;



    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        bgmSource = GetComponents<AudioSource>();
        audioSources = transform.GetChild(0).GetComponents<AudioSource>();
        bgmTxt = bgmSlider.GetComponentInChildren<TextMeshProUGUI>(true);
        sfxTxt = sfxSlider.GetComponentInChildren<TextMeshProUGUI>(true);

    }

    private void Update()
    {
        VolumSetting(bgmSlider.value, sfxSlider.value);

    }

    private void VolumSetting(float bgm, float sfx)
    {
        if(bgmSlider.gameObject.activeSelf)
        {
            bgmSource[0].volume = bgm;
            bgmSource[1].volume = bgm;

            for (int i = 1; i < audioSources.Length; i++)
            {
                audioSources[i].volume = sfx;
            }
            bgmTxt.text = string.Format("{0:F0}", bgmSlider.value * 100f);
            sfxTxt.text = string.Format("{0:F0}", sfxSlider.value * 100f);
        }
    }

    public void BGMPlayer(BGM bgm)
    {
        bgmSource[0].clip = bgmClip[(int)bgm];
        bgmSource[0].Play();

    }

    public void BGMPlayerBossSpawn(BGM bgm)
    {
        bgmSource[0].Pause();
        bgmSource[1].clip = bgmClip[(int)bgm];
        bgmSource[1].Play();
    }
    public void BGMPlayerBossDie()
    {
        bgmSource[1].Pause();
        bgmSource[0].Play();
    }
    public void SFXPlayer(SFX sfx)
    {
        for (int i = 1; i < audioSources.Length; i++)
        {
            if (audioSources[i].isPlaying)
                continue;
            else
            {
                audioSources[i].clip = sfxClip[(int)sfx];
                audioSources[i].Play();

                return;
            }
        }
    }
    public void AudioReSet()
    {
        bgmSlider.value = 0.5f;
        sfxSlider.value = 0.5f;

    }
}
