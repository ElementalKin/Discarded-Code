using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Card cardToRepresent;
    public Button selectCardButton;
    public GameObject[] cardPlayingFX;
    [Space(10)]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI descriptionText;
    [Space(5)]
    public Image artImage;
    public Image watermarkImage;
    public Image frameImage;
    public Image stickerImage;
    [Space(5)]
    public GameObject AttackIconPrefab;
    public GameObject BlockIconPrefab;
    public GameObject SupportIconPrefab;
    public VerticalLayoutGroup AttributeIconParent;
    [Space(5)]
    public Sprite[] watermarkSprites;
    [Space(10)]
    public GameObject handParent;
    public GameObject selectionParent;

    public Color addedDamageColor;
    public Color reducedDamageColor;
    public Color stickerTextColor;

    private CardHandUI myManager;
    private bool followingCursor = false;

    public bool isEnemyCard = false;

    private void Update()
    {
        if (followingCursor)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void SelectCard()
    {
        if (myManager.selectedCardUI != null)
        {
            myManager.ReturnSelectedCard();
        }

        selectCardButton.gameObject.GetComponent<Image>().raycastTarget = false;
        gameObject.transform.SetParent(selectionParent.transform);
        followingCursor = true;
    }

    public void PlayCard()
    {
        myManager.PlaySelectedCard();
    }

    public void ReturnCard()
    {
        selectCardButton.gameObject.GetComponent<Image>().raycastTarget = true;
        gameObject.transform.SetParent(handParent.transform);
        followingCursor = false;
    }

    public void DisplayCard(Card card, CardHandUI cardHand)
    {
        // This is a safe opperation since there will only ever be
        // ONE CardHandUI component in the scene at a time.
        myManager = cardHand;

        cardToRepresent = card;

        if (gameObject.TryGetComponent(out DynamicTooltipElement tooltipElem))
        {
            tooltipElem.tooltipMan = FindObjectOfType<TooltipManager>();
        }

        // Populate the title and battery cost.
        titleText.text = cardToRepresent.cardName;
        costText.text = cardToRepresent.cardCost.ToString();

        // Description. Step into this function for more.
        descriptionText.text = CardDescription(card);

        // Card art, watermark, and frame.
        artImage.sprite = cardToRepresent.cardArt;
        watermarkImage.sprite = watermarkSprites[(int)cardToRepresent.bodyPart];
        frameImage.sprite = cardToRepresent.cardFrame;

        if (stickerImage != null)
        {
            Sticker stickerCheck = myManager.stateMachine.PlayerHand.gameObject.GetComponent<PartsManager>().allPartsComponents[(int)card.bodyPart].sticker;
            if (stickerCheck != null)
            {
                stickerImage.gameObject.SetActive(true);
                stickerImage.sprite = stickerCheck.stickerArt;
            }
            else
            {
                stickerImage.gameObject.SetActive(false);
            }
        }

        // Attatch attritube icons.
        bool hasAtt = false, hasBlo = false, hasSup = false;
        for (int i = 0; i < cardToRepresent.cardEffects.Count; i++)
        {
            switch (cardToRepresent.cardEffects[i].effectType)
            {
                case CardHelpers.EffectType.Attack:
                    if (!hasAtt)
                    {
                        Instantiate(AttackIconPrefab, AttributeIconParent.transform);
                        hasAtt = true;
                    }
                    break;
                case CardHelpers.EffectType.Block:
                    if (!hasBlo)
                    {
                        Instantiate(BlockIconPrefab, AttributeIconParent.transform);
                        hasBlo = true;
                    }
                    break;
                case CardHelpers.EffectType.Support:
                case CardHelpers.EffectType.DeckEdit:
                case CardHelpers.EffectType.Heal:
                case CardHelpers.EffectType.Battery:
                    if (!hasSup)
                    {
                        Instantiate(SupportIconPrefab, AttributeIconParent.transform);
                        hasSup = true;
                    }
                    break;
                case CardHelpers.EffectType.None:
                default:
                    break;
            }
        }
    }

    public string CardDescription(Card card)
    {
        string returnString = "";

        // If this is a trap card, add this first.
        if (card.isTrapCard)
        {
            returnString += "When you draw this card, ";
        }

        // Go through all the card effects and append their descriptions to the returnString.
        for (int i = 0; i < card.cardEffects.Count; i++)
        {
            returnString += DescriptionBuilder(card, i);
            if (card.cardEffects.Count > 1) returnString += Environment.NewLine;
        }

        // If we have a manager...
        if (myManager != null && !isEnemyCard)
        {
            // Check for a sticker on this part...
            Sticker tryForSticker = myManager.stateMachine.PlayerHand.gameObject.GetComponent<PartsManager>().allPartsComponents[(int)card.bodyPart].sticker;
            if (tryForSticker != null)
            {
                // Add the sticker description to this card.
                returnString += $"<color=#{ColorUtility.ToHtmlStringRGB(stickerTextColor)}>{tryForSticker.stickerDescription}</color>";
            }
        }

        return returnString;
    }

    private string DescriptionBuilder(Card card, int index)
    {
        string currentEffect = "";

        switch (card.cardEffects[index].effectType)
        {
            case CardHelpers.EffectType.Attack:
                // SPECIAL CASE: Reference block
                if (card.cardEffects[index].referenceVariable && card.cardEffects[index].variable == CardHelpers.ReferenceVariable.Block)
                {
                    currentEffect += myManager != null ? $"Deal Damage equal to your block ({myManager.stateMachine.PlayerHand.block})" : "Deal Damage equal to your block.";

                    int additive = SoftStatusCalc(card.cardEffects[index]);

                    if (additive != 0) currentEffect += $", +<color=#{ColorUtility.ToHtmlStringRGB(additive > 0 ? addedDamageColor : reducedDamageColor)}>{additive}</color>.";
                    else currentEffect += ".";

                    break;
                }
                // END SPECIAL CASE

                // SPECIAL CASE: Reference cards in deck
                if (card.cardEffects[index].referenceVariable && card.cardEffects[index].variable == CardHelpers.ReferenceVariable.CardsInDeck)
                {
                    currentEffect += myManager != null ? $"Deal Damage equal to the amount of cards in your deck ({myManager.stateMachine.PlayerHand.deck.Count})" : "Deal Damage equal to the amount of cards in your deck.";

                    int additive = SoftStatusCalc(card.cardEffects[index]);

                    if (additive != 0) currentEffect += $", +<color=#{ColorUtility.ToHtmlStringRGB(additive > 0 ? addedDamageColor : reducedDamageColor)}>{additive}</color>.";
                    else currentEffect += ".";

                    break;
                }
                // END SPECIAL CASE

                currentEffect += card.cardEffects[index].applyToSelf ? "Take " : "Deal ";
                currentEffect += SoftOutputCalc(card.cardEffects[index]);
                currentEffect += " Damage";
                currentEffect += card.cardEffects[index].applyMultipleTimes ? $" {card.cardEffects[index].effectNumberTimes} times." : ".";
                break;

            case CardHelpers.EffectType.Block:

                // SPECIAL CASE: Double your block. ----------------------------------------------------------------------------------
                if (card.cardEffects[index].referenceVariable && card.cardEffects[index].variable == CardHelpers.ReferenceVariable.Block)
                {
                    currentEffect += myManager != null ? $"Double your block ({myManager.stateMachine.PlayerHand.block})" : "Double your block.";

                    int additive = SoftStatusCalc(card.cardEffects[index]);

                    if (additive != 0)
                    {
                        currentEffect += $", +<color=#{ColorUtility.ToHtmlStringRGB(additive > 0 ? addedDamageColor : reducedDamageColor)}>{additive}</color>.";
                    }
                    else currentEffect += ".";

                    break;
                }
                // END SPECIAL CASE. -------------------------------------------------------------------------------------------------

                // SPECIAL CASE: Double your block. ----------------------------------------------------------------------------------
                if (card.cardEffects[index].referenceVariable && card.cardEffects[index].variable == CardHelpers.ReferenceVariable.CardsInDiscard)
                {
                    currentEffect += myManager != null ? $"Gain Block equal to the amount of cards in The Heap ({myManager.stateMachine.PlayerHand.discard.Count})" : "Gain Block equal to the amount of cards in The Heap.";

                    int additive = SoftStatusCalc(card.cardEffects[index]);

                    if (additive != 0) currentEffect += $", +<color=#{ColorUtility.ToHtmlStringRGB(additive > 0 ? addedDamageColor : reducedDamageColor)}>{additive}</color>.";
                    else currentEffect += ".";

                    break;
                }
                // END SPECIAL CASE

                currentEffect += "Gain ";
                currentEffect += SoftOutputCalc(card.cardEffects[index]);
                currentEffect += " Block";
                currentEffect += card.cardEffects[index].applyMultipleTimes ? $" {card.cardEffects[index].effectNumberTimes} times." : ".";
                break;

            case CardHelpers.EffectType.Heal:
                currentEffect += "Heal ";
                currentEffect += SoftOutputCalc(card.cardEffects[index]);
                currentEffect += " Health.";
                break;

            case CardHelpers.EffectType.Battery:
                currentEffect += "Gain ";
                currentEffect += SoftOutputCalc(card.cardEffects[index]);
                currentEffect += " Battery.";
                break;

            case CardHelpers.EffectType.Support:
                currentEffect += card.cardEffects[index].applyToSelf ? "Gain " : "Inflict ";
                currentEffect += SoftOutputCalc(card.cardEffects[index]);
                currentEffect += $" {card.cardEffects[index].statusToEdit}.";
                break;

            case CardHelpers.EffectType.DeckEdit:
                switch (card.cardEffects[index].DeckAction)
                {
                    case CardHelpers.DeckModify.DrawCard:
                        // SPECIAL CASE: Draw 1 card --------------------------
                        if (card.cardEffects[index].appliedStacks == 1)
                        {
                            currentEffect += "Draw a Card.";
                            break;
                        }
                        // END SPECIAL CASE -----------------------------------

                        currentEffect += "Draw ";
                        currentEffect += SoftOutputCalc(card.cardEffects[index]);
                        currentEffect += " Cards.";
                        break;

                    case CardHelpers.DeckModify.DrawFromDiscard:
                        // SPECIAL CASE: Return 1 card --------------------------
                        if (card.cardEffects[index].appliedStacks == 1)
                        {
                            currentEffect += "Return a Card from The Heap.";
                            break;
                        }
                        // END SPECIAL CASE -----------------------------------

                        currentEffect += "Return ";
                        currentEffect += SoftOutputCalc(card.cardEffects[index]);
                        currentEffect += " Cards from The Heap.";
                        break;

                    case CardHelpers.DeckModify.DrawFromAbandon:
                        // SPECIAL CASE: Return 1 card --------------------------
                        if (card.cardEffects[index].appliedStacks == 1)
                        {
                            currentEffect += "Return an Abandoned Card.";
                            break;
                        }
                        // END SPECIAL CASE -----------------------------------

                        currentEffect += "Return ";
                        currentEffect += SoftOutputCalc(card.cardEffects[index]);
                        currentEffect += " Abandoned Cards.";
                        break;

                    case CardHelpers.DeckModify.ShuffleDeck:
                        currentEffect += "Shuffle your Deck.";
                        break;

                    case CardHelpers.DeckModify.ShuffleDiscardIntoDeck:
                        currentEffect += "Shuffle The Heap into your Deck.";
                        break;

                    case CardHelpers.DeckModify.ForceDiscardCard:
                        currentEffect += "Discard a random card from your hand.";
                        break;

                    case CardHelpers.DeckModify.ForceDiscardHand:
                        currentEffect += "Discard your whole hand.";
                        break;

                    case CardHelpers.DeckModify.PlayRandomFromHand:
                        currentEffect += "Play a random card from your hand.";
                        break;

                    case CardHelpers.DeckModify.PlayRandomFromDeck:
                        currentEffect += "Play a random card from your Deck.";
                        break;

                    case CardHelpers.DeckModify.PlayRandomFromDiscard:
                        currentEffect += "Play a random card from your The Heap.";
                        break;

                    case CardHelpers.DeckModify.AddSpecificCardToDeck:
                        if (card.cardEffects[index].cardToAdd == card) // If this card adds a copy of itself to the deck.
                        {
                            currentEffect += "Add ";
                            currentEffect += SoftOutputCalc(card.cardEffects[index]);
                            currentEffect += $" {(card.cardEffects[index].appliedStacks > 1 ? "copies" : "copy")} of this card ";
                        }
                        else // If this card adds a different kind of card to the deck.
                        {
                            currentEffect += "Add ";
                            currentEffect += SoftOutputCalc(card.cardEffects[index]);
                            currentEffect += $" \"{card.cardEffects[index].cardToAdd.cardName}\"";
                        }
                        currentEffect += $"to your{(card.cardEffects[index].applyToSelf ? "" : "opponent's")} deck";
                        break;

                    case CardHelpers.DeckModify.AbandonThisCard:
                        currentEffect += "Abandon.";
                        break;

                    default:
                        break;
                }
                break;

            case CardHelpers.EffectType.None:
                currentEffect += "This card does nothing.";
                break;
            default:
                break;
        }

        return currentEffect;
    }

    private string SoftOutputCalc(CardEffect cardEffect)
    {
        int calcedDamage = cardEffect.appliedStacks;
        string returnString = calcedDamage.ToString();

        // If the stacks are actually based on a variable, grab it here
        if (cardEffect.referenceVariable)
        {
            switch (cardEffect.variable)
            {
                case CardHelpers.ReferenceVariable.Attack:
                    // This should stay blank because it doesn't mean anything!
                    // I just wasn't thinking but its more effort to remove it!
                    // Game Desig!!!!
                    break;
                case CardHelpers.ReferenceVariable.Block:
                    calcedDamage = myManager.stateMachine.PlayerHand.block;
                    break;
                case CardHelpers.ReferenceVariable.Health:
                    calcedDamage = myManager.stateMachine.PlayerHand.health;
                    break;
                case CardHelpers.ReferenceVariable.AttackCardsInHand:
                    calcedDamage = myManager.stateMachine.PlayerHand.handCounterList[0];
                    break;
                case CardHelpers.ReferenceVariable.BlockCardsInHand:
                    calcedDamage = myManager.stateMachine.PlayerHand.handCounterList[1];
                    break;
                case CardHelpers.ReferenceVariable.SupportCardsInHand:
                    calcedDamage = myManager.stateMachine.PlayerHand.handCounterList[2];
                    break;
                case CardHelpers.ReferenceVariable.CardsInDeck:
                    calcedDamage = myManager.stateMachine.PlayerHand.deck.Count;
                    break;
                case CardHelpers.ReferenceVariable.CardsInDiscard:
                    calcedDamage = myManager.stateMachine.PlayerHand.discard.Count;
                    break;
                default:
                    // Do Nothing on purpose.
                    break;
            }
        }

        if (cardEffect.effectType == CardHelpers.EffectType.Attack || cardEffect.effectType == CardHelpers.EffectType.Block)
        {
            calcedDamage += SoftStatusCalc(cardEffect);
        }

        calcedDamage = Math.Max(calcedDamage, 0);

        // If the damage isn't the base, we need to add the color around it. True is higher than base, false is lower.
        if (calcedDamage != cardEffect.appliedStacks)
        {
            returnString = $"<color=#{ColorUtility.ToHtmlStringRGB(calcedDamage > cardEffect.appliedStacks ? addedDamageColor : reducedDamageColor)}>{calcedDamage}</color>";
        }
        
        return returnString;
    }

    private int SoftStatusCalc(CardEffect cardEffect)
    {
        int statusDamage = 0;

        if (myManager != null)
        {
            if (!isEnemyCard)
            {
                statusDamage += myManager.stateMachine.PlayerHand.statusList[1]; // Add Potential
                statusDamage -= myManager.stateMachine.PlayerHand.statusList[2]; // Subtract Dull
                if (!cardEffect.applyToSelf) // Only factor in enemy statuses if we're not giving this to ourselves.
                {
                    if (myManager.stateMachine.ai.Length > 0)
                    {
                        if (cardEffect.effectType == CardHelpers.EffectType.Attack) statusDamage += myManager.stateMachine.ai[0].enemyDeck.statusList[4]; // If its an attack, add Enemy Fragile.
                        if (cardEffect.effectType == CardHelpers.EffectType.Attack) statusDamage -= myManager.stateMachine.ai[0].enemyDeck.statusList[5]; // If its an attack, subtract Enemy Reinforced.
                    }
                }
                else
                {
                    if (cardEffect.effectType == CardHelpers.EffectType.Attack) statusDamage += myManager.stateMachine.PlayerHand.statusList[4]; // If its an attack, add Enemy Fragile.
                    if (cardEffect.effectType == CardHelpers.EffectType.Attack) statusDamage -= myManager.stateMachine.PlayerHand.statusList[5]; // If its an attack, subtract Enemy Reinforced.
                }
            }
            else
            {
                statusDamage += myManager.stateMachine.ai[0].enemyDeck.statusList[1]; // Add Potential
                statusDamage -= myManager.stateMachine.ai[0].enemyDeck.statusList[2]; // Subtract Dull
                if (!cardEffect.applyToSelf) // Only factor in player statuses if we're not giving this to ourselves.
                {
                    if (cardEffect.effectType == CardHelpers.EffectType.Attack) statusDamage += myManager.stateMachine.PlayerHand.statusList[4]; // If its an attack, add Player Fragile.
                    if (cardEffect.effectType == CardHelpers.EffectType.Attack) statusDamage -= myManager.stateMachine.PlayerHand.statusList[5]; // If its an attack, subtract Player Reinforced.
                }
                else
                {
                    if (cardEffect.effectType == CardHelpers.EffectType.Attack) statusDamage += myManager.stateMachine.ai[0].enemyDeck.statusList[4]; // If its an attack, add Enemy Fragile.
                    if (cardEffect.effectType == CardHelpers.EffectType.Attack) statusDamage -= myManager.stateMachine.ai[0].enemyDeck.statusList[5]; // If its an attack, subtract Enemy Reinforced.
                }
            }
        }
        
        return statusDamage;
    }
}
