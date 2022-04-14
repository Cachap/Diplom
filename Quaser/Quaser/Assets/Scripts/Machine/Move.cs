using UnityEngine;


public class Move : MonoBehaviour
{
    [SerializeField] private bool isTable;
    [SerializeField] private bool isGuide;

    void Update()
    {
        if(CreateWindow.isRun)
		{
            if (gameObject.tag == "CurrentTool")
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                    CreateWindow.y,
                    gameObject.transform.position.z);
            }

            if (isTable)
            {
                gameObject.transform.position = new Vector3(CreateWindow.x,
                    gameObject.transform.position.y,
                    CreateWindow.z);
            }

            if (isGuide)
            {
               gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                   gameObject.transform.position.y,
                   CreateWindow.z);
            }
        }
    }
}