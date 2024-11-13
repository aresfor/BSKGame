using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UIFramework
{
    public class TickData
    {
        //刷新间隔
        public int interval;

        //当前tick
        public int curTick;
        //UIController
        public IUIScreenController screenController;

        public TickData(IUIScreenController ctl, int tick)
        {
            interval = tick;
            screenController = ctl;
            curTick = 0;
        }
    }

    /// <summary>
    /// This is the centralized access point for all things UI.
    /// All your calls should be directed at this.
    /// </summary>
    public class UIFrame : MonoBehaviour
    {
        #region 参数
        [Tooltip("Set this to false if you want to manually initialize this UI Frame.")]
        [SerializeField] private bool initializeOnAwake = true;
        
        private PanelUILayer panelLayer;
        private WindowUILayer windowLayer;

        private Canvas mainCanvas;
        private GraphicRaycaster graphicRaycaster;

        //存放关闭，但是未销毁的UI
        [SerializeField] private Transform cacheTrans;
        //存放正在预加载的UI
        [SerializeField] private Transform preloadTrans;
        //clone到每个UI下，存放每个UI的子界面
        [SerializeField] private Transform childPanelsTrans;

        [SerializeField] private Camera uiCamera;

        private Dictionary<string, TickData> _dcUpdateUIScreen= new Dictionary<string, TickData>();

        private Dictionary<string, TickData> _dcLateUpdateUIScreen = new Dictionary<string, TickData>();

        private Dictionary<string, TickData> _dcFixedUpdateUIScreen = new Dictionary<string, TickData>();

        [SerializeField] private List<String> _lstUpdateUIScreen = new List<String>();
        [SerializeField] private List<String> _lstLateUpdateUIScreen = new List<String>();
        [SerializeField] private List<String> _lstFixedUpdateUIScreen = new List<String>();
        private bool IsScreenTransitionInProgress
        {
            get { return screensTransitioning.Count != 0; }
        }

        private HashSet<IUIScreenController> screensTransitioning;

        /// <summary>
        /// The main canvas of this UI
        /// </summary>
        public Canvas MainCanvas {
            get {
                if (mainCanvas == null) {
                    mainCanvas = GetComponent<Canvas>();
                }

                return mainCanvas;
            }
        }

        /// <summary>
        /// The Camera being used by the Main UI Canvas
        /// </summary>
        public Camera UICamera {
            get
            {
                if (uiCamera != null)
                {
                    return uiCamera;
                }
                return MainCanvas.worldCamera;
            }
        }

        public Transform CacheTrans
        {
            get
            {
                if (cacheTrans != null)
                {
                    return cacheTrans;
                }
                return windowLayer.transform;
            }
        }

        public Transform PreLoadTrans
        {
            get
            {
                if (preloadTrans != null)
                {
                    return preloadTrans;
                }
                return windowLayer.transform;
            }
        }

        public Transform ChildPanelsTrans
        {
            get
            {
                if (childPanelsTrans != null)
                {
                    return childPanelsTrans;
                }
                return new GameObject().transform;
            }
        }
        #endregion

        #region Base Function
        private void Awake() {
            if (initializeOnAwake) {
                Initialize();    
            }
        }

        /// <summary>
        /// Initializes this UI Frame. Initialization consists of initializing both the Panel and Window layers.
        /// Although literally all the cases I've had to this day were covered by the "Window and Panel" approach,
        /// I made it virtual in case you ever need additional layers or other special initialization.
        /// </summary>
        public virtual void Initialize() {
            if (panelLayer == null) {
                panelLayer = gameObject.GetComponentInChildren<PanelUILayer>(true);
                if (panelLayer == null) {
                    Debug.LogError("[UI Frame] UI Frame lacks Panel Layer!");
                }
                else {
                    panelLayer.Initialize();
                    panelLayer.UiFrame = this;
                }
            }

            if (windowLayer == null) {
                windowLayer = gameObject.GetComponentInChildren<WindowUILayer>(true);
                if (panelLayer == null) {
                    Debug.LogError("[UI Frame] UI Frame lacks Window Layer!");
                }
                else {
                    windowLayer.Initialize();
                    windowLayer.UiFrame = this;
                }
            }
            graphicRaycaster = MainCanvas.GetComponent<GraphicRaycaster>();
            screensTransitioning = new HashSet<IUIScreenController>();
        }

        private void Update()
        {
            for (int i = 0; i < _lstUpdateUIScreen.Count; i++)
            {
                var screenName = _lstUpdateUIScreen[i];
                if (_dcUpdateUIScreen.TryGetValue(screenName, out TickData tickData))
                {
                    tickData.curTick++;
                    if (tickData.curTick >= tickData.interval)
                    {
                        tickData.screenController.UIUpdate();
                        tickData.curTick = 0;
                    }
                }
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _lstLateUpdateUIScreen.Count; i++)
            {
                var screenName = _lstLateUpdateUIScreen[i];
                if (_dcLateUpdateUIScreen.TryGetValue(screenName, out TickData tickData))
                {
                    tickData.curTick++;
                    if (tickData.curTick >= tickData.interval)
                    {
                        tickData.screenController.UILateUpdate();
                        tickData.curTick = 0;
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < _lstFixedUpdateUIScreen.Count; i++)
            {
                var screenName = _lstFixedUpdateUIScreen[i];
                if (_dcFixedUpdateUIScreen.TryGetValue(screenName, out TickData tickData))
                {
                    tickData.curTick++;
                    if (tickData.curTick >= tickData.interval)
                    {
                        tickData.screenController.UIFixedUpdate();
                        tickData.curTick = 0;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            _dcUpdateUIScreen.Clear();
            _dcLateUpdateUIScreen.Clear();
            _dcFixedUpdateUIScreen.Clear();

            _lstUpdateUIScreen.Clear();
            _lstLateUpdateUIScreen.Clear();
            _lstFixedUpdateUIScreen.Clear();
        }

    #endregion

    #region Public Function

    #region WindowLayer
    /// <summary>
    /// Opens the Window with the given Id, with no Properties.
    /// </summary>
    /// <param name="screenId">Identifier.</param>
    public void OpenWindow(string screenId, bool isImmediate = false)
        {
            if (windowLayer.IsWindowVisible(screenId))
            {
                Debug.LogError($"Error! {screenId} already Visible!");
                return;
            }
            windowLayer.OpenScreenById(screenId, null, isImmediate);
        }

        /// <summary>
        /// Closes the Window with the given Id.
        /// </summary>
        /// <param name="screenId">Identifier.</param>
        public void CloseWindow(string screenId)
        {
            windowLayer.CloseScreenById(screenId);
        }

        /// <summary>
        /// Closes the currently open window, if any is open
        /// </summary>
        public void CloseCurrentWindow()
        {
            if (windowLayer.CurrentWindow != null)
            {
                CloseWindow(windowLayer.CurrentWindow.ScreenId);
            }
        }

        /// <summary>
        /// get the currently open window, if any is exist,return screenId
        /// 获取当前开启的window界面screenId
        /// </summary>
        public string GetCurrentWindowByScreenId()
        {
            if (windowLayer)
                return windowLayer.CurrentWindow.ScreenId;
            return null;
        }

        /// <summary>
        /// Opens the Window with the given id, passing in Properties.
        /// </summary>
        /// <param name="screenId">Identifier.</param>
        /// <param name="properties">Properties.</param>
        /// <typeparam name="T">The type of properties to be passed in.</typeparam>
        /// <seealso cref="IParameterData"/>
        public void OpenWindow<T>(string screenId, T param, bool isImmediate = false) where T : IParameterData
        {
            windowLayer.OpenScreenById<T>(screenId, param, null, isImmediate);
        }

        /// <summary>
        /// Registers the Window. Windows can only be opened after they're registered.
        /// </summary>
        /// <param name="screenId">Screen identifier.</param>
        /// <param name="controller">Controller.</param>
        /// <typeparam name="TWindow">The Controller type.</typeparam>
        public void RegisterWindow<TWindow>(string screenId, TWindow controller) where TWindow : IWindowController
        {
            windowLayer.RegisterScreen(screenId, controller);
        }

        /// <summary>
        /// Unregisters the Window.
        /// </summary>
        /// <param name="screenId">Screen identifier.</param>
        /// <param name="controller">Controller.</param>
        /// <typeparam name="TWindow">The Controller type.</typeparam>
        public void UnregisterWindow<TWindow>(string screenId, TWindow controller) where TWindow : IWindowController
        {
            windowLayer.UnregisterScreen(screenId, controller);
        }

        public void ClearWindowStack()
        {
            windowLayer.ClearStack();
        }

        public void AddWindowHistoryEmpty(Action action)
        {
            if (windowLayer != null)
                windowLayer.InWindowHistotyEmpty += action;
        }

        #endregion

        #region PanelLayer
        /// <summary>
        /// Open a panel by its id, passing no Properties.
        /// </summary>
        /// <param name="screenId">Panel Id</param>
        public void OpenPanel(string screenId, string parentIds = null)
        {
            if (panelLayer.IsPanelVisible(screenId))
            {
                Debug.LogError($"Error! {screenId} already Visible!");
                return;
            }
            panelLayer.OpenScreenById(screenId, parentIds);
        }

        /// <summary>
        /// Open a panel by its id, passing parameters.
        /// </summary>
        /// <param name="screenId">Identifier.</param>
        /// <param name="properties">Properties.</param>
        /// <typeparam name="T">The type of properties to be passed in.</typeparam>
        /// <seealso cref="IParameterData"/>
        public void OpenPanel<T>(string screenId, T param, string parentIds = null) where T : IParameterData
        {
            panelLayer.OpenScreenById<T>(screenId, param, parentIds);
        }

        /// <summary>
        /// Close the panel with the given id.
        /// </summary>
        /// <param name="screenId">Identifier.</param>
        public void ClosePanel(string screenId)
        {
            panelLayer.CloseScreenById(screenId);
        }

        /// <summary>
        /// Registers the panel. Panels can only be shown after they're registered.
        /// </summary>
        /// <param name="screenId">Screen identifier.</param>
        /// <param name="controller">Controller.</param>
        /// <typeparam name="TPanel">The Controller type.</typeparam>
        public void RegisterPanel<TPanel>(string screenId, TPanel controller) where TPanel : IPanelController
        {
            panelLayer.RegisterScreen(screenId, controller);
        }

        /// <summary>
        /// Unregisters the panel.
        /// </summary>
        /// <param name="screenId">Screen identifier.</param>
        /// <param name="controller">Controller.</param>
        /// <typeparam name="TPanel">The Controller type.</typeparam>
        public void UnregisterPanel<TPanel>(string screenId, TPanel controller) where TPanel : IPanelController
        {
            panelLayer.UnregisterScreen(screenId, controller);
        }

        #endregion

        #region All
        /// <summary>
        /// 开启界面
        /// </summary>
        /// <param name="screenId">界面ScreenId</param>
        /// <param name="param">界面参数</param>
        /// <param name="parentIds">父界面ScreenId</param>
        /// <param name="isImmediate">是否立即开启界面</param>
        public void OpenScreen<T>(string screenId, T param, string parentIds = null, bool isImmediate = false) where T : IParameterData
        {
            Type type;
            if (IsScreenRegistered(screenId, out type))
            {
                if (type == typeof(IWindowController))
                {
                    //如果是Window，不设置父界面，直接用windowLayer来控制
                    OpenWindow(screenId, param, isImmediate);
                }
                else if (type == typeof(IPanelController))
                {
                    OpenPanel(screenId, param, parentIds);
                }
            }
            else
            {
                Debug.LogError(string.Format("Tried to open Screen id {0} but it's not registered as Window or Panel!",
                    screenId));
            }
        }

        /// <summary>
        /// Searches for the given id among the Layers, opens the Screen if it finds it
        /// </summary>
        /// <param name="screenId">The Screen id.</param>
        public void OpenScreen(string screenId, string parentIds = null, bool isImmediate = false)
        {
            Type type;
            if (IsScreenRegistered(screenId, out type))
            {
                if (type == typeof(IWindowController))
                {
                    //如果是Window，不设置父界面，直接用windowLayer来控制
                    OpenWindow(screenId, isImmediate);
                }
                else if (type == typeof(IPanelController))
                {
                    OpenPanel(screenId, parentIds);
                }
            }
            else
            {
                Debug.LogError(string.Format("Tried to open Screen id {0} but it's not registered as Window or Panel!",
                    screenId));
            }
        }

        /// <summary>
        /// Searches for the given id among the Layers, Close the Screen if it finds it
        /// </summary>
        /// <param name="screenId">The Screen id.</param>
        public void CloseScreen(string screenId)
        {
            Type type;
            if (IsScreenRegistered(screenId, out type))
            {
                if (type == typeof(IWindowController))
                {
                    CloseWindow(screenId);
                }
                else if (type == typeof(IPanelController))
                {
                    ClosePanel(screenId);
                }
            }
            else
            {
                Debug.LogError(string.Format("Tried to Close Screen id {0} but it's not registered as Window or Panel!", screenId));
            }
        }

        /// <summary>
        /// Registers a screen. If transform is passed, the layer will
        /// reparent it to itself. Screens can only be shown after they're registered.
        /// </summary>
        /// <param name="screenId">Screen identifier.</param>
        /// <param name="controller">Controller.</param>
        public void RegisterScreen(string screenId, IUIScreenController controller)
        {
            IWindowController window = controller as IWindowController;
            if (window != null)
            {
                windowLayer.RegisterScreen(screenId, window);
                return;
            }

            IPanelController panel = controller as IPanelController;
            if (panel != null)
            {
                panelLayer.RegisterScreen(screenId, panel);
            }
        }

        /// <summary>
        /// Registers a screen. If transform is passed, the layer will
        /// reparent it to itself. Screens can only be shown after they're registered.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="screenTransform">Screen transform. If not null, will be reparented to proper layer</param>
        public void ReparentScreen(IUIScreenController controller, Transform screenTransform)
        {
            IWindowController window = controller as IWindowController;
            if (window != null)
            {
                if (screenTransform != null)
                {
                    windowLayer.ReparentScreen(controller, screenTransform);
                }

                return;
            }

            IPanelController panel = controller as IPanelController;
            if (panel != null)
            {
                if (screenTransform != null)
                {
                    panelLayer.ReparentScreen(controller, screenTransform);
                }
            }
        }

        /// <summary>
        /// Registers a screen. If transform is passed, the layer will
        /// reparent it to itself. Screens can only be shown after they're registered.
        /// </summary>
        /// <param name="controller">Controller.</param>
        /// <param name="screenTransform">Screen transform. If not null, will be reparented to proper layer</param>
        public void ReparentScreenToCache(IUIScreenController controller, Transform screenTransform)
        {
            IWindowController window = controller as IWindowController;
            if (window != null)
            {
                if (screenTransform != null)
                {
                    windowLayer.ReparentScreenToCache(screenTransform);
                }

                return;
            }

            IPanelController panel = controller as IPanelController;
            if (panel != null)
            {
                if (screenTransform != null)
                {
                    panelLayer.ReparentScreenToCache(screenTransform);
                }
            }
        }

        /// <summary>
        /// Checks if a given Panel is open.
        /// </summary>
        /// <param name="screenId">Panel identifier.</param>
        public bool IsScreenOpen(string screenId)
        {
            return panelLayer.IsPanelVisible(screenId) || windowLayer.IsWindowVisible(screenId);
        }

        /// <summary>
        /// Checks if a given screen id is registered to either the Window or Panel layers
        /// </summary>
        /// <param name="screenId">The Id to check.</param>
        public bool IsScreenRegistered(string screenId)
        {
            if (windowLayer.IsScreenRegistered(screenId))
            {
                return true;
            }

            if (panelLayer.IsScreenRegistered(screenId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a given screen id is registered to either the Window or Panel layers,
        /// also returning the screen type
        /// </summary>
        /// <param name="screenId">The Id to check.</param>
        /// <param name="type">The type of the screen.</param>
        public bool IsScreenRegistered(string screenId, out Type type)
        {
            if (windowLayer.IsScreenRegistered(screenId))
            {
                type = typeof(IWindowController);
                return true;
            }

            if (panelLayer.IsScreenRegistered(screenId))
            {
                type = typeof(IPanelController);
                return true;
            }

            type = null;
            return false;
        }

        public bool GetRegisteredScreen(string screenId, out IUIScreenController ctl)
        {
            if (windowLayer.GetRegisteredScreen(screenId, out var winCtl))
            {
                ctl = winCtl;
                return true;
            }

            if (panelLayer.GetRegisteredScreen(screenId, out var panelCtl))
            {
                ctl = panelCtl;
                return true;
            }

            ctl = null;
            return false;
        }

        public void AddTransition(IUIScreenController screen)
        {
            //Debug.Log($"******  Add {screen.ScreenId}");
            screensTransitioning.Add(screen);
            //Debug.Log($"******  锁屏幕");
            OnRequestScreenBlock();
        }

        public void RemoveTransition(IUIScreenController screen)
        {
            //Debug.Log($"******  Remove{screen.ScreenId}");
            screensTransitioning.Remove(screen);
            if (!IsScreenTransitionInProgress)
            {
                //Debug.Log($"******  解锁屏幕");
                OnRequestScreenUnblock();
            }
        }

        public void ClearTransition()
        {
            screensTransitioning.Clear();
            OnRequestScreenUnblock();
        }

        public void ResetAllScreen()
        {
            windowLayer.ResetAll();
            panelLayer.ResetAll();
        }

        #endregion

        #region Close && Hide && Clear

        /// <summary>
        /// HideScreen
        /// </summary>
        /// <param name="screenId">The Screen id.</param>
        public void HideScreen(string screenId)
        {
            if (GetRegisteredScreen(screenId, out IUIScreenController ctl))
            {
                if (ctl.IsVisible)
                    ctl.Hide();
            }
        }

        /// <summary>
        /// ShowScreen
        /// </summary>
        /// <param name="screenId">The Screen id.</param>
        public void ShowScreen(string screenId)
        {
            if (GetRegisteredScreen(screenId, out IUIScreenController ctl))
            {
                if(ctl.IsVisible)
                    ctl.Show();
            }
        }

        /// <summary>
        /// HideAllScreen
        /// </summary>
        public void HideAllScreen()
        {
            windowLayer.HideAll();
            panelLayer.HideAll();
        }

        /// <summary>
        /// ShowAllScreen
        /// </summary>
        public void ShowAllScreen()
        {
            windowLayer.ShowAll();
            panelLayer.ShowAll();
        }

        /// <summary>
        /// HideAllPanel
        /// </summary>
        public void HideAllPanel()
        {
            panelLayer.HideAll();
        }

        /// <summary>
        /// ShowAllPanel
        /// </summary>
        public void ShowAllPanel()
        {
            panelLayer.ShowAll();
        }

        /// <summary>
        /// Close all screens
        /// </summary>
        /// <param name="animate">Defines if screens should the screens animate out or not.</param>
        public void CloseAll(bool animate = false) {
            CloseAllWindows(animate);
            CloseAllPanels(animate);
        }

        /// <summary>
        /// Close all screens on the Panel Layer
        /// </summary>
        /// <param name="animate">Defines if screens should the screens animate out or not.</param>
        public void CloseAllPanels(bool animate = false) {
            panelLayer.CloseAll(animate);
        }

        /// <summary>
        /// Close all screens in the Window Layer
        /// </summary>
        /// <param name="animate">Defines if screens should the screens animate out or not.</param>
        public void CloseAllWindows(bool animate = false) {
            windowLayer.CloseAll(animate);
        }

        /// <summary>
        /// Clear all screens cache
        /// </summary>
        public void ClearAllCache()
        {
            ClearAllWindowsCache();
            ClearAllPanelsCache();
        }

        /// <summary>
        /// Clear all screens cache on the Window Layer
        /// </summary>
        public void ClearAllWindowsCache()
        {
            windowLayer.ClearAllCache();
        }


        /// <summary>
        /// Clear all screens cache on the Panel Layer
        /// </summary>
        public void ClearAllPanelsCache()
        {
            panelLayer.ClearAllCache();
        }
        #endregion

        #region Update
        public void AddUpdate(IUIScreenController ctl, int tick = 0)
        {
            _dcUpdateUIScreen.Add(ctl.ScreenId, new TickData(ctl, tick));
            if (!_lstUpdateUIScreen.Contains(ctl.ScreenId))
                _lstUpdateUIScreen.Add(ctl.ScreenId);
        }

        public void RemoveUpdate(IUIScreenController ctl)
        {
            _dcUpdateUIScreen.Remove(ctl.ScreenId);
            _lstUpdateUIScreen.Remove(ctl.ScreenId);
        }

        public void AddLateUpdate(IUIScreenController ctl, int tick = 0)
        {
            _dcLateUpdateUIScreen.Add(ctl.ScreenId, new TickData(ctl, tick));
            if (!_lstLateUpdateUIScreen.Contains(ctl.ScreenId))
                _lstLateUpdateUIScreen.Add(ctl.ScreenId);
        }

        public void RemoveLateUpdate(IUIScreenController ctl)
        {
            _dcLateUpdateUIScreen.Remove(ctl.ScreenId);
            _lstLateUpdateUIScreen.Remove(ctl.ScreenId);
        }

        public void AddFixedUpdate(IUIScreenController ctl, int tick = 0)
        {
            _dcFixedUpdateUIScreen.Add(ctl.ScreenId, new TickData(ctl, tick));
            if (!_lstFixedUpdateUIScreen.Contains(ctl.ScreenId))
                _lstFixedUpdateUIScreen.Add(ctl.ScreenId);
        }

        public void RemoveFixedUpdate(IUIScreenController ctl)
        {
            _dcFixedUpdateUIScreen.Remove(ctl.ScreenId);
            _lstFixedUpdateUIScreen.Remove(ctl.ScreenId);
        }
        #endregion

        #endregion

        #region Event
        private void OnRequestScreenBlock() {
            if (graphicRaycaster != null) {
                graphicRaycaster.enabled = false;
            }
        }

        private void OnRequestScreenUnblock() {
            if (graphicRaycaster != null) {
                graphicRaycaster.enabled = true;
            }
        }

        #endregion
    }
}
