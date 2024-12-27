

using System;
using Game.Core;
using Game.Math;

namespace Game.Gameplay
{
    /// <summary>
    /// 通用寻路接口
    /// </summary>
    public interface INavigation
    {
        //bool MoveToDestination(float3 destination);
        //@TODO: 改造非泛型基类
        public new bool MoveToDestination(float3 destination, IGraph graph);
        bool PauseMovement();
        bool ResumeMovement();
        bool StopMovement();
        bool HasArrived();
        bool SamplePosition(float3 originPosition, out float3 samplePosition);

        float3 GetMovementDirection();
        bool SetMovementDirection(float3 movementDirection);
        bool HasPath();
        void UpdateNavigation(float elapsedTime, float realElapsedTime);
        public Action<bool> OnMoveFinishedCall { get; set; }

    }

    public interface INavigationAgent : INavigation
    {
        bool IsPathFollowing();
        public void OnMoveFinish(bool success);
        bool Warp(float3 newPosition);
        public bool IsPathPending();
        float GetMaxSpeed();
        void SetMaxSpeed(float newValue);
        bool IsOrientToMovement();
        float3 GetDestination();
        float3 GetCurrentPosition();
        float GetStoppingDistance();
        void SetStoppingDistance(float newValue);
    }
}