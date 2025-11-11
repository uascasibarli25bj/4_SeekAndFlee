using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Agent follows Target", story: "[Agent] follows [Target]", category: "Action", id: "285248ea78247c6affc45eeee0029aa4")]
public partial class AgentFollowsTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> FollowDistance;

    private NavMeshAgent navAgent;

    protected override Status OnStart()
    {
        if (Agent?.Value == null || Target?.Value == null)
            return Status.Failure;

        navAgent = Agent.Value.GetComponent<NavMeshAgent>();

        if (navAgent == null)
            return Status.Failure;

        navAgent.isStopped = false;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent?.Value == null || Target?.Value == null || navAgent == null)
            return Status.Failure;

        float distance = Vector3.Distance(
            Agent.Value.transform.position,
            Target.Value.transform.position
        );

        // Si está más lejos que la distancia de seguimiento, ir hacia el objetivo
        if (distance > FollowDistance.Value)
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(Target.Value.transform.position);
        }
        else
        {
            // Si ya está suficientemente cerca, parar
            navAgent.isStopped = true;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (navAgent != null)
            navAgent.isStopped = true;
    }
}