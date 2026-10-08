using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class LocalizedTextMesh : LocalizedLabel
{
    protected override string ReadText() => GetComponent<TextMesh>().text;

    protected override void WriteText(string value) => GetComponent<TextMesh>().text = value;
}
