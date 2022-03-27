using UnityEngine;

public class ChildLabelCreator : MonoBehaviour
{
    [SerializeField] UIWorldTransformFollow labelPref;
    [SerializeField] Transform labelContainer;

    public void CreateLabels()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var temp = Instantiate<UIWorldTransformFollow>(labelPref, labelContainer);
            temp.SetTarget(transform.GetChild(i));
            temp.SetOffset(new Vector3(0, transform.GetChild(i).lossyScale.magnitude, 0));
        }
    }
}
