using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Game.UIFramework
{
    /// <summary>
    /// Base implementation for UI Screens. You'll probably want to inherit
    /// from one of its child classes: AWindowController or APanelController, not this.
    /// <seealso cref="AWindowController"/>
    /// <seealso cref="APanelController"/>
    /// </summary>
    public abstract class AUIScreenController<TProps> : MonoBehaviour, IUIScreenController
        where TProps : IScreenProperties
    {
        #region 参数
        [Header("Screen properties")]
        [Tooltip(
            "This is the data payload and settings for this screen. You can rig this directly in a prefab and/or pass it when you show this screen")]
        [SerializeField]
        private TProps properties;

        private CanvasGroup _canvasGroup;

        private List<APanelController> childPanels = new List<APanelController>();

        private IUIScreenController parentUI;

        private Animation _anim;

        protected Animation Anim
        {
            get { 
                if(_anim == null)
                    _anim = transform.GetComponent<Animation>();
                return _anim; }
        }

        [SerializeField]
        private string _animName_fadeIn;

        //            animName_fadeIn = string.Format("Ani_{0}_In",transform.name);
       // animName_fadeOut = string.Format("Ani_{0}_Out", transform.name);
        //animName_idle = string.Format("Ani_{0}_Idle", transform.name);
        protected string AnimName_fadeIn
        {
            get {
                if(string.IsNullOrEmpty(_animName_fadeIn))
                    _animName_fadeIn = string.Format("Ani_{0}_In", transform.name);
                return _animName_fadeIn;
            }
            set
            {
                _animName_fadeIn = value;
            }
        }

        [SerializeField]
        private string _animName_fadeOut;
        protected string AnimName_fadeOut
        {
            get
            {
                if (string.IsNullOrEmpty(_animName_fadeOut))
                    _animName_fadeOut = string.Format("Ani_{0}_Out", transform.name);
                return _animName_fadeOut;
            }
            set
            {
                _animName_fadeOut = value;
            }
        }

        [SerializeField]
        private string _animName_idle;
        protected string AnimName_idle
        {
            get
            {
                if (string.IsNullOrEmpty(_animName_idle))
                    _animName_idle = string.Format("Ani_{0}_Idle", transform.name);
                return _animName_idle;
            }
            set
            {
                _animName_idle = value;
            }
        }
        /// <summary>
        /// Is this screen currently visible?
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool IsVisible { get; private set; }

        /// <summary>
        /// The properties of this screen. Can contain
        /// serialized values, or passed in private values.
        /// </summary>
        /// <value>The properties.</value>
        protected TProps Properties
        {
            get { return properties; }
            set { properties = value; }
        }

        public UIFrame UiFrame { get; set; }

        /// <summary>
        /// Unique identifier for this ID. If using the default system, it should be the same name as the screen's Prefab.
        /// </summary>
        public string ScreenId { get; set; }

        #region 子界面相关
        public List<APanelController> ChildPanels
        {
            get { return childPanels; }
        }

        public IUIScreenController ParentUI
        {
            get { return parentUI; }
            set { parentUI = value; }
        }

        private Transform childPanelTrans;

        private bool isUpdate;
        private bool isLateUpdate;
        private bool isFixedUpdate;


        #endregion
        #endregion

        #region ActionEvent

        public Action<IUIScreenController> InTransitionBegin { get; set; }

        public Action<IUIScreenController> OutTransitionBegin { get; set; }

        /// <summary>
        /// Occurs when "in" transition is finished.
        /// </summary>
        public Action<IUIScreenController> InTransitionFinished { get; set; }

        /// <summary>
        /// Occurs when "out" transition is finished.
        /// </summary>
        public Action<IUIScreenController> OutTransitionFinished { get; set; }

        /// <summary>
        /// Screen can fire this event to request its responsible layer to close it
        /// </summary>
        /// <value>The close request.</value>
        public Action<IUIScreenController> CloseRequest { get; set; }

        /// <summary>
        /// If this screen is destroyed for some reason, it must warn its layer
        /// </summary>
        /// <value>The destruction action.</value>
        public Action<IUIScreenController> ScreenDestroyed { get; set; }

        public event Action EventOnOpen
        {
            add
            {
                _eventOnOpen += value;
                if (IsVisible) value();
            }
            remove
            {
                _eventOnOpen -= value;
            }
        }
        public event Action EventOnClose;

        private Action _eventOnOpen;

        public event Action EventOnShow
        {
            add
            {
                _eventOnShow += value;
                if (IsVisible) value();
            }
            remove
            {
                _eventOnShow -= value;
            }
        }
        public event Action EventOnHide;

        private Action _eventOnShow;

        #endregion

        #region Base Funtion
        private void Awake()
        {
            Init();
        }


        /// <summary>
        ///界面初始化
        /// </summary>
        private void Init()
        {
            _canvasGroup = transform.GetComponent<CanvasGroup>();
            isUpdate = false;
            isLateUpdate = false;
            isFixedUpdate = false;
            _anim = transform.GetComponent<Animation>();
            AddMonoData();
            AddListeners();
            OnInit();
        }

        /// <summary>
        /// Open this screen with the specified properties.
        /// 开启界面
        /// </summary>
        /// <param name="playAnim">是否播放动画</param>
        /// <param name="props">The data for the screen.</param>
        public void Open(bool playAnim = false, IScreenProperties props = null)
        {
            if (props != null)
            {
                if (props is TProps)
                {
                    SetProperties((TProps)props);
                }
                else
                {
                    Debug.LogError("Properties passed have wrong type! (" + props.GetType() + " instead of " +
                                   typeof(TProps) + ")");
                    return;
                }
            }
            UiFrame.ReparentScreen(this, transform);
            HierarchyFixOnShow();
            OnPropertiesSet();           

            OnOpen();
            _eventOnOpen?.Invoke();

            IsVisible = true;
            DoAnimation(playAnim, OnTransitionInFinished, true);

            if (childPanels.Count > 0)
            {
                foreach (var child in childPanels)
                {
                    child.Open(playAnim, child.properties);
                    child.transform.SetParent(childPanelTrans, false);
                }
            }
            Reset();
        }

        /// <summary>
        ///显示界面
        /// </summary>
        public void Show()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
            }
            OnShow();
            _eventOnShow?.Invoke();
        }

        public void Reset()
        {
            OnReset();
        }

        public void UIUpdate()
        {
            OnUpdate();
        }

        public void UILateUpdate()
        {
            OnLateUpdate();
        }

        public void UIFixedUpdate() 
        {
            OnFixedUpdate();
        }

        /// <summary>
        ///隐藏界面
        /// </summary>
        public void Hide()
        {
            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
            }
            OnHide();
            EventOnHide?.Invoke();
        }

        /// <summary>
        /// Close the screen
        /// </summary>
        /// <param name="animate">Should animation be played? (defaults to true)</param>
        public void Close(bool animate = false)
        {
            if (isUpdate)
                RemoveUpdate();
            if (isLateUpdate)
                RemoveLateUpdate();
            if (isFixedUpdate)
                RemoveFixedUpdate();
            isUpdate = false;
            isLateUpdate = false;
            isFixedUpdate = false;
            DoAnimation(animate, OnTransitionOutFinished, false);
            foreach (var child in childPanels)
            {
                child.Close(animate);
            }
        }
        
        public void Clear()
        {
            GameObject.DestroyImmediate(gameObject);
        }

        private void OnDestroy()
        {
            if (_anim != null)
                _anim.Stop();
            if (isUpdate)
                RemoveUpdate();
            if (isLateUpdate)
                RemoveLateUpdate();
            if (isFixedUpdate)
                RemoveFixedUpdate();
            isUpdate = false;
            isLateUpdate = false;
            isFixedUpdate = false;
            OnClear();
            if (ScreenDestroyed != null)
            {
                ScreenDestroyed(this);
            }

            InTransitionBegin = null;
            OutTransitionBegin = null;
            InTransitionFinished = null;
            OutTransitionFinished = null;
            CloseRequest = null;
            ScreenDestroyed = null;
            childPanelTrans = null;
            RemoveListeners();
            StopAllCoroutines();
        }

        #endregion

        #region Public Function

        /// <summary>
        ///关闭界面
        /// </summary>
        public void CloseUI()
        {
            CloseRequest(this);
        }

        /// <summary>
        ///开启界面Update
        /// </summary>
        /// <param name="tick">几帧刷新一次</param>
        protected void AddUpdate(int tick = 0)
        {
            isUpdate = true;
            UiFrame.AddUpdate(this, tick);
        }

        /// <summary>
        ///开启界面LateUpdate
        /// </summary>
        /// <param name="tick">几帧刷新一次</param>
        protected void AddLateUpdate(int tick = 0)
        {
            isLateUpdate = true;
            UiFrame.AddLateUpdate(this, tick);
        }

        /// <summary>
        ///开启界面FixedUpdate
        /// </summary>
        /// <param name="tick">几帧刷新一次</param>
        protected void AddFixedUpdate(int tick = 0)
        {
            isFixedUpdate = true;
            UiFrame.AddFixedUpdate(this, tick);
        }

        /// <summary>
        ///关闭界面Update
        /// </summary>
        /// <param name="tick">几帧刷新一次</param>
        protected void RemoveUpdate()
        {
            UiFrame.RemoveUpdate(this);
        }

        /// <summary>
        ///关闭界面LateUpdate
        /// </summary>
        /// <param name="tick">几帧刷新一次</param>
        protected void RemoveLateUpdate()
        {
            UiFrame.RemoveLateUpdate(this);
        }

        /// <summary>
        ///关闭界面FixedUpdate
        /// </summary>
        /// <param name="tick">几帧刷新一次</param>
        protected void RemoveFixedUpdate()
        {
            UiFrame.RemoveFixedUpdate(this);
        }

        #region 子界面相关

        //给UI下面添加childPanel节点，用来存贮子界面
        public void SetChildPanelTrans(Transform trans)
        {
            if (childPanelTrans == null)
            {
                childPanelTrans = Instantiate<Transform>(trans);
                childPanelTrans.name = "ChildPanels";
                childPanelTrans.SetParent(transform, false);
                childPanelTrans.transform.SetAsLastSibling();
            }
        }

        public void AddChildPanel(IUIScreenController childCtl)
        {
            if (childPanelTrans == null)
            {
                SetChildPanelTrans(UiFrame.ChildPanelsTrans);
            }
            APanelController panelCtl = (APanelController)childCtl;
            if (panelCtl != null)
            {
                panelCtl.transform.SetParent(childPanelTrans, false);
                childCtl.HierarchyFixOnShow();
                panelCtl.ParentUI = this;
                childPanels.Add(panelCtl);
                //Debug.Log($"****** {transform.name} 添加子界面: {panelCtl.name}");
            }
        }

        public void ClearChildPanel(IUIScreenController childCtl)
        {
            APanelController panelCtl = (APanelController)childCtl;
            if (panelCtl != null)
            {
                //Debug.Log($"****** {transform.name} 删除子界面: {panelCtl.name}");
                childPanels.Remove(panelCtl);
            }
        }

        public void ClearAllChildPanel()
        {
            //Debug.Log($"****** {transform.name} 清空子界面！");
            var removeList = new List<APanelController>();
            foreach (var child in childPanels)
            {
                removeList.Add(child);
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                var child = removeList[i];
                //Debug.Log($"****** {transform.name} 开始删除子界面: {child.name}");
                child.CloseUI();
            }
            childPanels.Clear();
        }
        #endregion
        #endregion

        #region Override Function

        /// <summary>
        ///界面刷新
        /// </summary>
        protected virtual void AddMonoData()
        {
        }

        /// <summary>
        /// For setting up all the listeners for events/messages. By default, called on Awake()
        /// </summary>
        protected virtual void AddListeners()
        {
        }

        /// <summary>
        /// For removing all the listeners for events/messages. By default, called on OnDestroy()
        /// </summary>
        protected virtual void RemoveListeners()
        {
        }

        /// <summary>
        /// When Properties are set for this screen, this method is called.
        /// At this point, you can safely access Properties.
        /// </summary>
        protected virtual void OnPropertiesSet()
        {
        }

        /// <summary>
        /// When setting the properties, this method is called.
        /// This way, you can extend the usage of your properties by
        /// certain conditions.
        /// </summary>
        /// <param name="props">Properties.</param>
        protected virtual void SetProperties(TProps props)
        {
            properties = props;
        }

        /// <summary>
        /// In case your screen has any special behaviour to be called
        /// when the hierarchy is adjusted
        /// </summary>
        protected virtual void HierarchyFixOnShow()
        {
            transform.SetAsLastSibling();
        }

        #region 生命周期
        /// <summary>
        ///界面初始化 暂时禁用
        /// </summary>
        private void OnInit()
        {
            
        }

        protected virtual void OnOpen()
        {
            
        }

        protected virtual void OnShow()
        {

        }

        /// <summary>
        ///界面刷新
        /// </summary>
        protected virtual void OnReset()
        {

        }

        protected virtual void OnUpdate()
        {

        }

        protected virtual void OnLateUpdate()
        {

        }

        protected virtual void OnFixedUpdate()
        {

        }

        protected virtual void OnHide()
        {

        }

        protected virtual void OnClose()
        {

        }

        /// <summary>
        ///清除界面
        /// </summary>
        protected virtual void OnClear()
        {

        }

        #endregion
        #endregion

        #region Animation
        private void DoAnimation(bool animate, Action callWhenFinished, bool isFadeIn)
        {
            if (animate && ((isFadeIn && HasOpenAnim()) || (!isFadeIn && HasCloseAnim())))
            {
                StopAllCoroutines();
                if (isFadeIn)
                {
                    Show();
                    StartCoroutine(PlayOpenAnim(callWhenFinished));
                }
                else
                {
                    StartCoroutine(PlayCloseAnim(callWhenFinished));
                }              
            }
            else
            {
                if (isFadeIn)
                    Show();

                if (callWhenFinished != null)
                {
                    callWhenFinished();
                }
            }
        }

        public IEnumerator PlayOpenAnim(Action callWhenFinished = null)
        {
            //Debug.Log($"****** {transform.name} 播放入场动画");
            var state = _anim[AnimName_fadeIn];
            if (InTransitionBegin != null)
            {
                InTransitionBegin(this);
            }
            if (state != null && _anim.Play(AnimName_fadeIn, PlayMode.StopAll))
            {
                state.enabled = true;
                state.time = 0;
                _anim.Sample();
                state.enabled = false;
                yield return null;
                state.enabled = true;
                yield return new WaitForSeconds(state.length);
            }
            //Debug.Log($"****** {transform.name} 入场动画 播放完成");
            if (callWhenFinished != null)
            {
                callWhenFinished();
            }
        }

        public IEnumerator PlayCloseAnim(Action callWhenFinished = null)
        {
            //Debug.Log($"****** {transform.name} 播放出场动画");
            var state = _anim[AnimName_fadeOut];
            if (OutTransitionBegin != null)
            {
                OutTransitionBegin(this);
            }
            if (state != null && _anim.Play(AnimName_fadeOut, PlayMode.StopAll))
            {
                yield return new WaitForSeconds(state.length);
            }
           //Debug.Log($"****** {transform.name} 出场动画 播放完成");
            if (callWhenFinished != null)
            {
                callWhenFinished();
            }
        }

        private void OnTransitionInFinished()
        {
            if (InTransitionFinished != null)
            {
                InTransitionFinished(this);
            }
            if (HasIdleAnim())
                _anim.Play(AnimName_idle, PlayMode.StopAll);
        }

        private void OnTransitionOutFinished()
        {
            properties.Data = null;
            OnClose();
            EventOnClose?.Invoke();
            Hide();
            IsVisible = false;
            UiFrame.ReparentScreenToCache(this, transform);
            if (OutTransitionFinished != null)
            {
                OutTransitionFinished(this);
            }
        }

        public bool HasOpenAnim()
        {
            if (_anim == null) return false;
            var state = _anim[AnimName_fadeIn];
            return state == null ? false : state.length > 0;
        }

        public bool HasCloseAnim()
        {
            if (_anim == null) return false;
            var state = _anim[AnimName_fadeOut];
            return state == null ? false : state.length > 0;
        }

        public bool HasIdleAnim()
        {
            if (_anim == null) return false;
            var state = _anim[AnimName_idle];
            return state == null ? false : state.length >= 0;
        }
        #endregion
    }
}
