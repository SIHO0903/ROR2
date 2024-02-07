using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LogBookProgress : MonoBehaviour
{
    CheckLogBooks checkLogBooks;
    int arraylength = 8;
    [SerializeField] Image[] unLock;
    Color colorA1;
    Color colorA0;

    void Awake()
    {
        colorA1 = Color.black;
        colorA0 = Color.black;

        colorA1.a = 1f;
        colorA0.a = 0f;
        checkLogBooks = new CheckLogBooks();
        checkLogBooks.checkLogBooks = new bool[arraylength];
        string path = Path.Combine(Application.dataPath, "LogBookData.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            checkLogBooks = JsonUtility.FromJson<CheckLogBooks>(jsonData);
        }
    }
    private void OnEnable()
    {
        for (int i = 0; i < checkLogBooks.checkLogBooks.Length; i++)
        {
            UnLockCheck(i);
        }
    }
    void UnLockCheck(int index)
    {
        if (checkLogBooks.checkLogBooks[index])
            unLock[index].color = colorA0;
        else
            unLock[index].color = colorA1;
    }
}
