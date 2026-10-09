using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPips : MonoBehaviour
{
    [SerializeField] private Image template;
    [SerializeField] private Sprite fullSprite;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Color fullColor = Color.white;
    [SerializeField] private Color emptyColor = Color.gray;

    private readonly List<Image> pips = new();

    public void SetSprites(Sprite full, Sprite empty)
    {
        fullSprite = full;
        emptySprite = empty != null ? empty : full;
    }

    public void Show(int filled, int total)
    {
        EnsureCount(total);

        for (int i = 0; i < pips.Count; i++)
        {
            bool isVisible = i < total;
            pips[i].gameObject.SetActive(isVisible);

            if (!isVisible) continue;

            bool isFull = i < filled;
            pips[i].sprite = isFull ? fullSprite : emptySprite;
            pips[i].color = isFull ? fullColor : emptyColor;
        }
    }

    private void EnsureCount(int count)
    {
        if (pips.Count == 0)
            pips.Add(template);

        while (pips.Count < count)
            pips.Add(Instantiate(template, template.transform.parent));
    }
}
