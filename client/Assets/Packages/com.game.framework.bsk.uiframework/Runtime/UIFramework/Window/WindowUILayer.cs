using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UIFramework
{
    /// <summary>
    /// This layer controls all Windows.
    /// Windows are Screens that follow a history and a queue, and are displayed
    /// one at a time (and may or may not be modals). This also includes pop-ups.
    /// </summary>
    public class WindowUILayer : AUILayer<IWindowController>
    {
        [SerializeField] private WindowParaLayer priorityParaLayer = null;

        public IWindowController CurrentWindow { get; private set; }
        
        private Queue<WindowHistoryEntry> windowQueue;
        private Stack<WindowHistoryEntry> windowHistory;

        private Queue<WindowHistoryEntry> preOpenQueue;

        public Action InWindowHistotyEmpty { get; set; }

        private bool IsPreOpenWindowInProgress
        {
            get { return preOpenQueue.Count > 0; }
        }

        private bool isWindowPlayingAnim;
        public override void Initialize() {
            base.Initialize();
            registeredScreens = new Dictionary<string, IWindowController>();
            windowQueue = new Queue<WindowHistoryEntry>();
            windowHistory = new Stack<WindowHistoryEntry>();
            preOpenQueue = new Queue<WindowHistoryEntry>();
        }

        protected override void ProcessScreenRegister(string screenId, IWindowController controller)
        {
            base.ProcessScreenRegister(screenId, controller);
            controller.InTransitionFinished += OnInAnimationFinished;
            controller.OutTransitionFinished += OnOutAnimationFinished;
        }

        protected override void ProcessScreenUnregister(string screenId, IWindowController controller)
        {
            base.ProcessScreenUnregister(screenId, controller);
            controller.InTransitionFinished -= OnInAnimationFinished;
            controller.OutTransitionFinished -= OnOutAnimationFinished;
        }
        public override void OpenScreen(IWindowController screen, bool isImmediate = false) { 
            OpenScreen<IWindowProperties>(screen, null, isImmediate);
        }

        public override void OpenScreen<TProp>(IWindowController screen, TProp properties, bool isImmediate = false) {
            IWindowProperties windowProp = properties as IWindowProperties;

            if (ShouldEnqueue(screen, windowProp)) {
                EnqueueWindow(screen, properties);
            }
            else {
                DoOpen(screen, windowProp, isImmediate);
            }
        }

        public override void CloseScreen(IWindowController screen) {
            base.CloseScreen(screen);
            if (screen == CurrentWindow) {
                windowHistory.Pop();
                isWindowPlayingAnim = true;
                screen.Close(true);

                //Debug.Log("***** 缓存栈去除 "+ CurrentWindow.ScreenId);
                CurrentWindow = null;
                //Debug.Log("***** 当前CurrentWindow = null");
                //Debug.Log("***** 当前缓存池：<<<<<< ");
                //foreach (var ui in windowHistory)
                //{
                    //Debug.Log("***** " + ui.Screen.ScreenId);
                //}
                //Debug.Log("                  >>>>> ");
                if (windowQueue.Count > 0) {
                    OpenNextInQueue();
                    return;
                }
                else if (windowHistory.Count > 0) {
                    OpenPreviousInHistory();
                    return;
                }
                InWindowHistotyEmpty?.Invoke();
            }
            else {
                Debug.LogError(
                    string.Format(
                        "[WindowUILayer] Hide requested on WindowId {0} but that's not the currently open one ({1})! Ignoring request.",
                        screen.ScreenId, CurrentWindow != null ? CurrentWindow.ScreenId : "current is null"));
            }
        }

        public override void CloseAll(bool shouldAnimateWhenHiding = false) {
            base.CloseAll(shouldAnimateWhenHiding);
            CurrentWindow = null;
            isWindowPlayingAnim = false;
            ClearStack();
        }

        public void ClearStack()
        {           
            windowHistory.Clear();
            preOpenQueue.Clear();
            windowQueue.Clear();
        }

        public override void ReparentScreen(IUIScreenController controller, Transform screenTransform) {
            IWindowController window = controller as IWindowController;

            if (window == null) {
                Debug.LogError("[WindowUILayer] Screen " + screenTransform.name + " is not a Window!");
            }
            else {
                if (window.IsPopup) {
                    priorityParaLayer.AddScreen(screenTransform);
                    return;
                }
            }

            base.ReparentScreen(controller, screenTransform);
        }        

        private void EnqueueWindow<TProp>(IWindowController screen, TProp properties) where TProp : IScreenProperties {
            windowQueue.Enqueue(new WindowHistoryEntry(screen, (IWindowProperties) properties, false));
        }
        
        private bool ShouldEnqueue(IWindowController controller, IWindowProperties windowProp) {
            if (CurrentWindow == null && windowQueue.Count == 0) {
                return false;
            }

            if (windowProp != null && windowProp.SuppressPrefabProperties) {
                return windowProp.WindowQueuePriority != WindowPriority.ForceForeground;
            }

            if (controller.WindowPriority != WindowPriority.ForceForeground) {
                return true;
            }

            return false;
        }

        private void OpenPreviousInHistory() {
            if (windowHistory.Count > 0) {
                WindowHistoryEntry window = windowHistory.Pop();
                DoOpen(window, false);
            }
        }

        private void OpenNextInQueue() {
            if (windowQueue.Count > 0) {
                WindowHistoryEntry window = windowQueue.Dequeue();
                DoOpen(window, false);
            }
        }

        private void DoOpen(IWindowController screen, IWindowProperties properties, bool isImmediate = false) {
            DoOpen(new WindowHistoryEntry(screen, properties, true), true, isImmediate);
        }

        private void DoOpen(WindowHistoryEntry windowEntry, bool playAnim = true, bool isImmediate = false) {
            bool playCloseAnim = false;
            if (CurrentWindow == windowEntry.Screen) {
                Debug.LogWarning(
                    string.Format(
                        "[WindowUILayer] The requested WindowId ({0}) is already open! This will add a duplicate to the " +
                        "history and might cause inconsistent behaviour. It is recommended that if you need to open the same" +
                        "screen multiple times (eg: when implementing a warning message pop-up), it closes itself upon the player input" +
                        "that triggers the continuation of the flow."
                        , CurrentWindow.ScreenId));
                return;
            }
            windowEntry.needPlayAnim = playAnim;
            if (CurrentWindow != null
                     && CurrentWindow.HideOnForegroundLost
                     && !windowEntry.Screen.IsPopup) {
                playCloseAnim = CurrentWindow.AlwaysPlayAnim;
                CurrentWindow.Close(playCloseAnim && !isImmediate);
            }
            if (isImmediate)
            {
                preOpenQueue.Clear();
                isWindowPlayingAnim = false;
                DoOpenWindow(windowEntry);
                return;
            }
            if (IsPreOpenWindowInProgress || playCloseAnim || isWindowPlayingAnim)
            {
                AddPreOpenWindow(windowEntry);
                return;
            }
            DoOpenWindow(windowEntry);
            //Debug.Log("***** 当前CurrentWindow = "+ windowEntry.Screen.ScreenId);
            //Debug.Log("***** 缓存栈添加 " + CurrentWindow.ScreenId);
            //Debug.Log("***** 当前缓存池：<<<<<< ");
            //foreach (var ui in windowHistory)
            //{
            //Debug.Log("***** " + ui.Screen.ScreenId);
            //}
            //Debug.Log("*****             >>>>> ");
        }

        public bool IsWindowVisible(string windowId)
        {
            IWindowController window;
            if (registeredScreens.TryGetValue(windowId, out window))
            {
                return window.IsVisible;
            }

            return false;
        }

        private void OnOutAnimationFinished(IUIScreenController screen)
        {
            isWindowPlayingAnim = false;
            if (preOpenQueue.Count > 0)
            {
                if (preOpenQueue.TryDequeue(out var windowEntry))
                {
                    DoOpenWindow(windowEntry);
                }
            }
        }

        private void OnInAnimationFinished(IUIScreenController screen)
        {
            isWindowPlayingAnim = false;
            if (preOpenQueue.Count > 0)
            {
                if (preOpenQueue.TryDequeue(out var windowEntry))
                {
                    DoOpen(windowEntry, windowEntry.needPlayAnim);
                }
            }
        }

        private void AddPreOpenWindow(WindowHistoryEntry windowEntry)
        {
            if (preOpenQueue.Contains(windowEntry))
            {
                Debug.LogError("[WindowUILayer] Screen " + windowEntry.Screen.ScreenId + " is pre opening!");
                return;
            }
            preOpenQueue.Enqueue(windowEntry);
        }

        private void DoOpenWindow(WindowHistoryEntry windowEntry)
        {
            windowHistory.Push(windowEntry);
            if (windowEntry.Screen.AlwaysPlayAnim && !windowEntry.needPlayAnim)
                windowEntry.needPlayAnim = true;
            isWindowPlayingAnim = true;
            windowEntry.Open(windowEntry.needPlayAnim);

            CurrentWindow = windowEntry.Screen;
        }
    }
}
