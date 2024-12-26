using System.Collections.Generic;
using Game.Gameplay;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Client
{

    //PC使用的输入处理类
    public class PCGameHandle : GameHandle
    {
        public static Dictionary<KeyCode, PlayerPCInputInfo> m_InputKeyList = new Dictionary<KeyCode, PlayerPCInputInfo>();

        public override void OnInit(IInputUtility inputUtility)
        {
            m_InputKeyList.Clear();
        }

        //真正执行输入运算
        public void ExecuteInput(Dictionary<KeyCode, PlayerPCInput> playerInputDic, float deltaTime)
        {
            ResetAllInputButtonState();
            
            foreach (var keyCode in playerInputDic.Keys)
            {
                //如果字典里没有 则添加
                PlayerPCInput input = playerInputDic[keyCode];
                m_InputKeyList.TryAdd(input.KeyCode, new PlayerPCInputInfo());
                
                UpdatePlayerPCInputInfo(input,m_InputKeyList[input.KeyCode]);
            }
            
            CalInputState(deltaTime);
        }
        
        
        private void ResetAllInputButtonState()
        {
            foreach (PlayerPCInputInfo input in m_InputKeyList.Values)
            {
                input.bLastState = input.bState;
                input.bState = false;
                input.bIsDown = false;
                input.bIsUp = false;
            }
        }


        private void UpdatePlayerPCInputInfo(PlayerPCInput input,PlayerPCInputInfo info)
        {
            //如果在执行之前存在抬起的输入
            if (input.ExistKeyUpBeforeExecute())
            {
                info.bLastState = false;
            }
            info.bState = true;
        }

        private void CalInputState(float deltaTime)
        {
            foreach (var key in m_InputKeyList.Keys)
            {
                var input = m_InputKeyList[key];
                input.bIsDown = (!input.bLastState && input.bState);
                input.bIsUp = (input.bLastState && !input.bState);
            }
        }

        public PlayerPCInputInfo GetPlayerPCInputInfo(KeyCode keyCode)
        {
            if (!m_InputKeyList.ContainsKey(keyCode))
            {
                m_InputKeyList.TryAdd(keyCode, new PlayerPCInputInfo());
            }
            
            return m_InputKeyList[keyCode];
        }
    }

}
