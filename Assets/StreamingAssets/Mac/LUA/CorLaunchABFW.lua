---
--- Lua 脚本启动ABFW框架，加载指定包中的资源
---
--- Created by Liu_Guozhu.
--- DateTime: 2018/12/3
---

--定义局部变量（用于本脚本两个函数中间数据传递）
local string loc_ScenesName=nil    --场景名称
local string Loc_PackageName=nil   --AB包名称
local string Loc_AssetName=nil     --资源名称

--导入xlua包
local util=require("xlua.util")  --异步转同步函数

--实例化ABFW框架中核心管理类
local abMgrClass=CS.ABFW.AssetBundleMgr
local abMgrObj=abMgrClass.GetInstance()


--加载AB包（通过调用ABFW框架）
-- 函数参数含义：
--     ScenesName  场景名称
--     packageName  AB包名称
--     assetName  资源名称
local function LoadABPackage(ScenesName,packageName,assetName)
    loc_ScenesName=ScenesName
    Loc_PackageName=packageName
    Loc_AssetName=assetName

    --调用AB包
    abMgrObj:LoadAssetBundlePackage(ScenesName,packageName,FinishWork)
end



--(回调方法) 加载指定AB包中内部的资源
function FinishWork()
    --资源对象
    local assetObj=nil

    --调用AB包中的资源
    assetObj=abMgrObj:LoadAsset(loc_ScenesName,Loc_PackageName,Loc_AssetName,false)
    if assetObj then
        CS.UnityEngine.Object.Instantiate(assetObj)
        print("CorLaunchABFW.lua/(回调函数) /FinishWork()/加载指定包资源成功！")
    else
        print("### 发生错误： CorLaunchABFW.lua/(回调函数) /FinishWork()/ 加载对象为nil，请检查！")
    end
end

return{
    yield_return=util.async_to_sync(LoadABPackage)
}

