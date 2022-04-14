using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField]
    private LineRenderer line;

    private void Start()
    {
        line.startWidth = 0.01f;
        line.positionCount = 0;
    }

    private void Update()
    {
        Vector3 currentPoint = transform.position;
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, currentPoint);
    }
}
