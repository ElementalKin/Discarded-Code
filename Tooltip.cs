using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipHeader;
    public TextMeshProUGUI tooltipBody;

    public LayoutElement layoutElem;

    public int characterLimit = 35;

    private RectTransform rectTransform;

    public Color numberColor;

    public GameObject headerPrefab;
    public GameObject bodyPrefab;
    public GameObject divderPrefab;

    [TextArea(3, 5)]
    public string[] statusEffectTexts;

    [TextArea(3, 5)]
    public string[] statusEffectTextsStatic;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Hide();
    }

    private void Update()
    {
        // Grab the mouse position.
        Vector2 mouseInput = Input.mousePosition;

        // Move the position and pivot based on the mouse input.
        gameObject.transform.position = mouseInput;
        rectTransform.pivot = new Vector2((mouseInput.x > Screen.width / 2 ? 1 : 0), (mouseInput.y > Screen.height / 2 ? 1 : 0));
    }

    // Shows a given header and body based on two strings passed in.
    public void Show(string header, string body)
    {
        tooltipHeader.text = header;
        tooltipBody.text = body;

        // Enable the tooltip.
        layoutElem.enabled = (tooltipHeader.text.Length > characterLimit || tooltipBody.text.Length > characterLimit);
        
        gameObject.SetActive(true);
    }

    // Shows a status effect given an index and a deck.
    // For use on the Status Effects Bar.
    public void ShowDynamicStatus(int statusEffectIndex, Deck deck)
    {
        // Grab the name of the status effect from the Enum, set as the header.
        tooltipHeader.text = ((CardHelpers.StatusEffect)statusEffectIndex).ToString();

        // Grab the text from the proper index in statusEffectTexts.
        // The place-holder text "[STATUSAMOUNT]" is replaced with the amount of the status the deck supplies.
        tooltipBody.text = statusEffectTexts[statusEffectIndex].Replace
            ("[STATUSAMOUNT]", $"<color=#{ColorUtility.ToHtmlStringRGB(numberColor)}>{deck.statusList[statusEffectIndex + 1]}</color>");

        // Enable the tooltip.
        layoutElem.enabled = (tooltipHeader.text.Length > characterLimit || tooltipBody.text.Length > characterLimit);

        gameObject.SetActive(true);
    }

    // Shows extra information (if possible) given a CardUI.
    // For use in the Cards in the Player's Hand.
    public void ShowDynamicCard(CardUI cardUI)
    {
        // Grab the Card from the CardUI.
        Card card = cardUI.cardToRepresent;

        int displayedEntries = 0;
        
        // Loop for as many card effects exist on the card...
        for (int i = 0; i < card.cardEffects.Count; i++)
        {
            // If the card is...
            // A support card OR
            // A deck edit card AND the action it preforms is abandoning OR adding a card...
            if (card.cardEffects[i].effectType == CardHelpers.EffectType.Support ||
                (card.cardEffects[i].effectType == CardHelpers.EffectType.DeckEdit && (card.cardEffects[i].DeckAction == CardHelpers.DeckModify.AbandonThisCard ||
                card.cardEffects[i].DeckAction == CardHelpers.DeckModify.AddSpecificCardToDeck)))
            {
                // If this is the 2nd+ entry, add a divider first.
                if (displayedEntries > 0) Instantiate(divderPrefab, gameObject.transform);

                // Create a new header and body.
                TextMeshProUGUI newTooltipHeader = Instantiate(headerPrefab, gameObject.transform).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI newTooltipBody = Instantiate(bodyPrefab, gameObject.transform).GetComponent<TextMeshProUGUI>();

                // If the card is support...
                if (card.cardEffects[i].effectType == CardHelpers.EffectType.Support)
                {
                    // Create header from statusToEdit, create body from grabbing the proper index in statusEffectTextsStatic.
                    newTooltipHeader.text = card.cardEffects[i].statusToEdit.ToString();
                    newTooltipBody.text = statusEffectTextsStatic[(int)card.cardEffects[i].statusToEdit];
                }
                else // Else, the card is deck edit...
                {
                    // Only supported actions for now are Adding Cards to Decks and Abandoning.
                    if (card.cardEffects[i].DeckAction == CardHelpers.DeckModify.AddSpecificCardToDeck)
                    {
                        // Create header from cardName, create body from CardDecsription.
                        newTooltipHeader.text = $"Add Card To Deck: {card.cardEffects[i].cardToAdd.cardName} (Cost - {card.cardEffects[i].cardToAdd.cardCost})";
                        newTooltipBody.text = cardUI.CardDescription(card.cardEffects[i].cardToAdd);
                    }
                    else if (card.cardEffects[i].DeckAction == CardHelpers.DeckModify.AbandonThisCard)
                    {
                        newTooltipHeader.text = "Abandon";
                        newTooltipBody.text = "After being played this card is removed from your deck until the end of combat.";
                    }
                }

                // Enable the tooltip.
                layoutElem.enabled = (newTooltipHeader.text.Length > characterLimit || newTooltipBody.text.Length > characterLimit);

                if (!gameObject.activeInHierarchy) gameObject.SetActive(true);

                displayedEntries++;
            }
        }
    }

    // Disables the tooltip.
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}