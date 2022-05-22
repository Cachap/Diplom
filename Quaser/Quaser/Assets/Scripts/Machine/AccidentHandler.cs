using Assets.Scripts.Machine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AccidentHandler : MonoBehaviour
{
    private Color defaultColorMaterial;
    private Color defaultColorImage;
    private readonly Color warningColor = new Color(0.76f, 0.11f, 0.22f, 1);

    private MeshRenderer meshRenderer;
    private GameObject AccidentPanel;
    private Button ResetButton;
    private Text textPanel;
    private Image panelImage;

    private Client client;
    private bool start;

    void Start()
    {
        client = GameObject.Find("Main Camera").GetComponent<Client>();

        AccidentPanel = GameObject.Find(nameof(AccidentPanel));
        ResetButton = GameObject.Find(nameof(ResetButton)).GetComponent<Button>();
        ResetButton.interactable = false;

        defaultColorImage = AccidentPanel.GetComponent<Image>().color;
        textPanel = AccidentPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
        panelImage = AccidentPanel.GetComponent<Image>();
        if (gameObject.name != "Cutter")
        {
            meshRenderer = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        }
        else
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        defaultColorMaterial = meshRenderer.material.color;
    }

	private void OnTriggerEnter(Collider other)
    {
        if ((gameObject.name == "Стол" && other.CompareTag("TriggerTable"))
            || (gameObject.name == "Направляющая стола" && other.CompareTag("TriggerGuide"))
            || (gameObject.name == "Шасси шпинделя" && other.CompareTag("TriggerSpindle"))
            || (gameObject.name == "Cutter" && other.CompareTag("TriggerSpindle")))
        {
            if(gameObject.name == "Стол")
            {
                client.plcHandler.accidentInputState = PlcHandler.AccidentInputStates.X;
			}

            if (gameObject.name == "Направляющая стола")
            {
                client.plcHandler.accidentInputState = PlcHandler.AccidentInputStates.Y;
			}

			if (gameObject.name == "Cutter" || gameObject.name == "Шасси шпинделя")
			{
				client.plcHandler.accidentInputState = PlcHandler.AccidentInputStates.Z;
			}

            start = true;
            meshRenderer.material.color = warningColor;
            textPanel.text = "Авария";
            ResetButton.interactable = true;
            StartCoroutine(Blink());

            client.plcHandler.WritePlcAccident();
		}

		if (other.CompareTag("AllowedTrigger"))
        {
            defaultColorMaterial = other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color;
            other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = warningColor;
            textPanel.text = "Авария";
            ResetButton.interactable = true;
            StartCoroutine(Blink());
        }
    }

	private void OnTriggerExit(Collider other)
    {
        if ((gameObject.name == "Стол" && other.CompareTag("TriggerTable"))
            || (gameObject.name == "Направляющая стола" && other.CompareTag("TriggerGuide"))
            || (gameObject.name == "Шасси шпинделя" && other.CompareTag("TriggerSpindle"))
            || (gameObject.name == "Cutter" && other.CompareTag("TriggerSpindle")))
        {
            client.plcHandler.accidentInputState = PlcHandler.AccidentInputStates.None;
            client.plcHandler.WritePlcAccident();
            start = false;

            textPanel.text = "Работа";
            meshRenderer.material.color = defaultColorMaterial;
            ResetButton.interactable = false;
            panelImage.color = defaultColorImage;
            StopCoroutine(Blink());
        }

        if (other.CompareTag("AllowedTrigger"))
        {
            other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = defaultColorMaterial;
            textPanel.text = "Работа";
            ResetButton.interactable = false;
            StopCoroutine(Blink());
        }
    }

	public IEnumerator Blink()
    {
        while(textPanel.text != "Работа")
        {
            if (panelImage.color == defaultColorImage)
                panelImage.color = warningColor;
            else
                panelImage.color = defaultColorImage;

            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
