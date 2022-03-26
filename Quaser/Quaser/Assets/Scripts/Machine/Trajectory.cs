using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;

    private void Start()
    {
        line.startWidth = 0.001f;
        line.startColor = Color.red;
        line.positionCount = 0;
        line.endColor = Color.red;
    }

    private void Update()
    {
        Vector3 currentPoint = transform.position;
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, currentPoint);
    }
}
