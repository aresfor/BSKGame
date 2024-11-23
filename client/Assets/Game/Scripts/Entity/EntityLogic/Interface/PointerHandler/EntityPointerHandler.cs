using System.Collections.Generic;
using Game.Core;
using GameFramework.Entity;

namespace Game.Client
{
    public class EntityPointerHandler:IPointerHandler
    {
        private IEntity m_Entity;
        public EntityPointerHandler(IEntity entity)
        {
            m_Entity = entity;
        }
        public bool PointerEnter()
        {
            bool bChildHasHandle = false;
            List<IEntity> results = ListPool<IEntity>.Get();
            GameEntry.Entity.GetChildEntities(m_Entity.Id, results);
            bChildHasHandle = results.Count > 0;
            foreach (var childEntity in results)
            {
                if (childEntity.LogicInterface is IPointerHandler pointerHandler)
                {
                    bool successHandle = pointerHandler.PointerEnter();
                    if (false == successHandle)
                        bChildHasHandle = false;
                }
            }
            results.Clear();
            ListPool<IEntity>.Release(results);
            
            return bChildHasHandle;
        }

        public bool PointerExit()
        {
            List<IEntity> results = ListPool<IEntity>.Get();
            GameEntry.Entity.GetChildEntities(m_Entity.Id, results);
            foreach (var childEntity in results)
            {
                if (childEntity.LogicInterface is IPointerHandler pointerHandler)
                {
                    pointerHandler.PointerExit();
                }
            }
            results.Clear();
            ListPool<IEntity>.Release(results);

            return true;
        }

        public bool PointerDown()
        {
            List<IEntity> results = ListPool<IEntity>.Get();
            GameEntry.Entity.GetChildEntities(m_Entity.Id, results);
            foreach (var childEntity in results)
            {
                if (childEntity.LogicInterface is IPointerHandler pointerHandler)
                {
                    pointerHandler.PointerDown();
                }
            }
            results.Clear();
            ListPool<IEntity>.Release(results);
            return true;
        }

        public bool PointerUp()
        {
            List<IEntity> results = ListPool<IEntity>.Get();
            GameEntry.Entity.GetChildEntities(m_Entity.Id, results);
            foreach (var childEntity in results)
            {
                if (childEntity.LogicInterface is IPointerHandler pointerHandler)
                {
                    pointerHandler.PointerUp();
                }
            }
            results.Clear();
            ListPool<IEntity>.Release(results);
            return true;
        }
    }
}