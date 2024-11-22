using UnityEngine;

namespace Game.Client
{
    /// <summary>
    /// 继承，实现从不同类型的transform身上查找socket
    /// </summary>
    public class SocketFindFactory
    {
        protected TransformSocketMap m_TsSocketMap;

        public virtual void SetTransformSocketMap(TransformSocketMap map)
        {
            m_TsSocketMap = map;
        }
        public virtual Transform FindSocket(string socketName)
        {
            if (m_TsSocketMap != null && m_TsSocketMap.m_SocketMap.ContainsKey(socketName))
            {
                return m_TsSocketMap.m_SocketMap[socketName];
            }
            if (m_TsSocketMap == null)
            {
                Debug.LogWarning($"[Socket] FindSocket, m_TsSocketMap == null, socketName = {socketName}");
            }
            else
            {
                Debug.LogWarningFormat("[Socket] FindSocket, m_TsSocketMap.m_SocketMap = {0} doesn't contains socketName = {1}", m_TsSocketMap.m_SocketMap, socketName);
            }

            return null;
        }
    }
}