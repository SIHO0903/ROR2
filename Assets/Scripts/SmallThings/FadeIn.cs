using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] string scene;
    Color color;
    float timer;
    bool fadein;
    private void OnEnable()
    {
        fadein = false;
        timer = 0f;
        color = Color.black;
        color.a = 0f;
        fadeImage.color = color;
    }
    public void SceneChange()
    {
        fadein = true;
    }
    IEnumerator FadeInCoroutine()
    {
        if (timer < 0.5f)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / 0.5f);
            fadeImage.color = color;
            yield return null;
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }
    private void Update()
    {
        if (fadein)
            StartCoroutine(FadeInCoroutine());

    }
}
