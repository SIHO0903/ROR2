using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum KeyAction
{
    UP, DOWN, LEFT, RIGHT, JUMP,RUN, 
    ATTACK, SKILL1, SKILL2, DASH, USEITEM, INVENTORY
}
public class KeySetting
{
    public List<KeyAction> keyActions;
    public List<KeyCode>   keyCodes;
}
public class KeySoundSetManager : MonoBehaviour
{

    int key;
    public static KeySoundSetManager instance;
    public Dictionary<KeyAction, KeyCode> keyValues = new Dictionary<KeyAction, KeyCode>();
    public KeyCode[] defaultKeys = new KeyCode[]
        {
            KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D,
        KeyCode.Space,
        KeyCode.LeftControl,
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.R,
        KeyCode.LeftShift,
        KeyCode.Q,
        KeyCode.Tab,
    };
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
        KeySetting keySetting = new KeySetting();

        for (int i = 0; i < defaultKeys.Length; i++)
        {
            keyValues.Add((KeyAction)i, defaultKeys[i]);
        }
        gameObject.SetActive(false);
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            keyValues[(KeyAction)key] = keyEvent.keyCode;
            key = -1;
        }
    }
    public void ChangeKey(int num)
    {
        key = num;
    }
    public void ResetKey()
    {
        for (int i = 0; i < defaultKeys.Length; i++)
        {
            keyValues[(KeyAction)i] = defaultKeys[i];
        }
        AudioManager.instance.AudioReSet();
    }
}
