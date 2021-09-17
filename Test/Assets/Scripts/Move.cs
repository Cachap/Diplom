using System.Runtime.InteropServices;
using UnityEngine;
using Teeeest;

public class Move : MonoBehaviour
{
    [SerializeField] private bool isTool;

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
            Debug.Log(TestClass.x);
            Debug.Log(TestClass.y);
            Debug.Log(TestClass.z);
    }

	void Update()
    {
        //      if (Input.GetKey(KeyCode.D) && isTool)
        //      {
        //          gameObject.transform.Translate(Vector3.right * Time.deltaTime * -1);
        //      }
        //      if (Input.GetKey(KeyCode.A) && isTool)
        //      {
        //          gameObject.transform.Translate(Vector3.right * Time.deltaTime);
        //      }

        //      if(Input.GetKey(KeyCode.UpArrow) && isTool)
        //{
        //           gameObject.transform.Translate(Vector3.up * Time.deltaTime * -1);
        //      }

        //      if (Input.GetKey(KeyCode.DownArrow) && isTool)
        //      {
        //      gameObject.transform.Translate(Vector3.up * Time.deltaTime);
        //      }

        //      if (Input.GetKey(KeyCode.W))
        //      {
        //         gameObject.transform.Translate(Vector3.forward * Time.deltaTime * -1);
        //      }
        //      if (Input.GetKey(KeyCode.S))
        //      {
        //         gameObject.transform.Translate(Vector3.forward * Time.deltaTime);
        //      }
        if(isTool)
		{
            gameObject.transform.position = new Vector3(TestClass.x, TestClass.y, TestClass.z);
        }
		else
		{
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, TestClass.z);
        }
    }

	private void OnTriggerEnter(Collider other)
	{

        if (other.gameObject.tag == "TriggerColor")
        {
            other.gameObject.GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.red;        
        }

        if (other.gameObject.tag == "Trigger")
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.red;
            Debug.Log("Уехал");
        }
    }

	private void OnTriggerExit(Collider other)
	{
        if (other.gameObject.tag == "Trigger")
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.white;
            Debug.Log("Уехал");
        }

        if (other.gameObject.tag == "TriggerColor")
        {
            other.gameObject.GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.white;
        }
    }
}
