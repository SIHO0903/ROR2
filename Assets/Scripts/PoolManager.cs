using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//������Ʈ Ǯ��
public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    public enum PrefabType {Player, Enemy, EnemyBullet,Environment,Items,LogBooks }
    [System.Serializable]
    public struct PrefabPoolData
    {
        public string name;
        public GameObject[] prefabs;
        public List<GameObject>[] pools;
    } 
    public List<PrefabPoolData> prefabDatas;
    private void Awake()
    {
        instance = this;
        for (int dataIdx = 0; dataIdx < prefabDatas.Count; dataIdx++)
        {
            
            PrefabPoolData poolData = prefabDatas[dataIdx];
            poolData.pools = new List<GameObject>[poolData.prefabs.Length];
            for (int i = 0; i < poolData.pools.Length; i++)
            {
                poolData.pools[i] = new List<GameObject>();
            }
            prefabDatas[dataIdx] = poolData; 
        }
    }
    public GameObject Get(PrefabType prefabTypes, int index, Transform transform=null)
    {   
        GameObject select = null;

        if (transform == null)
            transform = this.transform;

        //index�ش��ϴ� ��Ȱ��ȭ�� ������Ʈ Ȱ��ȭ�� ��ȯ
        foreach (GameObject item in prefabDatas[(int)prefabTypes].pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // �������� ���� Ȱ��ȭ�����Ͻ� ���� ������ ����Ʈ�� �߰�
        if (!select)
        {
            select = Instantiate(prefabDatas[(int)prefabTypes].prefabs[index], transform);
            int indexSub = select.name.IndexOf("(Clone)");
            if (indexSub > 0)
                select.name = select.name.Substring(0, indexSub);
            prefabDatas[(int)prefabTypes].pools[index].Add(select);
        }

        return select;
    }

}
