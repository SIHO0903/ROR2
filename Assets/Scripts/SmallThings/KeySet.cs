using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeySet : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] txt;
    Dictionary<KeyAction,KeyCode> keys;
    void Start()
    {
        keys = KeySoundSetManager.instance.keyValues;
        for (int i = 0; i < txt.Length; i++)
        {
            txt[i].text = keys[(KeyAction)i].ToString();
        }
    }

    void Update()
    {
        for (int i = 0; i < txt.Length; i++)
        {
            txt[i].text = keys[(KeyAction)i].ToString();
        }

    }
}
