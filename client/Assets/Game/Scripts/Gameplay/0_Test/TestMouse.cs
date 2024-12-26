using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class TestMouse:MonoBehaviour
    {
        private void OnMouseEnter()
        {
            Log.Error("MouseEnter");
        }

        private void OnMouseExit()
        {
            Log.Error("MouseExit");
        }
    }
}