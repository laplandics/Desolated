using System;
using System.Collections;
using System.Collections.Generic;
using Constant;
using EntityInteraction;
using UnityEngine;
using UnityEngine.AI;
using Utils;
using WorldObject;

namespace EntitySystem
{
    [CreateAssetMenu(fileName = "MovementSystem", menuName = "Systems/Movement")]
    public class MovementSystem : SystemBase
    {
        private readonly Dictionary<string, AgentInfo> _agentsMap = new();
        
        protected override void OnEntityRegistered(Entity entity)
        {
            var agentNavMesh = entity.TryGetComponent<NavMeshAgent>(out var agent) 
                ? agent : entity.gameObject.AddComponent<NavMeshAgent>();

            var agentInfo = new AgentInfo();
            agentInfo.Entity = entity;
            agentInfo.Agent = agentNavMesh;
            _agentsMap[entity.state.id] = agentInfo;
        }

        public void MoveTo(MoveInteraction interaction)
        {
            if (!_agentsMap.TryGetValue(interaction.ID, out var agentInfo)) return;
            const float t = Values.TURN_DURATION;
            
            var entity = RegisteredEntitiesMap[interaction.ID];
            var current = agentInfo.Agent.transform.position;
            var direction = (interaction.Point - current).normalized;
            var speed = entity.state.speed;
            var maxDist = speed * t;
            var currentDist = Vector3.Distance(current, interaction.Point);

            var encounterState = Main.CurrentEncounterState.Value;
            var distance = encounterState switch
            {
                EncounterStates.Exploration => currentDist,
                EncounterStates.Cutscene => 0f,
                EncounterStates.Battle => Mathf.Min(maxDist, currentDist),
                _ => throw new ArgumentOutOfRangeException()
            };

            var destination = current + direction * distance;
            
            entity.TransformsManager.SetTargetPosition(destination);
            agentInfo.Agent.SetDestination(destination);
            
            if (agentInfo.UpdateCoroutine != null) G.Resolve<Coroutines>().Stop(agentInfo.UpdateCoroutine);
            agentInfo.UpdateCoroutine = G.Resolve<Coroutines>().Start(UpdateAgentState(agentInfo));
        }

        private IEnumerator UpdateAgentState(AgentInfo agentInfo)
        {
            var agent = agentInfo.Agent;
            var entity = agentInfo.Entity;
            var delay = new WaitForSeconds(0.2f);
            
            yield return delay;
            while (IsAgentMoving(agent))
            {
                entity.TransformsManager.SetCurrentPosition(agent.transform.position);
                entity.TransformsManager.SetCurrentRotation(agent.transform.eulerAngles);
                yield return delay;
            }
            
            entity.TransformsManager.SetTargetPosition(Vector3.zero);
            entity.TransformsManager.SetCurrentPosition(agent.transform.position);
        }

        private bool IsAgentMoving(NavMeshAgent agent) => agent.velocity.sqrMagnitude > 0.01f;

        protected override void OnEntityUnregistered(Entity entity)
        {
            if (!_agentsMap.Remove(entity.state.id, out var agentInfo)) return;
            if (agentInfo.UpdateCoroutine == null) return;
            
            G.Resolve<Coroutines>().Stop(agentInfo.UpdateCoroutine);
        }
    }

    public struct AgentInfo
    {
        public Entity Entity;
        public NavMeshAgent Agent;
        public Coroutine UpdateCoroutine;
    }
}
