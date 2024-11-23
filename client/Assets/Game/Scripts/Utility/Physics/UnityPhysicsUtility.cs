using System;
using System.Collections.Generic;
using Game.Core;
using Game.Gameplay;
using Game.Math;
using UnityEngine;
using UnityGameFramework.Runtime;
using Log = Game.Core.Log;

namespace Game.Client
{
    public class UnityPhysicsUtility:IPhysicsUtility
    {
        public bool SingleLineCheck(float3 startTrace, float3 endTrace, int traceFlag,
            ref ImpactInfo impactInfo)
        {
            float3 direction = (endTrace - startTrace);
            float distance = math.length(direction);
            return SingleLineCheck(startTrace, direction, distance, traceFlag, ref impactInfo);
        }

        public bool SingleLineCheck(float3 startTrace, float3 direction, float distance, int traceFlag,
            ref ImpactInfo impactInfo, bool bDrawLine = false, DebugColor debugColor = DebugColor.kColorGreen,
            float duration = 5)
        {
            int layerMask = GetLayerMask(traceFlag);

            if (distance > 0 && Physics.Raycast(startTrace.ToVector3(), direction.ToVector3(), out var hitInfo,
                    distance, layerMask))
            {
                if (hitInfo.collider != null)
                {
                    impactInfo = ImpactInfo.Alloc();

                    impactInfo.HitLocation = hitInfo.point.ToFloat3();
                    impactInfo.HitObjPosition = hitInfo.collider.transform.position.ToFloat3();
                    impactInfo.HitNormal = hitInfo.normal.ToFloat3();
                    impactInfo.RayDir = direction;
                    impactInfo.StartPosition = startTrace;
                    impactInfo.ActorLayer = UnityLayersUtil.LayerNameToLayerType(hitInfo.collider.gameObject.layer);
                    impactInfo.Distance = hitInfo.distance;
                    impactInfo.HitGroup = EHitGroup.Default;
                    impactInfo.InstanceId = hitInfo.collider.GetInstanceID();
                    impactInfo.materialId = GetColliderPhysicalMaterialID(hitInfo.collider.sharedMaterial);
                    if (impactInfo.ActorLayer != LayerType.kTypeStaticActor)
                    {
                        var entity = hitInfo.collider.GetComponent<Entity>();
                        if (entity != null)
                        {
                            //if (entity.OwnerEntityActor != null && entity.OwnerEntityActor.OwnerEntity != null)
                            //{
                                impactInfo.HitEntityId = entity.Id;

#if !SHIPPING_EXTERNAL
                                // Log.Debug($"[Physics]SingleLineCheck {hitInfo.transform.name} Hit :{impactInfo.HitEntityId}");                  
#endif 
                            //}
                            //else
                            //{
                            //    Log.Error(
                            //        $"[Physics]SingleLineCheck {hitInfo.transform.name},get ColliderActorBinder ,but ownerEntityActor or OwnerEntity is null ,please check!");
                            //}
                        }
                    }
#if UNITY_EDITOR
                    if (bDrawLine)
                    {
                        DebugTool.DrawLine(startTrace.ToVector3(), hitInfo.point, debugColor, duration);
                        DebugTool.DrawGizmosSphere(hitInfo.point, 0.1f, Color.red, duration);
                    }
#endif
                    return true;
                }

                return false;
            }
            else
            {
                impactInfo = null;

                return false;
            }
        }

