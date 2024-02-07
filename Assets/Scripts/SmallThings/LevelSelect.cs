using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject LevelSelectObj;
    [SerializeField] GameObject desc;
    TextMeshProUGUI descTxt;
    private void Awake()
    {
        descTxt = desc.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        desc.SetActive(true);

        switch (gameObject.name)
        {
            case "Easy":
                Stage.speed = 0.5f;
                descTxt.text = "난이도 상승속도 : <color=yellow>-50%</color>";
                break;
            case "Normal":
                Stage.speed = 0.625f;
                descTxt.text = "중간난이도";
                break;
            case "Hard":
                Stage.speed = 0.8333f;
                descTxt.text = "난이도 상승속도 : <color=yellow>100%</color>";
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        desc.SetActive(false);
    }
    public void SetActiveTranparent()
    {
        LevelSelectObj.transform.localScale = Vector3.zero;
    }
}
