using Assets.Scripts.Machine;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{
    private static Text[] texts;

    private void Start()
    {
        texts = gameObject.GetComponentsInChildren<Text>();

        texts[3].text = $"Длина: -";
        texts[2].text = $"Радиус: -";
        texts[1].text = $"Название: -";
    }

    public static void Change(Tool tool)
    {
        texts[3].text = $"Длина: {tool.Length}";
        texts[2].text = $"Радиус: {tool.Radius}";
        texts[1].text = $"Название: {tool.Name}";
    }
}
