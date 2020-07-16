---
---  启动ABFW 框架。
---
---  通过调用ABFW 核心API ，实现lua中加载unity 中的资源（预设）
---
--- Created by Liu_Guozhu.
--- DateTime: 2018/12/3
---

--调用UI预设核心参数
local string scensName="mainscenes"  --场景名称
local string pacageName="mainscenes/ui.ab" --ab包名称
local string assetName="UI_Notice.prefab" --资源名称

LauchABFW={}
local this=LauchABFW

--引用“异步转同步”，加载ABFW框架功能
local yie_return=require('CorLaunchABFW').yield_return

--lua的协同
local co=coroutine.create(
    function()
        yie_return(scensName,pacageName,assetName)
      end
)

--启动lua协同
function LauchABFW:StartABFW()
    --启动lua的协同
    assert(coroutine.resume((co)))
end





