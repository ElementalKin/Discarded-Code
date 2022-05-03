using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicTooltipElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TooltipManager tooltipMan;

    // this is stupid, i will revisit this.
    public bool actuallyImACard;
    public CardUI cardToRepresent;
    [Space(15)]
    public Deck deckToRepresent;
    public CardHelpers.StatusEffect statusToRepresent;

    private void Start()
    {
        tooltipMan = FindObjectOfType<TooltipManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (actuallyImACard)
        {
            tooltipMan.ShowTooltipDynamicCard(cardToRepresent);
            return;
        }

        if (deckToRepresent != null)
        {
            tooltipMan.ShowTooltipDynamicStatus((int)statusToRepresent, deckToRepresent);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (actuallyImACard)
        {
            tooltipMan.HideTooltipDynamicCard();
            return;
        }

        tooltipMan.HideTooltip();
    }
}