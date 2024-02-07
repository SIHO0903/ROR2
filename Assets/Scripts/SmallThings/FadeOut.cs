using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    Image fadeOut;
    Color color;
    float timer;
    [SerializeField] float limit = 0.5f;
    private void Awake()
    {
        fadeOut = GetComponent<Image>();
    }
    private void OnEnable()
    {
        color = Color.black;
        color.a = 1f;
        fadeOut.color = color;
        timer = 0;
    }
    private void Update()
    {
        if(timer < limit)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / limit);
            fadeOut.color = color;
        }
    }
}
