using System.Collections;
using System.Collections.Generic;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : UIBase {

    private Text nameTxt;
    private Text lvTxt;
    private Slider expSlider;
    private Text expTxt;
    private Text beensTxt;

    private void Awake()
    {
        Bind(UIEvent.INIT_USERPANEL_INFO);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.INIT_USERPANEL_INFO:
                UserDto userDto = message as UserDto;
                 InitPanel(userDto.name,userDto.lv,userDto.beens);
                break;
            default:
                break;
        }
    }



    void Start () {
        nameTxt = transform.Find("nameTxt").GetComponent<Text>();
        lvTxt = transform.Find("lvTxt").GetComponent<Text>();
        expSlider = transform.Find("expSlider").GetComponent<Slider>();
        expTxt = transform.Find("expTxt").GetComponent<Text>();
        beensTxt = transform.Find("beensTxt").GetComponent<Text>();

        //SetPanelActive(false);
    }

    /// <summary>
    /// 初始化角色信息
    /// </summary>
    public void InitPanel(string name,int lv,int beens) {
        nameTxt.text = name;
        lvTxt.text = "LV : " + lv;
        //当前等级的总经验 ： 当前等级 * 100
        expTxt.text = lv + "/" + (lv+1) * 100;
        //滑动条
        expSlider.value = (float) lv/ (lv + 1) * 100;

        beensTxt.text = "x" + beens;

    }
	
	

}
