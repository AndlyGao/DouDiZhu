

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using UnityEngine.UI;


namespace HotUpdateModel
{
    [Hotfix]
    public class UIMgr:MonoBehaviour
    {
        public Text TxtNumber;              //显示数字控件
        private int _CountDownNum = 0;      //倒计时数字


        private void Start()
        {
            _CountDownNum = 0;
        }

        private void Update()
        {
            if (Time.frameCount%60==0)
            {
                ++_CountDownNum;
                TxtNumber.text = _CountDownNum.ToString();
            }
        }


    }//Class_end
}


