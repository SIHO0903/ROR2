using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    Image image;
    public string description;
    public GameObject itemToolTip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ÅÇ´©¸¥ÈÄ ÅøÆÁº¸±â");
        image = GetComponent<Image>();
        itemToolTip.transform.GetChild(0).GetComponent<Image>().sprite = image.sprite;
        itemToolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameObject.name;
        itemToolTip.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = description;
        Item.timerLock = false;
        itemToolTip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image = GetComponent<Image>();
        itemToolTip.transform.GetChild(0).GetComponent<Image>().sprite = image.sprite;
        itemToolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameObject.name;
        itemToolTip.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = description;
        Item.timerLock = true;
        itemToolTip.SetActive(false);
    }
}
