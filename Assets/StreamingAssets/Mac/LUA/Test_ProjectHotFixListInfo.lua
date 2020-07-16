---
---  “年会倒计时” UI显示。
---   热补丁更改lua脚本
---
--- Created by Liu_Guozhu.
--- DateTime: 2018/12/11
---

Test_ProjectHotFixListInfo={}
local this=Test_ProjectHotFixListInfo

--更新本项目的脚本方法
function this:HotFixScriptsMethod()
    xlua.private_accessible(CS.HotUpdateModel.UIMgr)  --可以访问私有字段
    xlua.hotfix(CS.HotUpdateModel.UIMgr,'Start',
    function(self)
        self._CountDownNum=10
    end
    )

    xlua.private_accessible(CS.HotUpdateModel.UIMgr)  --可以访问私有字段
    xlua.hotfix(CS.HotUpdateModel.UIMgr,'Update',
    function(self)
        if(CS.UnityEngine.Time.frameCount%60==0) then
            self._CountDownNum=self._CountDownNum-1
            self.TxtNumber.text=tostring(self._CountDownNum)
        end
    end
    )
end