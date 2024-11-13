using UnityEngine;
using System.Collections.Generic;
using System;

namespace Game.UIFramework {
    /// <summary>
    /// Base class for UI Layers. Layers implement custom logic
    /// for Screen types when opening, closing etc.
    /// <seealso cref="WindowUILayer"/>
    /// <seealso cref="PanelUILayer"/>
    /// </summary>
    public abstract class AUILayer<TScreen> : MonoBehaviour where TScreen : IUIScreenController {
        protected Dictionary<string, TScreen> registeredScreens;

        protected UIFrame uiFrame;

        public UIFrame UiFrame
        {
            set {
                uiFrame = value;
            }
        }


        /// <summary>
        /// Shows a screen
        /// </summary>
        /// <param name="screen">The ScreenController to show</param>
        public abstract void OpenScreen(TScreen screen, bool isImmediate = false);

        /// <summary>
        /// Shows a screen passing in properties
        /// </summary>
        /// <param name="screen">The ScreenController to show</param>
        /// <param name="properties">The data payload</param>
        /// <typeparam name="TProps">The type of the data payload</typeparam>
        public abstract void OpenScreen<TProps>(TScreen screen, TProps properties, bool isImmediate = false) where TProps : IScreenProperties;

        /// <summary>
        /// Hides a screen
        /// </summary>
        /// <param name="screen">The ScreenController to be hidden</param>
        public virtual void CloseScreen(TScreen screen)
        {
            var parentUI = screen.ParentUI;
            if (parentUI != null)
            {
                parentUI.ClearChildPanel(screen);
                screen.ParentUI = null;
            }
            screen.ClearAllChildPanel();
        }

        /// <summary>
        /// Initialize this layer
        /// </summary>
        public virtual void Initialize() {
            registeredScreens = new Dictionary<string, TScreen>();
        }

        /// <summary>
        /// Reparents the screen to this Layer's transform
        /// </summary>
        /// <param name="controller">The screen controller</param>
        /// <param name="screenTransform">The Screen Transform</param>
        public virtual void ReparentScreen(IUIScreenController controller, Transform screenTransform) {
            screenTransform.SetParent(transform, false);
        }

        /// <summary>
        /// 将UI界面移动到缓存池中
        /// </summary>
        /// <param name="screenTransform">The Screen Transform</param>
        public void ReparentScreenToCache(Transform screenTransform)
        {
            screenTransform.SetParent(uiFrame.CacheTrans, false);
        }

        /// <summary>
        /// Register a ScreenController to a specific ScreenId
        /// </summary>
        /// <param name="screenId">Target ScreenId</param>
        /// <param name="controller">Screen Controller to be registered</param>
        public void RegisterScreen(string screenId, TScreen controller) {
            if (!registeredScreens.ContainsKey(screenId)) {
                ProcessScreenRegister(screenId, controller);
            }
            else {
                Debug.LogError("[AUILayerController] Screen controller already registered for id: " + screenId);
            }
        }

        /// <summary>
        /// Unregisters a given controller from a ScreenId
        /// </summary>
        /// <param name="screenId">The ScreenId</param>
        /// <param name="controller">The controller to be unregistered</param>
        public void UnregisterScreen(string screenId, TScreen controller) {
            if (registeredScreens.ContainsKey(screenId)) {
                ProcessScreenUnregister(screenId, controller);
            }
            else {
                Debug.LogError("[AUILayerController] Screen controller not registered for id: " + screenId);
            }
        }