        public bool MultiLineCheck(float3 startTrace, float3 direction, float distance, int traceFlag,
            ref List<ImpactInfo> impactList, bool blockByStaticActor = false)
        {
            impactList ??= new List<ImpactInfo>();
            impactList.Clear();
            if (distance <= 0)
            {
                return false;
            }

            float forwardLength = 0.0001f;
            Vector3 forwardDelta = direction.ToVector3().normalized * forwardLength;
            int layerMask = GetLayerMask(traceFlag);
            RaycastHit hitInfo;
            float startDistance = 0;
            float totalDistance = distance;

            Collider lastHitCollider = null;
            int sameColliderHitCount = 0;

            Vector3 startTraceVec = startTrace.ToVector3();
            while (Physics.Raycast(startTraceVec, direction.ToVector3(), out hitInfo, distance, layerMask))
            {
                ImpactInfo impactInfo = ImpactInfo.Alloc();

                impactInfo.HitLocation = hitInfo.point.ToFloat3();
                impactInfo.HitObjPosition = hitInfo.collider.transform.position.ToFloat3();
                impactInfo.HitNormal = hitInfo.normal.ToFloat3();
                impactInfo.RayDir = direction;
                impactInfo.StartPosition = startTrace;
                impactInfo.ActorLayer = UnityLayersUtil.LayerNameToLayerType(hitInfo.collider.gameObject.layer);
                impactInfo.Distance = hitInfo.distance;
                impactInfo.materialId = GetColliderPhysicalMaterialID(hitInfo.collider.sharedMaterial);
                impactList.Add(impactInfo);
                startTraceVec = hitInfo.point + forwardDelta;
                startDistance += hitInfo.distance + forwardLength;
                distance = totalDistance - startDistance;

                if (impactInfo.ActorLayer != LayerType.kTypeStaticActor)
                {
                    var entity = hitInfo.collider.GetComponent<Entity>();
                    if (entity != null)
                    {
                        //if (entity.OwnerEntityActor != null && entity.OwnerEntityActor.OwnerEntity != null)
                        //{
                        impactInfo.HitEntityId = entity.Id;
                        //}
                        //else
                        //{
                        //    Log.Error(
                        //        $"[Physics]single line check {hitInfo.transform.name},get ColliderActorBinder ,but ownerEntityActor or OwnerEntity is null ,please check!");
                        //}
                    }
                }

                if (blockByStaticActor && UnityLayersUtil.IsStaticActor(hitInfo.collider.gameObject.layer))
                {
                    break;
                }

                if (lastHitCollider == hitInfo.collider)
                {
                    sameColliderHitCount++;
                    if (sameColliderHitCount > 10 &&
                        Vector3.Dot(direction.ToVector3().normalized, impactInfo.HitNormal.ToVector3()) < 0.00001f)
                        break;
                }
                else
                {
                    sameColliderHitCount = 0;
                }

                lastHitCollider = hitInfo.collider;
#if UNITY_EDITOR
                // DebugTool.DrawLine(startTrace.ToVector3(),
                //     startTrace.ToVector3() + direction.ToVector3() * hitInfo.distance, Color.red, 1);
#endif
            }

            return impactList.Count > 0;
        }


        public bool SphereCheck(float3 startTrace, float3 endTrace, float radius, int traceFlag,
            ref float3 hitPosition)
        {
            float3 hitNormal = float3.zero;
            bool result = SphereCheck(startTrace, endTrace, radius, traceFlag, ref hitPosition, ref hitNormal);
            if (result)
            {
                hitPosition += math.normalizesafe(hitNormal) * radius;
            }

            return result;
        }

        public bool SphereCheck(float3 startTrace, float3 endTrace, float radius, int traceFlag,
            ref float3 hitPosition, ref float3 hitNormal)
        {
            float3 direction = (endTrace - startTrace);
            float distance = math.length(direction);
            return SphereCheck(startTrace, direction, radius, distance, traceFlag, ref hitPosition,
                ref hitNormal);
        }

