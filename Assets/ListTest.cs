using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTest : MonoBehaviour
{
    private List<int> intList = new List<int>();
    private Dictionary<int, int> intDic = new Dictionary<int, int>();
    // Start is called before the first frame update
    void Start()
    {
        //intList.Add(0);
        //intList.Add(1);
        //intList.Remove(0);
        //intList.Add(0);
        //foreach (var item in intList)
        //{
        //    Debug.Log(item);
        //}
        intDic.Add(0,0);
        intDic.Add(1, 1);
        intDic.Remove(0);
        intDic.Add(0,0);
        foreach (var item in intDic.Keys)
        {
            Debug.Log(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
