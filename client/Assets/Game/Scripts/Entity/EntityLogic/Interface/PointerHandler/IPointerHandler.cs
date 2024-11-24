using System;
using Game.Math;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    // public enum EPOinterEventType
    // {
    //     None,
    //     Enter,
    //     Exit,
    //     Down,
    //     Up
    // }
    //
    public struct FPointerEventData
    {
        public float3 pointerWorldPos;
    }

    public interface IPointerHandler
    {

        public abstract bool PointerEnter(FPointerEventData eventData);

        public abstract bool PointerExit(FPointerEventData eventData);

        public abstract bool PointerDown(FPointerEventData eventData);

        public abstract bool PointerUp(FPointerEventData eventData);

    }
}