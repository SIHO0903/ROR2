using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    Image fadeIn;
    Color color;
    float timer;
    [SerializeField] float limit = 0.5f;
    private void Awake()
    {
        fadeIn = GetComponent<Image>();
    }
    private void OnEnable()
    {
        color = Color.black;
        color.a = 1f;
        fadeIn.color = color;
        timer = 0;
    }
    private void Update()
    {
        if(timer < limit)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / limit);
            fadeIn.color = color;
        }
    }
}
