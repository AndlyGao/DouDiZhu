RegisterChange={}
local this = RegisterChange
local Unity = CS.UnityEngine
function this:HotFixRegister()
	xlua.private_accessible(CS.RegistPanel)
	xlua.hotfix(CS.RegistPanel,'RegistClick',
		function (self)
			if(CS.System.String.IsNullOrEmpty(self.inputAccount.text)) then
				self.uiMsg:Set("账号不能为空",Unity.Color.red)
				CS.MsgCenter.Instance:Dispatch(CS.AreaCode.UI,CS.UIEvent.MessageInfoPanel,self.uiMsg)
				return
			end

			
			if(self.inputPassword.text ~= self.inputRepeat.text) then
					self.uiMsg:Set("密码不一致...", Unity.Color.red)
					CS.MsgCenter.Instance:Dispatch(CS.AreaCode.UI,CS.UIEvent.MessageInfoPanel,self.uiMsg)
					return
			end	

			local dto = CS.Protocol.Dto.AccountDto(self.inputAccount.text,self.inputPassword.text)
			self.serverMsg:Set(CS.Protocol.Code.OpCode.ACCOUNT,CS.Protocol.Code.AccountCode.REGISTER_CREQ,dto)
			CS.MsgCenter.Instance:Dispatch(CS.AreaCode.NET,0,self.serverMsg)

		end
	)
end


