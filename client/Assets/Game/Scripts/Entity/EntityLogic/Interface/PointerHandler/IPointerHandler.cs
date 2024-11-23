using System;
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
    // public struct FPointerEventData
    // {
    //     public EPOinterEventType EventType;
    // }

    public interface IPointerHandler
    {

        public abstract bool PointerEnter();

        public abstract bool PointerExit();

        public abstract bool PointerDown();

        public abstract bool PointerUp();

    }
}