        /// <summary>
        /// 获取已经注册过得Controller
        /// </summary>
        /// <param name="screenId">The desired ScreenId</param>
        /// <param name="ctl"> 输出的Controller</param>
        public bool GetRegisteredScreen(string screenId, out TScreen ctl)
        {
            if (registeredScreens.TryGetValue(screenId, out ctl))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to find a registered screen that matches the id
        /// and shows it.
        /// </summary>
        /// <param name="screenId">The desired ScreenId</param>
        /// <param name="parentIds">The desired ParentUI ScreenId</param>
        public void OpenScreenById(string screenId, string parentIds = null, bool isImmediate = false) {
            TScreen ctl;
            if (registeredScreens.TryGetValue(screenId, out ctl)) {
                OpenScreen(ctl, isImmediate);
            }
            else {
                Debug.LogError("[AUILayerController] Screen ID " + screenId + " not registered to this layer!");
                return;
            }
            if (!string.IsNullOrEmpty(parentIds))
            {
                IUIScreenController parentCtl;
                if (uiFrame.GetRegisteredScreen(parentIds, out parentCtl))
                {
                    parentCtl.AddChildPanel(ctl);
                    return;
                }
            }
        }

        /// <summary>
        /// Attempts to find a registered screen that matches the id
        /// and shows it, passing a data payload.
        /// </summary>
        /// <param name="screenId">The Screen Id (by default, it's the name of the Prefab)</param>
        /// <param name="param">The data payload for this screen to use</param>
        /// <typeparam name="T"> </typeparam>
        public void OpenScreenById<T>(string screenId, T param, string parentIds = null, bool isImmediate = false) where T : IParameterData {
            TScreen ctl;
            if (registeredScreens.TryGetValue(screenId, out ctl)) {
                IScreenProperties properties;
                if (ctl is AWindowController windowController)
                {
                    properties = new WindowProperties(windowController.WindowPriority,
                        windowController.HideOnForegroundLost,
                        windowController.IsPopup,
                        windowController.AlwaysPlayAnim,
                        param);
                }
                else if (ctl is APanelController panelController)
                {
                    properties = new PanelProperties(panelController.Priority,
                        panelController.AlwaysPlayAnim,
                        param);
                }
                else
                {
                    Debug.LogError("[AUILayerController] Screen ID " + screenId + " AScreenController is not Window or Panel!");
                    return;
                }
                OpenScreen(ctl, properties, isImmediate);
            }
            else {
                Debug.LogError("[AUILayerController] Screen ID " + screenId + " not registered!");
                return;
            }
            if (!string.IsNullOrEmpty(parentIds))
            {
                IUIScreenController parentCtl;
                if (uiFrame.GetRegisteredScreen(parentIds, out parentCtl))
                {
                    parentCtl.AddChildPanel(ctl);
                    return;
                }
            }
        }

        /// <summary>
        /// Attempts to find a registered screen that matches the id
        /// and hides it
        /// </summary>
        /// <param name="screenId">The id for this screen (by default, it's the name of the Prefab)</param>
        public void CloseScreenById(string screenId) {
            TScreen ctl;
            if (registeredScreens.TryGetValue(screenId, out ctl)) {
                CloseScreen(ctl);
            }
            else {
                Debug.LogError("[AUILayerController] Could not hide Screen ID " + screenId + " as it is not registered to this layer!");
            }
        }

        /// <summary>
        /// Checks if a screen is registered to this UI Layer
        /// </summary>
        /// <param name="screenId">The Screen Id (by default, it's the name of the Prefab)</param>
        /// <returns>True if screen is registered, false if not</returns>
        public bool IsScreenRegistered(string screenId) {
            return registeredScreens.ContainsKey(screenId);
        }

        public virtual void ShowAll()
        {
            foreach (var screen in registeredScreens)
            {
                if (screen.Value.IsVisible)
                    screen.Value.Show();
            }
        }

        public virtual void HideAll()
        {
            foreach (var screen in registeredScreens)
            {
                if (screen.Value.IsVisible)
                    screen.Value.Hide();
            }
        }

        public virtual void ResetAll()
        {
            foreach (var screen in registeredScreens)
            {
                if (screen.Value.IsVisible)
                {
                    Debug.Log($"****  {screen.Value.ScreenId} Reset!");
                    screen.Value.Reset();
                }
            }
        }

        /// <summary>
        /// Hides all screens registered to this layer
        /// </summary>
        /// <param name="shouldAnimateWhenHiding">Should the screen animate while hiding?</param>
        public virtual void CloseAll(bool shouldAnimateWhenHiding = false) {
            foreach (var screen in registeredScreens) {
                if (screen.Value.IsVisible)
                {
                    screen.Value.Close(shouldAnimateWhenHiding);
                }
            }
        }

        public virtual void ClearAllCache()
        {
            List<TScreen> screensToRemove = new List<TScreen>();

            foreach (var screen in registeredScreens)
            {
                if (!screen.Value.IsVisible)
                {
                    screensToRemove.Add(screen.Value);
                }
            }

            foreach (var screenToRemove in screensToRemove)
            {
                screenToRemove.Clear();
            }
            screensToRemove.Clear();
        }

        protected virtual void ProcessScreenRegister(string screenId, TScreen controller) {
            controller.ScreenId = screenId;
            registeredScreens.Add(screenId, controller);
            controller.ScreenDestroyed += OnScreenDestroyed;
            controller.InTransitionBegin += OnInAnimationBegin;
            controller.OutTransitionBegin += OnOutAnimationBegin;
            controller.InTransitionFinished += OnInAnimationFinished;
            controller.OutTransitionFinished += OnOutAnimationFinished;
            controller.CloseRequest += OnCloseRequested;
        }

        protected virtual void ProcessScreenUnregister(string screenId, TScreen controller) {
            controller.ScreenDestroyed -= OnScreenDestroyed;
            registeredScreens.Remove(screenId);
            controller.InTransitionBegin -= OnInAnimationBegin;
            controller.OutTransitionBegin -= OnOutAnimationBegin;
            controller.InTransitionFinished -= OnInAnimationFinished;
            controller.OutTransitionFinished -= OnOutAnimationFinished;
            controller.CloseRequest -= OnCloseRequested;
        }

        private void OnScreenDestroyed(IUIScreenController screen) {
            if (!string.IsNullOrEmpty(screen.ScreenId)
                && registeredScreens.ContainsKey(screen.ScreenId)) {
                UnregisterScreen(screen.ScreenId, (TScreen) screen);
            }
        }

        private void OnInAnimationBegin(IUIScreenController screen)
        {
            AddTransition(screen);
        }

        private void OnOutAnimationBegin(IUIScreenController screen)
        {
            AddTransition(screen);
        }

        private void OnInAnimationFinished(IUIScreenController screen)
        {
            RemoveTransition(screen);
        }

        private void OnOutAnimationFinished(IUIScreenController screen)
        {
            RemoveTransition(screen);
        }

        private void OnCloseRequested(IUIScreenController screen)
        {
            if (screen is TScreen tScreen)
            {
                CloseScreen(tScreen);
            }
            else
                Debug.LogError("Invaild UIScreenController Type!!!!!");
        }

        protected void AddTransition(IUIScreenController screen)
        {
            uiFrame.AddTransition(screen);
        }

        protected void RemoveTransition(IUIScreenController screen)
        {
            uiFrame.RemoveTransition(screen);
        }
    }
}
