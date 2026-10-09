using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : LocalizedLabel
{
    [SerializeField] private bool formatKeys;

    protected override string ReadText() => GetComponent<TMP_Text>().text;

    protected override void WriteText(string value) => GetComponent<TMP_Text>().text = formatKeys ? KeyGlyphs.Format(value) : value;
}
