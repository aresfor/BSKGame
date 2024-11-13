using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.UIFramework {
    /// <summary>
    /// This Layer controls Panels.
    /// Panels are Screens that have no history or queuing,
    /// they are simply shown and hidden in the Frame
    /// eg: a HUD, an energy bar, a mini map etc.
    /// </summary>
    public class PanelUILayer : AUILayer<IPanelController> {
        [SerializeField]
        [Tooltip("Settings for the priority para-layers. A Panel registered to this layer will be reparented to a different para-layer object depending on its Priority.")]
        private PanelPriorityLayerList priorityLayers = null;

        public override void ReparentScreen(IUIScreenController controller, Transform screenTransform) {
            var ctl = controller as IPanelController;
            if (ctl != null) {
                ReparentToParaLayer(ctl.Priority, screenTransform);
            }
            else {
                base.ReparentScreen(controller, screenTransform);
            }
        }

        public override void OpenScreen(IPanelController screen, bool isImmediate = false) {
            screen.Open(true);
        }

        public override void OpenScreen<TProps>(IPanelController screen, TProps properties, bool isImmediate = false) {
            screen.Open(true, properties);
        }

        public override void CloseScreen(IPanelController screen) {
            if (screen.IsVisible)
            {
                base.CloseScreen(screen);
                screen.Close(true);
            }
        }

        public bool IsPanelVisible(string panelId) {
            IPanelController panel;
            if (registeredScreens.TryGetValue(panelId, out panel)) {
                return panel.IsVisible;
            }

            return false;
        }
        
        private void ReparentToParaLayer(PanelPriority priority, Transform screenTransform) {
            Transform trans;
            if (!priorityLayers.ParaLayerLookup.TryGetValue(priority, out trans)) {
                trans = transform;
            }
            
            screenTransform.SetParent(trans, false);
        }
    }
}
