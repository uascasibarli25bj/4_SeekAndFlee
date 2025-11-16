using System;
using Unity.Behavior;
using UnityEngine;
using TMPro;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "Show Warning Text",
    story: "Shows warning text when target is within distance",
    category: "Action",
    id: "show-warning-text-node"
)]
public class ShowWarningText : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<string> Message;
    [SerializeReference] public BlackboardVariable<float> TriggerDistance;

    private TextMeshPro textObject;

    protected override Status OnStart()
    {
        // Crear el texto si no existe
        if (textObject == null)
        {
            GameObject go = new GameObject("WarningText");
            go.transform.SetParent(Agent.Value.transform);
            go.transform.localPosition = new Vector3(0, 3, 0); // encima de la cabeza

            textObject = go.AddComponent<TextMeshPro>();
            textObject.fontSize = 3;
            textObject.alignment = TMPro.TextAlignmentOptions.Center;
        }

        textObject.text = Message.Value;
        textObject.gameObject.SetActive(false); // empieza oculto

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        float dist = Vector3.Distance(Agent.Value.transform.position, Target.Value.transform.position);

        if (dist <= TriggerDistance.Value)
        {
            textObject.gameObject.SetActive(true);
        }
        else
        {
            textObject.gameObject.SetActive(false);
        }

        return Status.Running; // seguimos comprobando
    }

    protected override void OnEnd()
    {
        // Al finalizar, ocultamos el texto
        if (textObject != null)
            textObject.gameObject.SetActive(false);
    }
}
