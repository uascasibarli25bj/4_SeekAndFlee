using UnityEngine;

public class ShowWarningTextWhenClose : MonoBehaviour
{
    public Transform obstacle;
    public float distanceThreshold = 3f;
    public GameObject warningText;

    void Update()
    {
        if (obstacle == null || warningText == null)
            return;

        float distance = Vector3.Distance(transform.position, obstacle.position);

        warningText.SetActive(distance <= distanceThreshold);
    }
}
