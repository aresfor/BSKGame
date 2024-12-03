using System.Collections;
using System.Collections.Generic;
using Game.Gameplay;
using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global

//处理大部分共有输入逻辑
namespace Game.Client
{
    public class GameHandle 
    {
        //保存hud ui点击响应状态
        protected static List<HudClickState> m_HudClickStateList = new List<HudClickState>();
        
        //初始化
        public void Init(IInputUtility inputUtility)
        {
            InitHudButtonState();
            
            OnInit(inputUtility);
        }
        
        //初始化HUD状态记录列表 各个平台都通用
        public void InitHudButtonState()
        {
            m_HudClickStateList.Clear();
            for (int i = 0; i < (int) EHudBtnIndex.HudButtonNum; i++)
            {
                m_HudClickStateList.Add(new HudClickState());
            }
        }
        
        /// <summary>
        /// 由InputUtility调用，跟随逻辑帧进行Execute
        /// </summary>
        public virtual void OnExecute(float deltaTime)
        {
            CalcHudButtonState();
        }
        
        public virtual void OnInit(IInputUtility inputUtility)
        {
            
        }

        #region HudButton相关判定函数

        public static void SetHudButtonClickState(EHudBtnIndex index, bool isClick)
        {
            m_HudClickStateList[(int) index].isClick = isClick;
        }
        
        public static void SetHudButtonPressState(EHudBtnIndex index, bool isPress)
        {
            m_HudClickStateList[(int) index].isPress = isPress;
        }

        public static void SetHudButtonDown(EHudBtnIndex index, bool isDown)
        {
            m_HudClickStateList[(int)index].isDown = isDown;
        }

        public static void SetHudButtonUp(EHudBtnIndex index, bool isUp)
        {
            m_HudClickStateList[(int)index].isUp = isUp;
        }

        public static bool GetHudButtonPressState(EHudBtnIndex index)
        {
            if ((int)index > m_HudClickStateList.Count - 1) return false;
            return m_HudClickStateList[(int)index].isPress;
        }

        public static bool GetHudButtonClick(EHudBtnIndex index)
        {
            if ((int) index > m_HudClickStateList.Count - 1) return false;
            return m_HudClickStateList[(int) index].isClick;
        }

        public static bool GetHudButtonState(EHudBtnIndex index)
        {
            if ((int)index > m_HudClickStateList.Count - 1) return false;
            return m_HudClickStateList[(int)index].state;
        }

        public static bool GetIsButtonDown(EHudBtnIndex index)
        {
            if ((int)index > m_HudClickStateList.Count - 1) return false;
            return m_HudClickStateList[(int)index].isDown;
        }

        public static bool GetIsButtonUp(EHudBtnIndex index)
        {
            if ((int)index > m_HudClickStateList.Count - 1) return false;
            return m_HudClickStateList[(int)index].isUp;
        }

        public void CalcHudButtonState()
        {
            for (int i = 0; i < m_HudClickStateList.Count; i++)
            {
                if (m_HudClickStateList[i].lastClick)
                {
                    m_HudClickStateList[i].isClick = false;
                    m_HudClickStateList[i].lastClick = false;
                }
                if (m_HudClickStateList[i].isClick)
                {
                    m_HudClickStateList[i].state = !m_HudClickStateList[i].state;
                    m_HudClickStateList[i].lastClick = true;
                }
                else 
                {
                    m_HudClickStateList[i].param = 0;
                    m_HudClickStateList[i].param2 = Vector2.zero;
                    m_HudClickStateList[i].param3 = null;
                }
            }
        }
        #endregion
    }
}

