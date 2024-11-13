using UnityEngine;

namespace Game.UIFramework {
    /// <summary>
    /// Properties common to all panels
    /// </summary>
    [System.Serializable] 
    public class PanelProperties : IPanelProperties {
        [SerializeField] 
        [Tooltip("Panels go to different para-layers depending on their priority. You can set up para-layers in the Panel Layer.")]
        private PanelPriority priority;

        public PanelPriority Priority {
            get { return priority; }
            set { priority = value; }
        }

        [SerializeField]
        protected bool alwaysPlayAnim = false;
        public bool AlwaysPlayAnim
        {
            get { return alwaysPlayAnim; }
            set { alwaysPlayAnim = value; }
        }

        private IParameterData data;
        public IParameterData Data
        {
            get { return data; }
            set { data = value; }
        }

        public PanelProperties(PanelPriority priority, bool alwaysPlay = false, IParameterData param = null)
        {
            Priority = priority;
            AlwaysPlayAnim = alwaysPlay;
            Data = param;
        }
    }
}
