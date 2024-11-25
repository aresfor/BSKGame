using Game.Core;
using Game.Gameplay;
using Game.Math;

namespace Game.Client
{
    public partial class RoleEntityLogic:INavigationAgent
    {
        private INavigationAgent m_AgentImpl;

        public void InitNavigationAgent()
        {
            var boardGraph = GameUtils.Board;
            m_AgentImpl = new GraphNavigationAgent(this);
        }
        
        //@TODO: 改造非泛型基类
        public bool MoveToDestination(float3 destination, IGraph graph)
        {
            return m_AgentImpl.MoveToDestination(destination, graph);

        }
        
        // public bool MoveToDestination(float3 destination)
        // {
        // }

        public bool PauseMovement()
        {
            return m_AgentImpl.PauseMovement();
        }

        public bool ResumeMovement()
        {
            return m_AgentImpl.ResumeMovement();
        }

        public bool StopMovement()
        {
            return m_AgentImpl.StopMovement();
        }

        public bool HasArrived()
        {
            return m_AgentImpl.HasArrived();
        }

        public bool SamplePosition(float3 originPosition, out float3 samplePosition)
        {
            return m_AgentImpl.SamplePosition(originPosition, out samplePosition);
        }

        public float3 GetMovementDirection()
        {
            return m_AgentImpl.GetMovementDirection();
        }

        public bool SetMovementDirection(float3 movementDirection)
        {
            return m_AgentImpl.SetMovementDirection(movementDirection);
        }

        public bool HasPath()
        {
            return m_AgentImpl.HasPath();
        }

        public void UpdateNavigation(float elapsedTime, float realElapsedTime)
        {
            m_AgentImpl.UpdateNavigation(elapsedTime, realElapsedTime);
        }

        public bool IsPathFollowing()
        {
            return m_AgentImpl.IsPathFollowing();
        }

        public void OnMoveFinish(bool success)
        {
            m_AgentImpl.OnMoveFinish(success);
        }

        public bool Warp(float3 newPosition)
        {
            return m_AgentImpl.Warp(newPosition);
        }

        public bool IsPathPending()
        {
            return m_AgentImpl.IsPathPending();
        }

        public float GetMaxSpeed()
        {
            return m_AgentImpl.GetMaxSpeed();
        }

        public void SetMaxSpeed(float newValue)
        {
            m_AgentImpl.SetMaxSpeed(newValue);
        }

        public bool IsOrientToMovement()
        {
            return m_AgentImpl.IsOrientToMovement();
        }

        public float3 GetDestination()
        {
            return m_AgentImpl.GetDestination();
        }

        public float3 GetCurrentPosition()
        {
            return m_AgentImpl.GetCurrentPosition();
        }

        public float GetStoppingDistance()
        {
            return m_AgentImpl.GetStoppingDistance();
        }

        public void SetStoppingDistance(float newValue)
        {
            m_AgentImpl.SetStoppingDistance(newValue);
        }
    }
}