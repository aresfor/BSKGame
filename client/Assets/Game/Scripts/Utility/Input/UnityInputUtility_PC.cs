using Game.Core;
using Game.Gameplay;
using Game.Math;
using GameFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Client
{
    public class UnityInputUtility_PC: IInputUtility
    {
        private bool m_EnableInPut;
        public bool EnableInput
        {
            get=> m_EnableInPut && Initialized;
            set=> m_EnableInPut = value;
        }
        
        public void Init()
        {
            Initialized = true;
            EnableInput = true;
        }

        public void Deinit()
        {
            Initialized = false;
        }

        public bool Initialized { get; set; }
        public void Update(float deltaTime)
        {
            FireMouseRayCast();
        }

        private void FireMouseRayCast()
        {
            //检测是否在UI上
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Camera.main == null)
                return;
            var mouseRay = GameUtils.MouseRay;

            ImpactInfo impactInfo = null;

            bool hit = PhysicsUtils.SingleLineCheck(mouseRay.origin.ToFloat3(), mouseRay.direction.ToFloat3()
                , 100.0f, InputUtils.MouseRayTraceLayer
                , ref impactInfo, true, duration: 0.0f);

            var eventArgs = ReferencePool.Acquire<MouseRayCastEventArgs>();
            eventArgs.ImpactInfo = impactInfo;
            eventArgs.bIsHit = hit;
            GameEntry.Event.FireNow(this, eventArgs);
            ImpactInfo.Recycle(impactInfo);
        }
        
        
        public float2 GetMoveInputParam(EInputParam param)
        {
            throw new System.NotImplementedException();
        }

        public bool GetInput(EInputParam moveParam)
        {
            throw new System.NotImplementedException();
        }

        public bool GetKeyDown(EKeyCode keyCode)
        {
            if (false == EnableInput)
                return false;

            return Input.GetKeyDown((KeyCode)(int)keyCode);
        }

        public bool GetKeyUp(EKeyCode keyCode)
        {
            if (false == EnableInput)
                return false;

            return Input.GetKeyUp((KeyCode)(int)keyCode);
        }

        public bool GetKey(EKeyCode keyCode)
        {
            if (false == EnableInput)
                return false;

            return Input.GetKey((KeyCode)(int)keyCode);
        }

       
    }
}