        public bool SphereCheck(float3 startTrace, float3 direction, float radius, float distance,
            int traceFlag, ref float3 hitPosition, ref float3 hitNormal)
        {
            int layerMask = GetLayerMask(traceFlag);
            if (distance > 0 && Physics.SphereCast(startTrace.ToVector3(), radius, direction.ToVector3(),
                    out var hitInfo, distance, layerMask))
            {
                if (hitInfo.collider != null)
                {
                    hitPosition = hitInfo.point.ToFloat3();
                    hitNormal = hitInfo.normal.ToFloat3();
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public bool SphereCheck(float3 startTrace, float3 direction, float radius, float distance,
            int traceFlag, ref ImpactInfo impactInfo)
        {
            int layerMask = GetLayerMask(traceFlag);
            Vector3 spos = startTrace.ToVector3();
            Vector3 endpos = startTrace.ToVector3() + direction.ToVector3() * distance;
            //DebugTool.DrawLine(spos, endpos, Color.red, 1);         
            if (distance > 0 && Physics.SphereCast(startTrace.ToVector3(), radius, direction.ToVector3(),
                    out var hitInfo, distance, layerMask))
            {
                if (hitInfo.collider != null)
                {
                    //DebugTool.DrawLine(spos, endpos, Color.yellow, 1);
                    impactInfo = ImpactInfo.Alloc();
                    impactInfo.HitLocation = hitInfo.point.ToFloat3();
                    impactInfo.HitObjPosition = hitInfo.collider.transform.position.ToFloat3();
                    impactInfo.HitNormal = hitInfo.normal.ToFloat3();
                    impactInfo.RayDir = direction;
                    impactInfo.StartPosition = startTrace;
                    impactInfo.ActorLayer = UnityLayersUtil.LayerNameToLayerType(hitInfo.collider.gameObject.layer);
                    impactInfo.Distance = hitInfo.distance;
                    impactInfo.HitGroup = EHitGroup.Default;
                    impactInfo.materialId = GetColliderPhysicalMaterialID(hitInfo.collider.sharedMaterial);

                    return true;
                }

                return false;
            }
            else
            {
                impactInfo = null;
                return false;
            }
        }

        #region PhysicsSwap NonAlloc (暂时保留，考虑移除)

        public int OverlapSphereNonAlloc(float3 startPos, float radius, int traceFlag,
            ref List<ImpactInfo> impactList, int maxCount)
        {
            impactList.Clear();
            int LayerMask = GetLayerMask(traceFlag);
            Vector3 startPos_Vector3 = new Vector3(startPos.x, startPos.y, startPos.z);

            Collider[] colliders = new Collider[maxCount];
            int size = Physics.OverlapSphereNonAlloc(startPos_Vector3, radius, colliders, LayerMask);

            for (int i = 0; i < size; i++)
            {
                Collider curCollider = colliders[i];

                //PS: OverLap无法完全填充完整个结构体数据 只能填充关键部分 缺少HitLocation HitNormal RayDir ThroughWallCount ThroughPlayerCount HitGroup
                ImpactInfo impactInfo = ImpactInfo.Alloc();

                impactInfo.HitObjPosition = curCollider.transform.position.ToFloat3();
                impactInfo.StartPosition = startPos;
                impactInfo.Distance = Vector3.Distance(startPos_Vector3, curCollider.transform.position);
                impactInfo.ActorLayer = UnityLayersUtil.LayerNameToLayerType(curCollider.gameObject.layer);
                impactInfo.HitGroup = EHitGroup.Default;
                impactInfo.InstanceId = curCollider.GetInstanceID();
                impactInfo.materialId = GetColliderPhysicalMaterialID(curCollider.sharedMaterial);
                Log.Debug("[Physics]OverlapSphereNonAlloc hitCollider:" + curCollider.name);

                if (impactInfo.ActorLayer != LayerType.kTypeStaticActor)
                {
                    var entity = curCollider.GetComponent<Entity>();
                    //if (entity != null)
                    //{
                      //  if (entity.OwnerEntityActor != null && entity.OwnerEntityActor.OwnerEntity != null)
                        //{
                        impactInfo.HitEntityId = entity.Id;
                        //}
                        //else
                        //{
                        //   Log.Error(
                        //        $"[Physics]OverlapSphereNonAlloc {curCollider.transform.name},get ColliderActorBinder ,but ownerEntityActor or OwnerEntity is null ,please check!");
                        //}
                    //}
                }

                impactList.Add(impactInfo);
            }

            return size;
        }

        public int BoxCastNonAlloc(float3 startPos, float3 halfExtents, float3 direction,
            quaternion orientation, float maxDistance,
            int traceFlag, ref List<ImpactInfo> impactList, int maxCheckCount,bool bIsIgnoreDetailCheck = false)
        {
            impactList.Clear();

            int LayerMask = GetLayerMask(traceFlag);
            RaycastHit[] hitResults = new RaycastHit[maxCheckCount];

            int size = Physics.BoxCastNonAlloc(new Vector3(startPos.x, startPos.y, startPos.z),
                new Vector3(halfExtents.x, halfExtents.y, halfExtents.z),
                new Vector3(direction.x, direction.y, direction.z), hitResults,
                orientation.ToQuaternion(),
                maxDistance, LayerMask);

            Debug.DrawLine(startPos.ToVector3(),startPos.ToVector3() + direction.ToVector3() * maxDistance,Color.yellow,1.0f);

            for (int i = 0; i < size; i++)
            {
                RaycastHit hitInfo = hitResults[i];
                if (hitInfo.collider != null)
                {
                    //var tmpHitTypeMask = HitTypeMask.Tag2HitMask(hitInfo.collider.tag);  
                    //或，因为其他运算中可能会附有其他mask

                    ImpactInfo impactInfo = ImpactInfo.Alloc();
                    
                    impactInfo.HitLocation = hitInfo.point.ToFloat3();
                    impactInfo.HitObjPosition = hitInfo.collider.transform.position.ToFloat3();
                    impactInfo.HitNormal = hitInfo.normal.ToFloat3();
                    impactInfo.RayDir = direction;
                    impactInfo.StartPosition = startPos;
                    impactInfo.ActorLayer = UnityLayersUtil.LayerNameToLayerType(hitInfo.collider.gameObject.layer);
                    impactInfo.Distance = hitInfo.distance;
                    impactInfo.HitGroup = EHitGroup.Default;
                    impactInfo.InstanceId = hitInfo.collider.GetInstanceID();
                    impactInfo.materialId = GetColliderPhysicalMaterialID(hitInfo.collider.sharedMaterial);

                    // Log.Debug("[PhysicsService]HitCollider:" + hitInfo.collider.name);
                    bool bIsCheckValid = true;
                    if (impactInfo.ActorLayer != LayerType.kTypeStaticActor)
                    {
                        var entity = hitInfo.collider.GetComponent<Entity>();
                        if (entity != null)
                        {
                            //if (entity.OwnerEntityActor != null && entity.OwnerEntityActor.OwnerEntity != null)
                            //{
                                impactInfo.HitEntityId = entity.Id;
                                if (!bIsIgnoreDetailCheck)
                                {
                                    //直接进行细节检测 从命中点位开始 但是如果出现一开始就碰撞体里面，则使用StartPosition
                                    Vector3 detailStart = startPos.ToVector3();
                                    if (detailStart.magnitude < 0.01f)
                                    {
                                        detailStart = startPos.ToVector3() - direction.ToVector3() * 1.0f;
                                    }
                                    // detailStart = detailStart - direction.ToVector3() * 2.0f;
                                    // DrawGizmos.Instance.DrawSphereGizmos(hitInfo.point.ToFloat3(), 0.2f, Color.magenta, 1.0f);
                                    bIsCheckValid = BoxCheckNoPhysics(impactInfo.HitEntityId,detailStart.ToFloat3(),halfExtents,direction,50.0f,traceFlag,ref impactInfo);
                                    if (!bIsCheckValid)
                                    {
                                        Log.Debug("[PhysicsService]CheckFail:" + hitInfo.collider.name + " Entity:" + impactInfo.HitEntityId);
                                    }
                                }
                            //}
                            //else
                            //{
                            //    Log.Error(
                            //        $"[Physics]BoxCastNonAlloc {hitInfo.transform.name},get ColliderActorBinder ,but ownerEntityActor or OwnerEntity is null ,please check!");
                            //}
                        }
                    }

                    if (bIsCheckValid)
                    {
                        impactList.Add(impactInfo);
                    }
                }
                
            }

            return impactList.Count;
        }

        #endregion

        #region NoPhysics

        static NotPhyHitResult bestResult = new NotPhyHitResult();
        static NotPhyHitResult tempResult = new NotPhyHitResult();

        public bool BoxCheckNoPhysics(int detailEntityId, float3 boxPosition, float3 boxExtend,
            float3 direction, float distance,
            int traceFlag, ref ImpactInfo impactInfo)
        {
            return false;
            
            // Entity detailEntity = GameEntry.Entity.GetEntity(detailEntityId);
            // if (detailEntity != null)
            // {
            //     IBeAttackAbleColliderDetailHandler detailHandler = world.GetEntityActor<EntityActor>(detailEntity) as IBeAttackAbleColliderDetailHandler;
            //     //实现细节检测的以细节为准
            //     if (detailHandler != null)
            //     {
            //         //使用列表搜集所有命中结果，寻找最近的元素
            //         List<ImpactInfo> impactInfos = ListPoolManager.Instance.Get<List<ImpactInfo>>();
            //         //TIPs:调用AA的函数会导致这个ImpactInfo被回收！！！！！！
            //         ImpactInfo TargetImpactInfo = ImpactInfo.Alloc();
            //         //首先使用中心射线（优先级最高）
            //         if (detailHandler.SingleLineCheckDetails(world,detailEntityId,boxPosition,direction,distance,traceFlag,ref TargetImpactInfo))
            //         {
            //             impactInfos.Add(TargetImpactInfo);
            //         }
            //         
            //         //其余射线谁先命中就以谁的为准
            //         List<float3> Vertices = GetDirectionVerticesByBox(boxPosition,boxExtend,direction);
            //         for (int i = 0; i < Vertices.Count; i++)
            //         {
            //             TargetImpactInfo = ImpactInfo.Alloc();
            //             Debug.DrawLine(Vertices[i].ToVector3(),(Vertices[i] + direction * distance).ToVector3(), Color.blue,1.0f);
            //             if (detailHandler.SingleLineCheckDetails(world,detailEntityId,Vertices[i],direction,distance,traceFlag,ref TargetImpactInfo))
            //             {
            //                 impactInfos.Add(TargetImpactInfo);
            //             }
            //         }
            //
            //         if (impactInfos.Count < 1)
            //         {
            //             ListPoolManager.Instance.Recycle(Vertices);
            //             for (int i = 0; i < impactInfos.Count; i++)
            //             {
            //                 ImpactInfo.Recycle(impactInfos[i]);
            //             }
            //             ListPoolManager.Instance.Recycle(impactInfos);
            //
            //             return false;
            //         }
            //         else
            //         {
            //             //找到Distance最近的物体
            //             impactInfos.Sort((a,b) => a.Distance.CompareTo(b.Distance));
            //             impactInfo = impactInfos[0];
            //             
            //             ListPoolManager.Instance.Recycle(Vertices);
            //             for (int i = 1; i < impactInfos.Count; i++)
            //             {
            //                 ImpactInfo.Recycle(impactInfos[i]);
            //             }
            //             ListPoolManager.Instance.Recycle(impactInfos);
            //             return true;
            //         }
            //             
            //     }
            //     else//没有实现的直接按默认返回即可
            //     {
            //         return true;
            //     }
            // }
            //
            // return false;
        }
        
        /// 获取box朝向Direction上的四个顶点
        private List<float3> GetDirectionVerticesByBox(float3 boxCenter,float3 boxExtend,float3 direction)
        {
            List<float3> AllVertices = Game.Core.ListPool<float3>.Get();
            AllVertices.Add(boxCenter + new float3(-boxExtend.x, -boxExtend.y, -boxExtend.z));
            AllVertices.Add(boxCenter + new float3(boxExtend.x, -boxExtend.y, -boxExtend.z));
            AllVertices.Add(boxCenter + new float3(-boxExtend.x, boxExtend.y, -boxExtend.z));
            AllVertices.Add(boxCenter + new float3(boxExtend.x, boxExtend.y, -boxExtend.z));
            AllVertices.Add(boxCenter + new float3(-boxExtend.x, -boxExtend.y, boxExtend.z));
            AllVertices.Add(boxCenter + new float3(boxExtend.x, -boxExtend.y, boxExtend.z));
            AllVertices.Add(boxCenter + new float3(-boxExtend.x, boxExtend.y, boxExtend.z));
            AllVertices.Add(boxCenter + new float3(boxExtend.x, boxExtend.y, boxExtend.z));

            //筛选指定朝向的顶点
            List<float3> realPoint = ListPool<float3>.Get();
            for (int i = 0; i < AllVertices.Count; i++)
            {
                if (math.dot(AllVertices[i] - boxCenter,direction) > 0)
                {
                    realPoint.Add(AllVertices[i]);
                }
            }
            //回收
            AllVertices.Clear();
            Game.Core.ListPool<float3>.Release(AllVertices);
            return realPoint;
        }


        public bool SingleLineCheckNoPhysics(int detailEntityID, float3 startTrace, float3 direction,
            float distance, int traceFlag, ref ImpactInfo impactInfo)
        {
            return false;
            // try
            // {
            //     Entity ownerEntity = world.GetEntity(detailEntityID);
            //     if (ownerEntity != null)
            //     {
            //         IBeAttackAbleColliderDetailHandler detailHandler =
            //             world.GetEntityActor<EntityActor>(ownerEntity) as IBeAttackAbleColliderDetailHandler;
            //         if (detailHandler != null)
            //         {
            //             impactInfo = ImpactInfo.Alloc();
            //             return detailHandler.SingleLineCheckDetails(detailEntityID, startTrace, direction,
            //                 distance, traceFlag, ref impactInfo); 
            //             //拓展Socket  var detailColliders = EntityColliderServiceMgr(world).GetColliderDatas(pawnEntityID);
            //         }
            //     }
            //
            //     return false;
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     throw;
            // }
        }

        public bool SingleLineCheckNoPhysics(int pawnEntityID, float3 startTrace, float3 direction,
            float distance,
            int traceFlag, ref ImpactInfo impactInfo, bool bDrawLine = false, DebugColor color = DebugColor.kColorGray,
            float duration = 5)
        {
            return SingleLineCheckNoPhysics(pawnEntityID, startTrace, direction, distance, traceFlag,
                ref impactInfo);
            //TODO Draw
        }

        //TODO colliders Recycle
        public bool SingleLineCheckDetailNoPhysics(List<Collider> detailColliders, int detailEntityID,
            float3 startTrace, float3 direction, float distance, int traceFlag, ref ImpactInfo impactInfo)
        {
            bool hasHit = false;
            var startTraceV3 = startTrace.ToVector3();
            var directionV3 = direction.ToVector3();

            for (int i = 0; i < detailColliders.Count; ++i)
            {
                var coll = detailColliders[i];
                if (coll == null|| !coll.gameObject.activeSelf||!coll.enabled || !UnityLayersUtil.IsFireActor(coll.gameObject.layer))
                {
                    continue;
                }

                tempResult.Reset();

                bool bHitTarget = false;
                if (coll is CapsuleCollider capsuleCollider)
                {
                    bHitTarget = RaycastCapsuleNoPhysic(capsuleCollider, capsuleCollider.transform, startTraceV3,
                        startTraceV3 + directionV3 * distance, ref tempResult);
                }
                else if (coll is BoxCollider boxCollider)
                {
                    bHitTarget = RaycastBoxNoPhysic(boxCollider, boxCollider.transform, startTraceV3,
                        startTraceV3 + directionV3 * distance, ref tempResult);
                }

                if (bHitTarget)
                {
                    var tmpHitGroup = HitGroupName.NameToEnum(coll.gameObject.name); //Check
                    //var tmpHitTypeMask = HitTypeMask.Tag2HitMask(coll.tag);
                    // Log.Debug($"-----[Fire][Details][CheckName2Layer]:{detailEntityID},collider:{coll.gameObject.name} 2 {tmpHitGroup}");
                    //if (!hasHit || tempResult.Distance < bestResult.Distance)//距离优先方案
                    //ignoreDistance
                    if (!hasHit  || ((tempResult.Distance < bestResult.Distance))) //包围盒子弱点优先
                    {
                        //或，因为其他运算中可能会附有其他mask
                        impactInfo.HitTypeMask = 0;
                        //impactInfo.HitTypeMask |= tmpHitTypeMask;
                        bestResult = tempResult;
                        impactInfo.RayDir = direction; //impactInfo NULL ??
                        impactInfo.StartPosition = startTrace;
                        impactInfo.ActorLayer = UnityLayersUtil.LayerNameToLayerType(coll.gameObject.layer);
                        impactInfo.HitGroup = tmpHitGroup;
                        impactInfo.HitEntityId = detailEntityID;
                        impactInfo.materialId = GetColliderPhysicalMaterialID(coll.sharedMaterial);
                        
                    }

                    if (impactInfo.HitGroup != EHitGroup.Default)
                    {
                        hasHit = true;
                    }
                }
            }

            if (hasHit)
            {
                impactInfo.Distance = bestResult.Distance;
                impactInfo.HitLocation = bestResult.HitLocation;
                impactInfo.HitNormal = bestResult.HitNormal;
                impactInfo.RayDir = direction;
                impactInfo.StartPosition = startTrace;
#if UNITY_EDITOR
                // Debug.DrawLine(impactInfo.HitLocation.ToVector3(),
                //     impactInfo.HitLocation.ToVector3() + Vector3.up * 0.2f, Color.blue, 4);
#endif
            }
            else
            {
                ImpactInfo.Recycle(impactInfo);
                impactInfo = null;
            }

            return hasHit;
        }


        #region NoPhysics Impl

        private bool RaycastCapsuleNoPhysic(CapsuleCollider coll, Transform trans, Vector3 start,
            Vector3 end, ref NotPhyHitResult notPhyHitResult)
        {
            const float kMinSize = 0.00001F;

            // Calc scale
            Vector3 scale = trans.lossyScale;

            float absoluteHeight = Mathf.Max(Mathf.Abs(coll.height * scale.y), 0);
            float absoluteRadius = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.z)) * coll.radius;
            float cylinderHeight = absoluteHeight - absoluteRadius * 2.0F;
            cylinderHeight = Mathf.Max(cylinderHeight, kMinSize);
            absoluteRadius = Mathf.Max(absoluteRadius, kMinSize);

            Vector3 center = trans.TransformPoint(coll.center);

            Quaternion rot = trans.rotation;

            // 0, 1 or 2 corresponding to the X, Y and Z axes
            switch (coll.direction)
            {
                case 0:
                    rot *= Quaternion.AngleAxis(90, Vector3.up);
                    break;
                case 1:
                    rot *= Quaternion.AngleAxis(90, Vector3.right);
                    break;
                case 2:
                    break;
            }

            return RaycastCapsuleNoPhysicImpl(center.ToFloat3(), rot.ToQuaternion(), cylinderHeight,
                absoluteRadius, start.ToFloat3(), end.ToFloat3(), ref notPhyHitResult);
        }

        public bool RaycastCapsuleNoPhysicImpl(float3 center, quaternion rot, float height, float Radius,
            float3 Start, float3 End, ref NotPhyHitResult notPhyHitResult)
        {
            //DebugTool.DrawLine(Start, End, Color.red);
            // DebugTool.DrawCapsule(center + rot * Vector3.forward * height / 2, center - rot * Vector3.forward * height / 2, Radius, Color.green);
            const float KINDA_SMALL_NUMBER = (1e-4f);
            Quaternion unityrot = rot.ToQuaternion();
            Quaternion inverseRot = Quaternion.Inverse(unityrot);

            Vector3 LocalStart = inverseRot * (Start.ToVector3() - center.ToVector3());
            Vector3 LocalEnd = inverseRot * (End.ToVector3() - center.ToVector3());
            float HalfHeight = height * 0.5f;

            // bools indicate if line enters different areas of sphyl
            bool testTop = false;
            bool testMiddle = false;
            bool testBottom = false;
            if (LocalStart.z >= HalfHeight) // start in top
            {
                testTop = true;

                if (LocalEnd.z < HalfHeight)
                {
                    testMiddle = true;

                    if (LocalEnd.z < -HalfHeight)
                        testBottom = true;
                }
            }
            else if (LocalStart.z >= -HalfHeight) // start in middle
            {
                testMiddle = true;

                if (LocalEnd.z >= HalfHeight)
                    testTop = true;

                if (LocalEnd.z < -HalfHeight)
                    testBottom = true;
            }
            else // start at bottom
            {
                testBottom = true;

                if (LocalEnd.z >= -HalfHeight)
                {
                    testMiddle = true;

                    if (LocalEnd.z >= HalfHeight)
                        testTop = true;
                }
            }

            // find line in terms of point and unit direction vector
            Vector3 RayDir = LocalEnd - LocalStart;
            float RayLength = RayDir.magnitude;
            float RecipRayLength = (1.0f / RayLength);
            if (RayLength > KINDA_SMALL_NUMBER)
                RayDir *= RecipRayLength;

            Vector3 SphereCenter = Vector3.zero;
            float R = Radius;
            float R2 = R * R;

            // Check line against each sphere, then the cylinder Because shape
            // is convex, once we hit something, we dont have to check any more.
            bool bNoHit = true;

            if (testTop)
            {
                SphereCenter.z = HalfHeight;
                bNoHit = !RaycastSphereNoPhysic(SphereCenter.ToFloat3(), R, LocalStart.ToFloat3(),
                    RayDir.ToFloat3(), RayLength, ref notPhyHitResult);

                // Discard hit if in cylindrical region.
                if (!bNoHit && notPhyHitResult.HitLocation.z < HalfHeight)
                    bNoHit = true;
            }

            if (testBottom && bNoHit)
            {
                SphereCenter.z = -HalfHeight;
                bNoHit = !RaycastSphereNoPhysic(SphereCenter.ToFloat3(), R, LocalStart.ToFloat3(),
                    RayDir.ToFloat3(), RayLength, ref notPhyHitResult);

                if (!bNoHit && notPhyHitResult.HitLocation.z > -HalfHeight)
                    bNoHit = true;
            }

            if (testMiddle && bNoHit)
            {
                // First, check if start is inside cylinder. If so - just return it.
                if (NoPhysicUtil.Square2D(LocalStart.ToFloat3()) <= R2 && LocalStart.z <= HalfHeight &&
                    LocalStart.z >= -HalfHeight)
                {
                    notPhyHitResult.HitLocation = Start;
                    notPhyHitResult.Distance = 0.0f;
                    notPhyHitResult.HitNormal = -RayDir.ToFloat3();

                    return true;
                }
                else // if not.. solve quadratic
                {
                    float A = NoPhysicUtil.Square2D(RayDir.ToFloat3());
                    float B = 2 * (LocalStart.x * RayDir.x + LocalStart.y * RayDir.y);
                    float C = NoPhysicUtil.Square2D(LocalStart.ToFloat3()) - R2;
                    float disc = B * B - 4 * A * C;

                    if (disc >= 0 && Mathf.Abs(A) > KINDA_SMALL_NUMBER * KINDA_SMALL_NUMBER)
                    {
                        float root = Mathf.Sqrt(disc);
                        float s = (-B - root) / (2 * A);
                        float z1 = LocalStart.z + s * RayDir.z;

                        // if its not before the start of the line, or past its end, or beyond the end of the cylinder, we have a hit!
                        if (s > 0 && s < RayLength && z1 <= HalfHeight && z1 >= -HalfHeight)
                        {
                            notPhyHitResult.Distance = s;

                            notPhyHitResult.HitLocation.x = LocalStart.x + s * RayDir.x;
                            notPhyHitResult.HitLocation.y = LocalStart.y + s * RayDir.y;
                            notPhyHitResult.HitLocation.z = z1;

                            notPhyHitResult.HitNormal.x = notPhyHitResult.HitLocation.x;
                            notPhyHitResult.HitNormal.y = notPhyHitResult.HitLocation.y;
                            notPhyHitResult.HitNormal.z = 0;
                            notPhyHitResult.HitNormal = math.normalizesafe(notPhyHitResult.HitNormal);

                            bNoHit = false;
                        }
                    }
                }
            }

            // If we didn't hit anything - return
            if (bNoHit)
                return false;

            notPhyHitResult.HitLocation = (unityrot * notPhyHitResult.HitLocation.ToVector3()).ToFloat3() + center;
            notPhyHitResult.HitNormal = (unityrot * notPhyHitResult.HitNormal.ToVector3()).ToFloat3();

            return true;
        }

        public bool RaycastSphereNoPhysic(float3 Origin, float Radius, float3 Start, float3 Dir,
            float Length, ref NotPhyHitResult impactInfo)
        {
            const float KINDA_SMALL_NUMBER = (1e-4f);
            float3 StartToOrigin = Origin - Start;

            float L2 = math.lengthsq(StartToOrigin); // Distance of line start from sphere centre (squared).
            float R2 = Radius * Radius;

            // If we are starting inside sphere, return a hit.
            if (L2 < R2)
            {
                impactInfo.HitLocation = Start;
                impactInfo.Distance = 0;
                impactInfo.HitNormal = -math.normalizesafe(StartToOrigin);
                return true;
            }

            if (Length < KINDA_SMALL_NUMBER)
                return false; // Zero length and not starting inside - doesn't hit.

            float D = math.dot(StartToOrigin, Dir); // distance of sphere centre in direction of query.
            if (D < 0.0f)
                return false; // sphere is behind us, but we are not inside it.

            float
                M2 = L2 - (D *
                           D); // pythag - triangle involving StartToCenter, (Dir * D) and vec between SphereCenter & (Dir * D)
            if (M2 > R2)
                return false; // ray misses sphere

            // this does pythag again. Q2 = R2 (radius squared) - M2
            float t = D - math.sqrt(R2 - M2);
            if (t > Length)
                return false; // Ray doesn't reach sphere, reject here.

            impactInfo.HitLocation = Start + Dir * t;
            impactInfo.Distance = t;
            impactInfo.HitNormal = math.normalizesafe((impactInfo.HitLocation - Origin));

            return true;
        }

        private bool RaycastBoxNoPhysic(BoxCollider col, Transform trans, Vector3 start, Vector3 end,
            ref NotPhyHitResult notPhyHitResult)
        {
            Vector3 localStart = trans.InverseTransformPoint(start);
            Vector3 localEnd = trans.InverseTransformPoint(end);

            Vector3 extent = col.size / 2.0f;

            float hitDis = 0;
            if (NoPhysicUtil.RaycastAABB(col.center.ToFloat3(), extent.ToFloat3(), localStart.ToFloat3(),
                    localEnd.ToFloat3(), out hitDis))
            {
                notPhyHitResult.HitLocation = (start + (end - start) * hitDis).ToFloat3();
                notPhyHitResult.HitNormal = float3.zero;
                notPhyHitResult.Distance = hitDis; //(hit.HitLocation - start).magnitude;
                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region private

        public int GetLayerMask(int traceFlag)
        {
            int layerMask = 0;

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.Entity)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_Entity);
                
            }
            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.World)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_World);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.Pawn)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_Pawn);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.FirstPerson)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_FirstPerson);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.Fire)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_Fire);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.StaticActor)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_StaticActor);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.StaticActor_ProjectileBreak)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_StaticActor_ProjectileBreak);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.AimAssistance)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_AimAssistance);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.Projectile)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_Projectile);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.Monster)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_Monster);
            }

            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.FirstPerson)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_FirstPerson);
            }
            
            if ((traceFlag & PhysicsLayerDefine.GetFlag(PhysicTraceType.AutoFire)) != 0)
            {
                layerMask |= (1 << UnityLayersUtil.LayerMask_AutoFire);
            }
            return layerMask;
        }

        public uint GetColliderPhysicalMaterialID(PhysicMaterial material)
        {
            //"Earth" default
            uint materialID = 3;
            if (material != null)
            {
                materialID = (uint)material.dynamicFriction;
            }

            return materialID;
        }

        #endregion
    }
}