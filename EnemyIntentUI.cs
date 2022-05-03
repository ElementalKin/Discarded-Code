using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntentUI : MonoBehaviour
{
    public HorizontalLayoutGroup cardbackParent;

    public GameObject cardbackPrefab;

    public GameObject AttackIconPrefab;
    public GameObject BlockIconPrefab;
    public GameObject SupportIconPrefab;

    public CardHandUI cardHandUI;

    [HideInInspector]
    public bool okayToContinue = false;
    public float timeBeforeNextIntent = 3f;
    private float currTimer = 0;

    [HideInInspector]
    public int maxDamageThisTurn;

    private StateMachine stateMachine;

    private static readonly string cardFlip = "Flip";

    // Order in hierarchy:
    //  MainHolder
    //      Back [Grid Layout] - Enabled by default - Child 0
    //      Front [CardUI] - Disabled by default - Child 1
    //          Card Frame
    //              Card Elements
    //              Cost Attritube Icons

    private void Start()
    {
        if (stateMachine == null)
        {
            stateMachine = FindObjectOfType<StateMachine>();
        }
    }

    private void Update()
    {
        if (currTimer <= 0)
        {
            if (okayToContinue)
            {
                DisplayIntent();

                okayToContinue = false;
            }
        }
        else
        {
            currTimer -= Time.deltaTime;
        }
    }

    public void CalculateDamage()
    {
        if (stateMachine == null)
        {
            stateMachine = FindObjectOfType<StateMachine>();
        }

        maxDamageThisTurn = 0;

        Deck displayDeck = stateMachine.ai[0].enemyDeck;

        for (int i = 0; i < displayDeck.hand.Count; i++)
        {
            for (int j = 0; j < displayDeck.hand[i].cardEffects.Count; j++)
            {
                if (displayDeck.hand[i].cardEffects[j].effectType == CardHelpers.EffectType.Attack)
                {
                    int calc = displayDeck.hand[i].cardEffects[j].appliedStacks * Mathf.Max(1, displayDeck.hand[i].cardEffects[j].effectNumberTimes);
                    maxDamageThisTurn += calc;
                }
            }
        }

        //Debug.Log($"Expected Damage next turn: {maxDamageThisTurn}");
    }

    public void DisplayIntent()
    {
        if (stateMachine == null)
        {
            stateMachine = FindObjectOfType<StateMachine>();
        }

        //maxDamageThisTurn = 0;

        foreach (Transform child in cardbackParent.gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        Deck displayDeck = stateMachine.ai[0].enemyDeck;

        for (int i = 0; i < displayDeck.hand.Count; i++)
        {
            GameObject newCardback = Instantiate(cardbackPrefab, cardbackParent.transform);
            Transform newCardBackTrans = newCardback.transform.GetChild(0).transform;
            CardUI newCardFrontCardUI = newCardback.transform.GetChild(1).GetComponent<CardUI>();

            // Attatch the attribute icons to the back of the cards.
            bool hasAtt = false, hasBlo = false, hasSup = false;
            for (int j = 0; j < displayDeck.hand[i].cardEffects.Count; j++)
            {
                switch (displayDeck.hand[i].cardEffects[j].effectType)
                {
                    case CardHelpers.EffectType.Attack:
                        if (!hasAtt)
                        {
                            Instantiate(AttackIconPrefab, newCardBackTrans);
                            hasAtt = true;

                            //int calc = displayDeck.hand[i].cardEffects[j].appliedStacks * Mathf.Max(1, displayDeck.hand[i].cardEffects[j].effectNumberTimes);
                            //maxDamageThisTurn += calc;
                        }
                        break;
                    case CardHelpers.EffectType.Block:
                        if (!hasBlo)
                        {
                            Instantiate(BlockIconPrefab, newCardBackTrans);
                            hasBlo = true;
                        }
                        break;
                    case CardHelpers.EffectType.Support:
                    case CardHelpers.EffectType.DeckEdit:
                    case CardHelpers.EffectType.Heal:
                    case CardHelpers.EffectType.Battery:
                        if (!hasSup)
                        {
                            Instantiate(SupportIconPrefab, newCardBackTrans);
                            hasSup = true;
                        }
                        break;
                    case CardHelpers.EffectType.None:
                    default:
                        break;
                }
            }

            // Set up the CardUI on the front.
            newCardFrontCardUI.isEnemyCard = true;
            newCardFrontCardUI.DisplayCard(displayDeck.hand[i], cardHandUI);
        }

        cardHandUI.UpdatePlayerStats(stateMachine.PlayerHand.health, stateMachine.PlayerHand.block);
    }

    public void FlipCards()
    {
        foreach (Transform child in cardbackParent.gameObject.transform)
        {
            if (child.gameObject.TryGetComponent(out Animator cardBackAnimator))
            {
                cardBackAnimator.SetTrigger(cardFlip);
                currTimer = timeBeforeNextIntent;
            }
        }
    }
}