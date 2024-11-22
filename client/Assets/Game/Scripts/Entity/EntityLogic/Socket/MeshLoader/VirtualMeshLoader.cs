

using UnityGameFramework.Runtime;

namespace Game.Client
{
    public class VirtualMeshLoader
    {
        protected string virtualMeshName;
        protected SocketFindFactory transformSocketFinder;
        protected Entity entity;

        public VirtualMeshLoader(Entity inEntity
            , string inVirtualMeshName
            , SocketFindFactory inSocketFindFactory)
        {
            entity = inEntity;
            virtualMeshName = inVirtualMeshName;
            transformSocketFinder = inSocketFindFactory;
        }

        public void BeginLoadVirtualMesh()
        {
            
        }
    }

}
 

