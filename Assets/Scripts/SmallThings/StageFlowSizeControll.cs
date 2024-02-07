using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class StageFlowSizeControll : MonoBehaviour
{
    public Vector3 initialPosition = new Vector3(125f, -50f, 0f);
    public Vector2 size = new Vector2(180f, 48f);

    RectTransform[] stageFlows;
    void Start()
    {
        stageFlows = new RectTransform[transform.transform.childCount];
        for (int i = 0; i < transform.transform.childCount; i++)
        {
            stageFlows[i] = transform.GetChild(i).GetComponent<RectTransform>();
            stageFlows[i].anchoredPosition = new Vector2(initialPosition.x + i * size.x, initialPosition.y);
            stageFlows[i].sizeDelta = size;
        }

    }


}
