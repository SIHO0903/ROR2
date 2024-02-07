using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillUINameTxt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] txt;
    string dash;
    private void OnEnable()
    {
        if (KeySoundSetManager.instance.keyValues[KeyAction.RUN].ToString().Contains("Control"))
            dash = "Ctrl";
        else if (KeySoundSetManager.instance.keyValues[KeyAction.RUN].ToString().Contains("Shift"))
            dash = "Shift";
        else
            dash = KeySoundSetManager.instance.keyValues[KeyAction.RUN].ToString();

        txt[0].text = KeySoundSetManager.instance.keyValues[KeyAction.INVENTORY].ToString();
        txt[1].text = dash;
        txt[2].text = KeySoundSetManager.instance.keyValues[KeyAction.ATTACK].ToString();
        txt[3].text = KeySoundSetManager.instance.keyValues[KeyAction.SKILL1].ToString();
        txt[4].text = KeySoundSetManager.instance.keyValues[KeyAction.DASH].ToString();
        txt[5].text = KeySoundSetManager.instance.keyValues[KeyAction.SKILL2].ToString();
        txt[6].text = KeySoundSetManager.instance.keyValues[KeyAction.USEITEM].ToString();
    }
}
