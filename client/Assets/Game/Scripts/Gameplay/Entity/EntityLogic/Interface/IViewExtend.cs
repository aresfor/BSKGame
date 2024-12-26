using UnityEngine;

namespace Game.Client
{
    public interface IViewExtend
    {
        const string MainMeshName = "Model"; 
        Transform GetTransform(string tsName);
        Transform GetLogicTransform();

        Transform GetMainMesh();
    }
}