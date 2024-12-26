using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;
// ReSharper disable InconsistentNaming
// ReSharper disable UnassignedField.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace Game.Client
{
    #region Common
    
    //通用HUD按钮
    public enum EHudBtnIndex
    {
        Default = 0,
        ReAmmoButton = 1,
        HudButtonNum,//计数位
    }
    
    //HUD按钮状态
    public class HudClickState
    {
        public bool isPress;
        public bool isClick;
        public bool state;
        public uint param;
        public Vector2 param2;
        public List<uint> param3;
        public bool lastClick;

        public bool isUp;
        public bool isDown;
    }

    #endregion

    #region MobileInputArea
    
    //用于区域相交判定的Btn列表
    public enum EBtnIndex
    {
        MoveArea = 0,
        FireButton = 1,
        MoveButton = 2,
        RotationButton = 3,
        ZoomButton = 4,
        JumpButton = 5,
        SprintButton = 6,
        InteractiveButton = 7,
        ButtonNum, //Button数量统计
    }

    public enum EBtnShapeType
    {
        Circle,
        Box,
        LeftRightButton,
    }

    //手机端输入阶段
    public enum InputPhase
    {
        Began,
        Moved,
        Stationary,
        Ended,
        Canceled,
    }
    
    //辅助进行输入区域判定的Button
    public class InputButtonInfo
    {
        public Vector2 Position;
        public Vector2 Size;
        public EBtnShapeType ButtonType;
        public bool IsFixed;
        public bool Enable = true;
        public bool bIsDown = false;
        public bool bIsUp = false;
        public bool bState = false;
        public bool bIsLock = false;
        public bool bLastState = false;
        
        public bool InButtonRange(Vector2 point)
        {
            if (Size == Vector2.zero)
            {
                return false;
            }
            if (ButtonType == EBtnShapeType.Circle)
            {
                float dis = (point - Position).magnitude * 2;
                if (dis <= Size.y)
                {
                    return true;
                }
                return false;
            }
            else if (ButtonType == EBtnShapeType.Box || ButtonType == EBtnShapeType.LeftRightButton)
            {
                if (Mathf.Abs(point.x - Position.x) * 2 <= Size.x
                    && Mathf.Abs(point.y - Position.y) * 2 <= Size.y)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
    
    //手机端使用的输入记录类
    public class PlayerMobileInput
    {
        public int FingerID;
        public InputPhase Phase;
        public Vector2 InitialPosition;
        public Vector2 CurrentPosition;
        public Vector2 LastPosition;
        public Vector2 DeltaPosition;
        public float DeltaTime;
        public float InitTime;
        public int InitFrameNum;
        public int LastUpdateFrameNum;
        public bool IgnoreUICollider;
        public int MiddleTouchUpFrameNum;

        public PlayerMobileInput(int inFingerID, Vector2 inPosition, int initFrameNum, float initTime)
        {
            FingerID = inFingerID;
            InitialPosition = inPosition;
            LastPosition = inPosition;
            InitFrameNum = initFrameNum;
            InitTime = initTime;
            MiddleTouchUpFrameNum = 0;
        }

        //逻辑帧，存在多个touch变化，可能在touch前 有抬起操作
        public bool ExistTouchUpBeforeTouch()
        {
            return (MiddleTouchUpFrameNum != 0 && MiddleTouchUpFrameNum < LastUpdateFrameNum);
        }

        //逻辑帧最终否在touch
        public bool IsFinalInTouch()
        {
            return (MiddleTouchUpFrameNum == 0 || MiddleTouchUpFrameNum < LastUpdateFrameNum);
        }

        public override string ToString()
        {
            return
                $"({FingerID} ({InitFrameNum}->{LastUpdateFrameNum}): {Phase} | {InitialPosition} | {CurrentPosition} | {DeltaPosition} | {MiddleTouchUpFrameNum})";
        }
    }
    
    
    //PC端使用的输入记录类
    public class PlayerPCInput: IReference
    {
        public KeyCode KeyCode;
        public float InitTime;
        public int InitFrameNum;
        public int LastUpdateFrameNum;
        public int MiddleUpFrameNum;

        public PlayerPCInput()
        {
        }
        
        public bool ExistKeyUpBeforeExecute()
        {
            return (MiddleUpFrameNum != 0 && MiddleUpFrameNum < LastUpdateFrameNum);
        }

        public void Clear()
        {
            InitFrameNum = 0;
            InitTime = 0;
            MiddleUpFrameNum = 0;
        }
    }
    
    //PC记录输入状态
    public class PlayerPCInputInfo
    {
        public bool Enable = true;
        public bool bIsDown = false;
        public bool bIsUp = false;
        public bool bState = false;
        public bool bIsLock = false;
        public bool bLastState = false;
    }

    //输入配置信息记录列表
    public class InputConfig 
    {
        public List<InputButtonConfig> inputButtonConfigs;
    }
    
    public class InputButtonConfig 
    {
        public int id;
        public float xSize;
        public float ySize;
        public int type;
        public bool isFixed;
    }

    #endregion
    
}
