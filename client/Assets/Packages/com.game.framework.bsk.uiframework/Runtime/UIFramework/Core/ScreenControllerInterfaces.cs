using System;
using UnityEngine;

namespace Game.UIFramework {
    /// <summary>
    /// Interface that all UI Screens must implement directly or indirectly
    /// </summary>
    public interface IUIScreenController {
        string ScreenId { get; set; }
        bool IsVisible { get; }
        UIFrame UiFrame { get; set; }

        IUIScreenController ParentUI { get; set; }

        void Open(bool playAnim = false, IScreenProperties props = null);      
        void Show();
        void Reset();
        void UIUpdate();
        void UILateUpdate();
        void UIFixedUpdate();
        void Hide();
        void Close(bool animate = false);
        void Clear();

        Action<IUIScreenController> InTransitionBegin { get; set; }
        Action<IUIScreenController> OutTransitionBegin { get; set; }
        Action<IUIScreenController> InTransitionFinished { get; set; }
        Action<IUIScreenController> OutTransitionFinished { get; set; }
        Action<IUIScreenController> CloseRequest { get; set; }
        Action<IUIScreenController> ScreenDestroyed { get; set; }

        event Action EventOnOpen;

        event Action EventOnClose;

        void HierarchyFixOnShow() { }

        void SetChildPanelTrans(Transform trans) { }
        
        void AddChildPanel(IUIScreenController childCtl) { }

        void ClearChildPanel(IUIScreenController childCtl) { }
        
        void ClearAllChildPanel() { }
    }

    /// <summary>
    /// Interface that all Windows must implement
    /// </summary>
    public interface IWindowController : IUIScreenController {
        bool HideOnForegroundLost { get; }
        bool IsPopup { get; }
        bool AlwaysPlayAnim { get; }
        IParameterData Data { get; }
        WindowPriority WindowPriority { get; }
    }

    /// <summary>
    /// Interface that all Panels must implement
    /// </summary>
    public interface IPanelController : IUIScreenController {
        bool AlwaysPlayAnim { get; }
        IParameterData Data { get; }
        PanelPriority Priority { get; }
    }
}
