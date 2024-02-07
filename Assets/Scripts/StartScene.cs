using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tempTxt;
    float timer = 0;
    private void Update()
    {
        if(timer < 1f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene("MainmenuScene");
        }
        tempTxt.text = string.Format("{0:F1}", timer);
    }
}
