using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioStart : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.BGMPlayer(AudioManager.BGM.MainMenuScene);
    }
}
