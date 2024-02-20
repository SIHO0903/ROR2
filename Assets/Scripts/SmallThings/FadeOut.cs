using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    [SerializeField] string scene;
    Color color;
    float timer;
    [SerializeField] float endTime = 0.5f;
    bool fadeOut;
    private void OnEnable()
    {
        fadeOut = false;
        timer = 0f;
        color = Color.black;
        color.a = 0f;
        fadeImage.color = color;
    }
    public void SceneChange()
    {
        fadeOut = true;
    }
    IEnumerator FadeInCoroutine()
    {
        if (timer < endTime)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / endTime);
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
        if (fadeOut)
            StartCoroutine(FadeInCoroutine());
    }
}
