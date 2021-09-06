using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private bool isTool;

    void Update()
    {
            if (Input.GetKey(KeyCode.D) && isTool)
            {
                gameObject.transform.Translate(Vector3.right * Time.deltaTime * -1);
            }
            if (Input.GetKey(KeyCode.A) && isTool)
            {
                gameObject.transform.Translate(Vector3.right * Time.deltaTime);
            }

            if(Input.GetKey(KeyCode.UpArrow) && isTool)
		    {
                 gameObject.transform.Translate(Vector3.up * Time.deltaTime * -1);
            }

            if (Input.GetKey(KeyCode.DownArrow) && isTool)
            {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.W))
            {
               gameObject.transform.Translate(Vector3.forward * Time.deltaTime * -1);
            }
            if (Input.GetKey(KeyCode.S))
            {
               gameObject.transform.Translate(Vector3.forward * Time.deltaTime);
            }
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag == "TriggerColor")
        {
            other.gameObject.GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.red;
            Debug.Log("Столкновение");          
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
