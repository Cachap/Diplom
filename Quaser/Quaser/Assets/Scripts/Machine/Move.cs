using UnityEngine;


public class Move : MonoBehaviour
{
    [SerializeField] private bool isTable;
    [SerializeField] private bool isGuide;

    private const float MAX_X = 0.600f;
    private const float MAX_Y = 0.610f;
    private const float MAX_Z = 0.305f;

    void Update()
    {
        if(CreateWindow.isRun)
		{
            if (CreateWindow.y >= MAX_Y * -1
                && gameObject.tag == "CurrentTool")
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                    CreateWindow.y,
                    gameObject.transform.position.z);
            }

            if (isTable
                && CreateWindow.x >= MAX_X * -1 
                && CreateWindow.x <= MAX_X
                && CreateWindow.z >= MAX_Z * -1 
                && CreateWindow.z <= MAX_Z)
            {
                gameObject.transform.position = new Vector3(CreateWindow.x,
                    gameObject.transform.position.y,
                    CreateWindow.z);
            }

            if (isGuide
               && CreateWindow.z >= MAX_Z * -1 
               && CreateWindow.z <= MAX_Z)
            {
               gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                   gameObject.transform.position.y,
                   CreateWindow.z);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "TriggerColor")
            {
                gameObject.GetComponentInChildren<GameObject>().GetComponent<Material>().color = Color.red;
            }
        }
    }
}