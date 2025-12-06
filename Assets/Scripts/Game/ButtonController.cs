using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [Header("Click Settings")]
    [SerializeField] public AudioClip clickClip;

    private Color imageColor;
    private Color textColor;
    private Color highlight = new Color(0.9607843f, 0.9607843f, 0.9607843f, 1.0f);
    private Color click = new Color(0.7843137f, 0.7843137f, 0.7843137f, 1.0f);

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            imageColor = image.color;
            image.color = highlight;
        }

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            textColor = text.color;
            text.color = highlight;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.color = imageColor;
        }

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.color = textColor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.color = click;
        }

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.color = click;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.color = highlight;
        }

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.color = highlight;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFXClip(clickClip, transform);
    }
}
