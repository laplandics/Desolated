using System;
using System.Collections;
using Constant;
using R3;
using UnityEngine;
using UnityEngine.AI;

namespace Entity.Ability
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Move : BaseModule
    {
        public NavMeshAgent agent;
        public MoveState state;
        
        private float _moveRange;
        private readonly ReactiveProperty<float> _agentSpeed = new();
        
        public override EntityModules ModuleType => EntityModules.Move;

        public override void OnAdd()
        {
            _agentSpeed.Value = state.agentSpeed;
            
            _agentSpeed.Subscribe(speed => agent.speed = speed);
            _agentSpeed.Subscribe(speed => state.agentSpeed = speed);
            _agentSpeed.Subscribe(CalculateMoveRange);
        }

        public Vector3 GetMovePoint(Vector3 point)
        {
            var encounter = Main.CurrentEncounterPhase;
            var maxDistance = float.MaxValue;
            if (encounter.Value == EncounterPhases.Battle)
            { maxDistance = _moveRange; }
            
            var current = transform.position;
            var dist = Vector3.Distance(point, current);
            var distance = Mathf.Min(dist, maxDistance);
            var direction = (point - current).normalized;
            
            var destination = current + direction * distance;
            return destination;
        }

        public IEnumerator MoveTo(Vector3 point)
        {
            var destination = GetMovePoint(point);
            agent.SetDestination(destination);
            yield return new WaitWhile(IsMoving);
        }

        private bool IsMoving() => agent.hasPath && !agent.pathPending && agent.velocity.sqrMagnitude > 0.01f;
        private void CalculateMoveRange(float speed) => _moveRange = speed * Values.TURN_DURATION;
        
        public override void ParseToState(string stateString) => state = GetState<MoveState>(stateString);
        public override string ParseToString() => GetState(state);
    }
    
    [Serializable]
    public struct MoveState
    {
        public float agentSpeed;
    }
}