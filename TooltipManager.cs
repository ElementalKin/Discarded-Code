using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public Tooltip tooltip;
    public Tooltip tooltipCard;

    public void ShowTooltip(string header, string body)
    {
        tooltip.Show(header, body);
    }

    public void ShowTooltipDynamicStatus(int status, Deck deck)
    {
        tooltip.ShowDynamicStatus(status, deck);
    }

    public void ShowTooltipDynamicCard(CardUI card)
    {
        tooltipCard.ShowDynamicCard(card);
    }

    public void HideTooltip()
    {
        tooltip.Hide();
    }

    public void HideTooltipDynamicCard()
    {
        tooltipCard.Hide();

        foreach (Transform child in tooltipCard.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}