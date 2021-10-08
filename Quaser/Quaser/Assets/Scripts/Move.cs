using UnityEngine;
using Teeeest;

public class Move : MonoBehaviour
{
    [SerializeField] private bool isTool;
    [SerializeField] private bool isTable;
    [SerializeField] private bool isGuide;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        TestClass.CreateWindow();
        TestClass.x = gameObject.transform.position.x;
        TestClass.y = gameObject.transform.position.y;
        TestClass.z = gameObject.transform.position.z;
    }

    void Update()
    {
        if (isTool)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, TestClass.y, gameObject.transform.position.z);
        }
        if(isTable)
		{
            gameObject.transform.position = new Vector3(TestClass.x, gameObject.transform.position.y, TestClass.z);
        }
        if (isGuide)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, TestClass.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TriggerColor")
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.red;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "TriggerColor")
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.white;
        }
    }
}
