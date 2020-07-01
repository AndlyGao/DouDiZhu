using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ui自适应脚本  大于16:9的以高适配/小于16:9的以宽适配
/// </summary>
public class UIAdaptation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CanvasScaler cs = GetComponent<CanvasScaler>();
        if (((float)Screen.width / Screen.height) >= (1920f / 1080))
        {
            cs.matchWidthOrHeight = 1;
        }
        else
        {
            cs.matchWidthOrHeight = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
