using Assets.Scripts.Machine;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{
    private static Text[] texts;

    private void Start()
    {
        texts = gameObject.GetComponentsInChildren<Text>();

        texts[3].text = $"�����: -";
        texts[2].text = $"������: -";
        texts[1].text = $"��������: -";
    }

    public static void Change(Tool tool)
    {
        texts[3].text = $"�����: {tool.Length}";
        texts[2].text = $"������: {tool.Radius}";
        texts[1].text = $"��������: {tool.Name}";
    }
}
