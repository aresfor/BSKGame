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
        public bool PointerEnter(FPointerEventData eventData)
        {
            bool bChildHasHandle = false;
            List<IEntity> results = ListPool<IEntity>.Get();
            GameEntry.Entity.GetChildEntities(m_Entity.Id, results);
            bChildHasHandle = results.Count > 0;
            foreach (var childEntity in results)
            {
                if (childEntity.LogicInterface is IPointerHandler pointerHandler)
                {
                    bool successHandle = pointerHandler.PointerEnter(eventData);
                    if (false == successHandle)
                        bChildHasHandle = false;
                }
            }
            results.Clear();
            ListPool<IEntity>.Release(results);
            
            return bChildHasHandle;
        }

        public bool PointerExit(FPointerEventData eventData)
        {
            List<IEntity> results = ListPool<IEntity>.Get();
            GameEntry.Entity.GetChildEntities(m_Entity.Id, results);
            foreach (var childEntity in results)
            {
                if (childEntity.LogicInterface is IPointerHandler pointerHandler)
                {
                    pointerHandler.PointerExit(eventData);
                }
            }
            results.Clear();
            ListPool<IEntity>.Release(results);

            return true;
        }

        public bool PointerDown(FPointerEventData eventData)
        {
            List<IEntity> results = ListPool<IEntity>.Get();
            GameEntry.Entity.GetChildEntities(m_Entity.Id, results);
            foreach (var childEntity in results)
            {
                if (childEntity.LogicInterface is IPointerHandler pointerHandler)
                {
                    pointerHandler.PointerDown(eventData);
                }
            }
            results.Clear();
            ListPool<IEntity>.Release(results);
            return true;
        }

        public bool PointerUp(FPointerEventData eventData)
        {
            List<IEntity> results = ListPool<IEntity>.Get();
            GameEntry.Entity.GetChildEntities(m_Entity.Id, results);
            foreach (var childEntity in results)
            {
                if (childEntity.LogicInterface is IPointerHandler pointerHandler)
                {
                    pointerHandler.PointerUp(eventData);
                }
            }
            results.Clear();
            ListPool<IEntity>.Release(results);
            return true;
        }
    }
}