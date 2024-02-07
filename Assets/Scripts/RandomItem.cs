using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomItem : MonoBehaviour, IInteractable
{
    TextMeshProUGUI goldText;
    public float timer;
    private void Awake()
    {
        goldText = GetComponentInChildren<TextMeshProUGUI>(true);
    }
    public void Interact(int gold)
    {
        goldText.gameObject.SetActive(true);
        goldText.text = gold + "$";
    }
    public void SetActiveFalse()
    {
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (goldText.gameObject.activeSelf && timer >= 0.2f)
        {
            timer = 0;
            goldText.gameObject.SetActive(false);
        }

    }

}
