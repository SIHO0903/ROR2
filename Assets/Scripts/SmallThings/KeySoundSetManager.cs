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
    public static KeySoundSetManager instance; 
    
    int key;
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
        //싱글톤
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        //초기키설정값 세팅
        for (int i = 0; i < defaultKeys.Length; i++)
        {
            keyValues.Add((KeyAction)i, defaultKeys[i]);
        }
        gameObject.SetActive(false);

    }

    private void OnGUI()
    {
        // 입력받은 키값을 keyValues에 넣은후 key초기화
        Event keyEvent = Event.current;
        if (keyEvent.isKey)
        {
            keyValues[(KeyAction)key] = keyEvent.keyCode;
            key = -1;
        }
    }

    // 버튼 마다 고유키값을 받아옴
    // 키세팅버튼에서 OnClick()으로 호출
    public void ChangeKey(int num)
    {
        key = num;
    }

    // 초기키세팅값으로 리셋
    // 초기화버튼에서 OnClick()으로 호출
    public void ResetKey()
    {
        for (int i = 0; i < defaultKeys.Length; i++)
        {
            keyValues[(KeyAction)i] = defaultKeys[i];
        }
        AudioManager.instance.AudioReSet();
    }
}
