using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : LocalizedLabel
{
    protected override string ReadText() => GetComponent<Text>().text;

    protected override void WriteText(string value) => GetComponent<Text>().text = value;
}
