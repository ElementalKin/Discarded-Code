using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardHandUI : MonoBehaviour
{
    public StateMachine stateMachine;
    public CardFunctionality cardPlayer;
    public EnemyIntentUI enemyIntentUI;

    private Card selectedCard;

    public GameObject cardPrefab;
    [HideInInspector]
    public CardUI selectedCardUI;

    public HorizontalLayoutGroup cardHand;
    public GameObject cardSelectedParent;

    public Button playCardZone;
    public Button returnCardZone;

    public Button playButton;
    public Button endTurnButton;
    public GameObject endTurnFX;

    public bool UIIsUp => anim.GetBool("OpenUI");

    public Slider minimapSlider;
    public static readonly float[] minimapLocations = new float[]
    {
        0.00f,
        1.25f,
        2.00f,
        2.75f,
        3.65f,
        4.80f,
        5.65f,
        6.50f,
        7.44f,  // >::)
        9.00f
    };

    [Header("Player Deck Stats")]
    public TextMeshProUGUI deckCounterTex;
    public TextMeshProUGUI discardCounterTex;
    public TextMeshProUGUI abandonCounterTex;
    public Image batteryAmountImage;
    public TextMeshProUGUI batteryAmountTex;
    public Sprite[] batteryAmountSprites;
    public Color[] batteryAmountColors;

    [Header("Player Stats")]
    public Image playerHealthSlider;
    public Image playerIntentSlider;
    public TextMeshProUGUI playerHealthTex;
    public GameObject playerBlockGameObject;
    public TextMeshProUGUI playerBlockTex;
    public StatusEffectUI[] playerStatuses;

    [Header("Enemy Stats")]
    public Image enemyHealthSlider;
    public TextMeshProUGUI enemyHealthTex;
    public GameObject enemyBlockGameObject;
    public TextMeshProUGUI enemyBlockTex;
    public StatusEffectUI[] enemyStatuses;

    [Header("Misc Stats")]
    public int drawnCards;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Sets up the first turn for the player's hand by setting moves to max,
    /// setting the proper FX, and setting up status effect tooltips.
    /// </summary>
    /// <param name="cardList">List of Cards to make into UI Elements.</param>
    public void DisplayHand(List<Card> cardList)
    {
        if (endTurnFX != null)
        {
            endTurnFX.SetActive(false);
        }

        PopulateCards(cardList);

        // The enemy intent handles it's own showing since its now on a timer.
        enemyIntentUI.okayToContinue = true;
        enemyIntentUI.CalculateDamage();

        playButton.gameObject.SetActive(false);

        batteryAmountImage.sprite = batteryAmountSprites[stateMachine.moves];
        batteryAmountImage.color = batteryAmountColors[stateMachine.moves];
        batteryAmountTex.text = $"{stateMachine.moves} / {stateMachine.maxMoves}";

        minimapSlider.value = minimapLocations[Mathf.Clamp(SceneManagment.numberOfBattles + 1, 1, minimapLocations.Length - 1)];

        foreach (var enemyStatus in enemyStatuses)
        {
            if (enemyStatus.gameObject.TryGetComponent(out DynamicTooltipElement tooltip))
            {
                tooltip.deckToRepresent = stateMachine.ai[0].enemyDeck;
            }
        }
    }

    public void ToggleBattleUI(bool toggleTo)
    {
        anim.SetBool("OpenUI", toggleTo);
    }

    /// <summary>
    /// Use the given card list to create new GameObjects with the
    /// CardUI Script on them childed to the CardHand.
    /// </summary>
    /// <param name="cardList">List of Cards to make into UI Elements.</param>
    private void PopulateCards(List<Card> cardList)
    {
        ClearCards();

        drawnCards = 0;

        for (int i = 0; i < cardList.Count; i++)
        {
            // Instantiate a new card and set all the relevant variables.
            CardUI newCard = Instantiate(cardPrefab, cardHand.gameObject.transform).GetComponent<CardUI>();

            newCard.DisplayCard(cardList[i], this);
            newCard.selectCardButton.onClick.AddListener(delegate { SelectCard(newCard); });
            newCard.handParent = cardHand.gameObject;
            newCard.selectionParent = cardSelectedParent;

            drawnCards++;
        }

        // Set the count of all the Card piles (deck, discard, abandon).
        deckCounterTex.text = stateMachine.PlayerHand.deck.Count.ToString();
        discardCounterTex.text = stateMachine.PlayerHand.discard.Count.ToString();
        //abandonCounterTex.text = stateMachine.PlayerHand.abandon.Count.ToString();

        batteryAmountImage.gameObject.transform.GetChild(0).gameObject.SetActive(stateMachine.moves > 3);

        ReturnSelectedCard();
    }

    public void SelectCard(CardUI selected)
    {
        if ((stateMachine.moves > 0 && stateMachine.moves >= selected.cardToRepresent.cardCost) || selected.cardToRepresent.cardCost == 0)
        {
            selected.SelectCard();
            selectedCardUI = selected;

            selectedCard = selected.cardToRepresent;

            playCardZone.gameObject.SetActive(true);
            returnCardZone.gameObject.SetActive(true);
        }
    }

    public void PlaySelectedCard()
    {
        if (selectedCard != null)
        {
            if ((stateMachine.moves > 0 && stateMachine.moves >= selectedCard.cardCost) || selectedCard.cardCost == 0)
            {
                //stateMachine.PlayerHand.gameObject.GetComponent<PartsManager>().HandCardToHeadPart(selectedCard);

                cardPlayer.playCard(selectedCard, stateMachine.ai[0].enemyDeck, stateMachine.PlayerHand);
                stateMachine.PlayerHand.gameObject.GetComponent<PartsManager>().PlayAnim(selectedCard.animationOnPlay.ToString());

                if (stateMachine.moves < 0) stateMachine.moves = 0;

                // Set the battery Sprite, Color, and the text above it.
                batteryAmountImage.sprite = batteryAmountSprites[Mathf.Min(stateMachine.moves, batteryAmountSprites.Length - 1)];
                batteryAmountImage.color = batteryAmountColors[Mathf.Min(stateMachine.moves, batteryAmountSprites.Length - 1)];
                batteryAmountTex.text = $"{stateMachine.moves} / {stateMachine.maxMoves}";

                // Disable the play button.
                playButton.gameObject.SetActive(false);

                drawnCards--;

                stateMachine.PlayerHand.DiscardUsedCard(selectedCard);
                Destroy(selectedCardUI.gameObject);
                //Debug.Log("Card Played!");

                // Refresh UI
                PopulateCards(stateMachine.PlayerHand.hand);
            }

            foreach (Transform child in cardHand.gameObject.transform)
            {
                if (child.gameObject.TryGetComponent(out CardUI cardUI))
                {
                    if (cardUI.cardToRepresent.cardCost != 0)
                    {
                        cardUI.selectCardButton.interactable = stateMachine.moves > 0;
                    }
                }
            }

            if (stateMachine.moves == 0)
            {
                if (endTurnFX != null)
                {
                    endTurnFX.SetActive(true);
                }
            }
        }
    }

    public void ReturnSelectedCard()
    {
        if (selectedCardUI != null) selectedCardUI.ReturnCard();
        selectedCardUI = null;

        selectedCard = null;

        playCardZone.gameObject.SetActive(false);
        returnCardZone.gameObject.SetActive(false);
    }

    public void UpdatePlayerStats(float health, int block)
    {
        playerHealthTex.text = Mathf.Max(health, 0).ToString() + " / " + stateMachine.PlayerHand.maxHealth.ToString();
        playerHealthSlider.fillAmount = health / (float)stateMachine.PlayerHand.maxHealth;

        playerBlockTex.text = block.ToString();
        playerBlockGameObject.SetActive(stateMachine.PlayerHand.block > 0);

        // Update the enemy's intent on the player.
        float calcedIntent = (enemyIntentUI.maxDamageThisTurn - block + stateMachine.ai[0].enemyDeck.statusList[1] - stateMachine.ai[0].enemyDeck.statusList[2] + stateMachine.PlayerHand.statusList[4] - stateMachine.PlayerHand.statusList[5]);
        playerIntentSlider.fillAmount = (calcedIntent / (float)stateMachine.PlayerHand.maxHealth) + (1 - playerHealthSlider.fillAmount);

        // Update statuses
        foreach (StatusEffectUI statusUI in playerStatuses)
        {
            statusUI.UpdateStackAmount(stateMachine.PlayerHand);
        }
    }

    public void UpdateEnemyStats(float health, int block)
    {
        enemyHealthTex.text = Mathf.Max(health, 0).ToString() + " / " + stateMachine.ai[0].enemyDeck.maxHealth.ToString();
        enemyHealthSlider.fillAmount = health / (float)stateMachine.ai[0].enemyDeck.maxHealth;

        enemyBlockTex.text = block.ToString();
        enemyBlockGameObject.SetActive(stateMachine.ai[0].enemyDeck.block > 0);

        // Update statuses
        foreach (StatusEffectUI statusUI in enemyStatuses)
        {
            statusUI.UpdateStackAmount(stateMachine.ai[0].enemyDeck);
        }
    }

    /// <summary>
    /// UNUSED FUNCTION. Staying for reference.
    /// </summary>
    public void DiscardSelectedCard()
    {
        stateMachine.PlayerHand.DiscardUsedCard(selectedCard);
        Destroy(selectedCardUI.gameObject);
        //Debug.Log("Card Discarded!");

        playButton.gameObject.SetActive(false);

        drawnCards--;
    }

    /// <summary>
    /// Called via a Unity Button Event.
    /// Clear all cards and tells the state machine to move onto the enemy's turn.
    /// </summary>
    public void EndTurn()
    {
        ClearCards();

        enemyIntentUI.FlipCards();

        stateMachine.EnemyTurn();
        //Debug.Log("Turn Ended!");
    }

    /// <summary>
    /// Clear all GameObjects from CardHand.
    /// </summary>
    private void ClearCards()
    {
        foreach (Transform child in cardHand.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}