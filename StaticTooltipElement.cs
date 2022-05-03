using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticTooltipElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // I may expand this to be a bit more flexable
    // In cases of tooltips being used in multiple areas.
    // But for now, this will work!
    public TooltipManager tooltipMan;

    public string tooltipHeader;
    [TextArea(3,5)]
    public string tooltipBody;

    private void Start()
    {
        tooltipMan = FindObjectOfType<TooltipManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipMan.ShowTooltip(tooltipHeader, tooltipBody);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipMan.HideTooltip();
    }

    private void OnDisable()
    {
        if (tooltipMan != null)
        {
            tooltipMan.HideTooltip();
        }
    }
}