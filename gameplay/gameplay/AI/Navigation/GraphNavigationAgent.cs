using System;
using System.Collections.Generic;
using Game.Core;
using Game.Math;
using GameFramework;

namespace Game.Gameplay
{


    public enum ENavigationState : byte
    {
        None,
        Idle,
        Pause,
        Running,

    }

    public class GraphNavigationAgent : INavigationAgent
    {
        private ENavigationState m_NavigationState = ENavigationState.Idle;
        private Queue<float3> m_PathPoints = new Queue<float3>();
        private float3 m_CurrentPoint;
        private float3 m_CurrentMovementDirection;
        private float3 m_CurrentDestination;

        public Action<bool> OnMoveFinishedCall { get; set; }
        private IGameEntityLogic m_EntityLogic;
        public IView EntityView => m_EntityLogic as IView;
        private float m_CurrentSpeed = 5;
        private float m_MaxSpeed = 5;
        private float m_StoppingDistance = 0.01f;
        public static float3 InValidPosition = new float3(float.PositiveInfinity);



        public GraphNavigationAgent(IGameEntityLogic entityLogic)
        {
            m_EntityLogic = entityLogic;
        }

        //@TODO: 改造一个非泛型基类
        public new bool MoveToDestination(float3 destination, IGraph graph)
        {
            if (!IsValidPosition(destination))
                return false;

            //@TODO: 需要吗？
            if (false == SamplePosition(destination, out var samplePosition))
                return false;

            List<float3> path = ListPool<float3>.Get();

            bool result = false;

            //@TEMP:
            if (graph.AStar(GetCurrentPosition(), samplePosition, path))
            {
                m_PathPoints.Clear();
                foreach (var pathPoint in path)
                {
                    m_PathPoints.Enqueue(pathPoint);
                }

                result = path.Count > 0;
            }

            if (result)
            {
                //最后一个点就是终点
                m_CurrentDestination = path[^1];
                var startPoint = m_PathPoints.Dequeue();
                StartNewPathPoint(startPoint);
            }


            path.Clear();
            ListPool<float3>.Release(path);


            return result;
        }

        public void UpdateNavigation(float elapsedTime, float realElapsedTime)
        {


            if (m_NavigationState is ENavigationState.Running)
            {
                var preDirection = m_CurrentMovementDirection;
                m_CurrentMovementDirection = math.normalize(m_CurrentPoint - GetCurrentPosition());


                if (math.distance(m_CurrentPoint, GetCurrentPosition()) <= GetStoppingDistance()
                    || math.dot(preDirection, m_CurrentMovementDirection) <= 0)
                {
                    if (m_PathPoints.Count > 0)
                    {
                        float3 newPoint = m_PathPoints.Dequeue();
                        StartNewPathPoint(newPoint);
                    }
                    else
                    {
                        OnMoveFinish(true);
                        return;
                    }
                }

                EntityView.ViewPosition =
                    GetCurrentPosition() + m_CurrentMovementDirection * m_CurrentSpeed * elapsedTime;
            }
        }

        private void StartNewPathPoint(float3 pathPoint)
        {
            m_CurrentPoint = pathPoint;
            m_CurrentMovementDirection = math.normalize(m_CurrentPoint - GetCurrentPosition());

            if (m_NavigationState is ENavigationState.Idle)
            {
                m_NavigationState = ENavigationState.Running;
            }
            else if (m_NavigationState is ENavigationState.Pause)
            {
                ResumeMovement();
            }
        }

        public bool IsValidPosition(float3 inPosition)
        {
            return -InValidPosition.x < inPosition.x && inPosition.x < InValidPosition.x
                                                     && -InValidPosition.y < inPosition.y &&
                                                     inPosition.y < InValidPosition.y
                                                     && -InValidPosition.z < inPosition.z &&
                                                     inPosition.z < InValidPosition.z;
        }



        public bool PauseMovement()
        {
            m_NavigationState = ENavigationState.Pause;
            return true;
        }

        public bool ResumeMovement()
        {
            if (HasPath())
            {
                m_NavigationState = ENavigationState.Running;
            }

            return true;
        }

        public bool StopMovement()
        {
            m_NavigationState = ENavigationState.Idle;
            return true;
        }

        public bool HasArrived()
        {
            if (IsPathFollowing())
            {
                return false;
            }
            else
            {
                return math.distance(GetCurrentPosition(), GetDestination()) <= GetStoppingDistance();
            }
        }

        public bool SamplePosition(float3 originPosition, out float3 samplePosition)
        {
            samplePosition = originPosition;
            return true;
        }

        public float3 GetMovementDirection()
        {
            if (IsPathFollowing())
                return math.normalize(m_CurrentMovementDirection);

            return float3.zero;
        }

        public bool SetMovementDirection(float3 movementDirection)
        {
            m_CurrentMovementDirection = movementDirection;
            return true;
        }

        public bool HasPath()
        {
            return (IsPathFollowing() || m_PathPoints.Count > 0);

        }



        public bool IsPathFollowing()
        {
            return m_NavigationState == ENavigationState.Running;
        }

        public void OnMoveFinish(bool success)
        {
            m_NavigationState = ENavigationState.Idle;
            //精准设置位置
            EntityView.ViewPosition = m_CurrentPoint;
            OnMoveFinishedCall?.Invoke(success);
        }

        public bool Warp(float3 newPosition)
        {
            if (SamplePosition(newPosition, out var samplePosition))
            {
                EntityView.ViewPosition = samplePosition;
                return true;
            }

            return false;
        }

        public bool IsPathPending()
        {
            return false;
        }

        public float GetMaxSpeed()
        {
            return m_MaxSpeed;
        }

        public void SetMaxSpeed(float newValue)
        {
            m_MaxSpeed = newValue;
        }

        public bool IsOrientToMovement()
        {
            return true;
        }

        public float3 GetDestination()
        {
            return m_CurrentDestination;
        }

        public float3 GetCurrentPosition()
        {
            return EntityView.ViewPosition;
        }

        public float GetStoppingDistance()
        {
            return m_StoppingDistance;
        }

        public void SetStoppingDistance(float newValue)
        {
            m_StoppingDistance = newValue;
        }
    }
}