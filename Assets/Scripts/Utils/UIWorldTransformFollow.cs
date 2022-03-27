using UnityEngine;
using UnityEngine.Events;

public class UIWorldTransformFollow : MonoBehaviour
{
    [SerializeField] Transform targetToFollow = default;
    [SerializeField, Range(0.01f, 1f)] float maxScale = 1;
    [SerializeField] Vector3 offset = Vector3.up * 3;
    [SerializeField] float maxDistanceToSee = 30;

    public UnityEvent<string> onSetTarget;

    float distance = 0;
    float delta = 0;

    Vector3 screenPoint = Vector3.zero;

    private void Update()
    {
        screenPoint = Camera.main.WorldToScreenPoint(targetToFollow.position + offset);

        screenPoint.z = screenPoint.z >= 0 ? 0 : -1;

        if (screenPoint.z < 0)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            distance = Vector3.Distance(Camera.main.transform.position, targetToFollow.position + offset);
            delta = Mathf.Clamp01(distance / maxDistanceToSee);
            transform.position = screenPoint;
            transform.localScale = Vector3.one * maxScale * (1 - delta);
        }
    }

    public void SetTarget(Transform target)
    {
        targetToFollow = target;
        onSetTarget?.Invoke(target.name);
    }

    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}
