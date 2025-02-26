﻿using System.Collections.Generic;
using UnityEngine;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{

    [Description("Split the Flow in multiple directions. Calls all outputs in the same frame but in order")]
    [Name("Split", 90)]
    public class Split : FlowControlNode
    {

        [SerializeField, ExposeField]
        [GatherPortsCallback]
        [MinValue(2), DelayedField]
        private int _portCount = 4;
        
        protected override void RegisterPorts() {
            var outs = new List<FlowOutput>();
            for ( var i = 0; i < _portCount; i++ ) {
                outs.Add(AddFlowOutput(i.ToString()));
            }
            AddFlowInput("In", (f) =>
            {
                for ( var i = 0; i < _portCount; i++ ) {
                    outs[i].Call(f);
                }
            });
        }
        // protected override void RegisterPorts() {
        //     var outs = new List<FlowOutput>();
        //     for ( var i = 0; i < _portCount; i++ ) {
        //         outs.Add(AddFlowOutput(i.ToString()));
        //     }
        //     AddFlowInput("In", (f) =>
        //     {
        //         IntItem item = f.ReadParameter<DataItem>("Depth") as IntItem;
        //         for ( var i = 0; i < _portCount; i++ ) {
        //             if (item!=null && item!=default)
        //             {
        //                 f.WriteParameter("Depth", new IntItem(item.GetVariable() + 1));
        //             }
        //             outs[i].Call(f);
        //         }
        //     });
        // }
    }
}