using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Deck : MonoBehaviour
{
    //Beginning battle variables
    public TextMeshProUGUI TEMPTEXTREMOVEMELATER;
    public bool dontSendPartCardsToDeck;
    [HideInInspector]
    public bool isEnemyDeck = false;
    public bool beginningOfCombat;
    public int startDrawAmount = 3;
    public int normalStartDrawAmount = 5;

    private bool AbandonPlayed = false;
    public EnemyAI enAI;

    [SerializeField] AudioCollection audCollection;

    /// <summary>
    /// Counters for future use with status effects 
    /// Counts the amount of types in deck, hand, discard and support
    /// </summary>

    /// <summary>
    /// deckAttackTypeCounter   0
    /// deckBlockTypeCounter    1
    /// deckHealTypeCounter     2
    /// deckSupportTypeCounter  3
    /// deckEditTypeCounter     4
    /// deckNoneTypeCounter     5
    /// </summary>
    public List<int> deckCounterList = new List<int>() { 0, 0, 0, 0, 0, 0,};

    /// <summary>
    /// handAttackTypeCounter   0
    /// handBlockTypeCounter    1
    /// handHealTypeCounter     2
    /// handSupportTypeCounter  3
    /// handEditTypeCounter     4
    /// handNoneTypeCounter     5
    /// </summary>
    public List<int> handCounterList = new List<int>() { 0, 0, 0, 0, 0, 0, };

    /// <summary>
    /// discardAttackTypeCounter    0
    /// discardBlockTypeCounter     1
    /// discardHealTypeCounter      2
    /// discardSupportTypeCounter   3
    /// discardEditTypeCounter      4
    /// discardNoneTypeCounter      5
    /// </summary>
    public List<int> discardCounterList = new List<int>() { 0, 0, 0, 0, 0, 0, };

    /// <summary>
    /// None             0,
    /// Potential        1,
    /// Dull             2,
    /// Squeeze          3,
    /// Fragile          4,
    /// Reinforce        5,
    /// Revenge          6,
    /// BOGO             7,
    /// Tattered         8,
    /// Regen            9,
    /// TerrainScanners  10,
    /// </summary>
    public List<int> statusList = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    //public int finisherTypeCounter = 0;

    //[HideInInspector]
    public int health;
    public int maxHealth = 20;

    public int block = 0;
    public int potential = 0;

    public List<Card> cards = new List<Card>();
    
    //public Card placeHolder;
    //Update list when cards in hand are played 
    public List<Card> hand = new List<Card>();
    public List<Card> deck = new List<Card>();
    public List<Card> discard = new List<Card>();
    public List<Card> abandon = new List<Card>();

    public List<Card> enemyCardTemp = new List<Card>();
    public Card bufferCard = null;

    private CardFunctionality cardPlayer;
    public StateMachine state;
    //TEMP VALUES

    //public Card cardTemp;
    //public GameObject headPart;
    //public GameObject leftArmPart;
    //public GameObject rightArmPart;
    //public GameObject torsoPart;
    //public GameObject legsPart;
    //public GameObject backPart;

    //public GameObject[] bodyParts;

    //public GameObject part = gameObject.GetComponent<PartsManager>().partCards


    // Deck won't be started until scene loads.
    void Awake()
    {
        //EffectTypeCounter(deck);
        health = maxHealth;
    }

    private void Start()
    {
        cardPlayer = FindObjectOfType<CardFunctionality>();
        normalStartDrawAmount = startDrawAmount;
        if (TEMPTEXTREMOVEMELATER != null)
        {
            Destroy(TEMPTEXTREMOVEMELATER.gameObject);
        }
    }

    //For Testing TEMP
    private void Update()
    {
        // TODO REMOVE ME!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //string statusPrintout = $"Potential: {statusList[1]}\nDull: {statusList[2]}\nSqueeze: {statusList[3]}\nFragile: {statusList[4]}\nReinforce: {statusList[5]}\nRevenge: {statusList[6]}\nBOGO: {statusList[7]}\n";
    }
    //TEMP
    public void EffectTypeCounter()
    {
        //resets all the counters
        deckCounterList[0] = 0;
        deckCounterList[1] = 0;
        deckCounterList[2] = 0;
        deckCounterList[3] = 0;
        deckCounterList[4] = 0;
        deckCounterList[5] = 0;

        handCounterList[0] = 0;
        handCounterList[1] = 0;
        handCounterList[2] = 0;
        handCounterList[3] = 0;
        handCounterList[4] = 0;
        handCounterList[5] = 0;

        discardCounterList[0] = 0;
        discardCounterList[1] = 0;
        discardCounterList[2] = 0;
        discardCounterList[3] = 0;
        discardCounterList[4] = 0;
        discardCounterList[5] = 0;


        // For each card in the deck switch on card effect type
        // check each card in effects array and switch on the effect type and depending on what that is
        // increment proper variable
        // Need to clean up further

        // Deck effect Type Counter
        foreach (Card counter in deck)
        {
            foreach (CardEffect cardeffectcard in counter.cardEffects)
            {
                switch (cardeffectcard.effectType)
                {
                    case CardHelpers.EffectType.Attack:
                        deckCounterList[0] += 1;
                        break;
                    case CardHelpers.EffectType.Block:
                        deckCounterList[1] += 1;
                        break;
                    case CardHelpers.EffectType.Heal:
                        deckCounterList[2] += 1;
                        break;
                    case CardHelpers.EffectType.Support:
                        deckCounterList[3] += 1;
                        break;
                    case CardHelpers.EffectType.DeckEdit:
                        deckCounterList[4] += 1;
                        break;
                    case CardHelpers.EffectType.None:
                        deckCounterList[5] += 1;
                        break;
                    default:
                        Debug.Log("Something went wrong with the switchcase for cardeffects," +
                            " type not none or any current");
                        break;
                }
            }
        }

        // Hand effect Type Counter
        foreach (Card counter in hand)
        {
            foreach (CardEffect cardeffectcard in counter.cardEffects)
            {
                switch (cardeffectcard.effectType)
                {
                    case CardHelpers.EffectType.Attack:
                        handCounterList[0] += 1;
                        break;
                    case CardHelpers.EffectType.Block:
                        handCounterList[1] += 1;
                        break;
                    case CardHelpers.EffectType.Heal:
                        handCounterList[2] += 1;
                        break;
                    case CardHelpers.EffectType.Support:
                        handCounterList[3] += 1;
                        break;
                    case CardHelpers.EffectType.DeckEdit:
                        handCounterList[4] += 1;
                        break;
                    case CardHelpers.EffectType.None:
                        handCounterList[5] += 1;
                        break;
                    default:
                        Debug.Log("Something went wrong with the switchcase for cardeffects," +
                            " type not none or any current");
                        break;
                }
            }
        }

        // Discard effect Type Counter
        foreach (Card counter in discard)
        {
            foreach (CardEffect cardeffectcard in counter.cardEffects)
            {
                switch (cardeffectcard.effectType)
                {
                    case CardHelpers.EffectType.Attack:
                        discardCounterList[0] += 1;
                        break;
                    case CardHelpers.EffectType.Block:
                        discardCounterList[1] += 1;
                        break;
                    case CardHelpers.EffectType.Heal:
                        discardCounterList[2] += 1;
                        break;
                    case CardHelpers.EffectType.Support:
                        discardCounterList[3] += 1;
                        break;
                    case CardHelpers.EffectType.DeckEdit:
                        discardCounterList[4] += 1;
                        break;
                    case CardHelpers.EffectType.None:
                        discardCounterList[5] += 1;
                        break;
                    default:
                        Debug.Log("Something went wrong with the switchcase for cardeffects," +
                            " type not none or any current");
                        break;
                }
            }
        }

    }

    /// <summary>
    /// Call on start of statemachine, grabs card lists from parts and puts into hand
    /// </summary>
    public void BuildDeck()
    {
        // Edit by Jay- sorry for the unannounced edit, just gettin this 
        // up to date with the implementation of the partsManager!
        if (!dontSendPartCardsToDeck)
        {
            deck = GetComponent<PartsManager>().RefreshParts();
        }

        ShuffleDeck();

        // Grab decklist from player/AI attached hand ()
        //foreach (GameObject part in bodyParts)
        //{
        //    // TryGetComponent returns a bool true/false depending on if the game object
        //    // actually has that component or not. So first we check to make sure the
        //    // part has a PartsManager on it. If it does, we add all the cards.
        //    // If it doesnt, we just don't do anything with that part.
        //    if (part.TryGetComponent<PartsManager>(out PartsManager partMan))
        //    {
        //        for (int i = 0; i < partMan.partCards.Count; i++)
        //        {
        //            deck.Add(partMan.partCards[i]);
        //        }
        //    }
        //}

        /// <summary>
        ///for (int i = 0; i < headPart.gameObject.GetComponent<PartsManager>().partCards.Count; i++)
        ///{
        ///    deck.Add(headPart.gameObject.GetComponent<PartsManager>().partCards[i]);
        ///}
        ///for (int i = 0; i < leftArmPart.gameObject.GetComponent<PartsManager>().partCards.Count; i++)
        ///{
        ///    deck.Add(leftArmPart.gameObject.GetComponent<PartsManager>().partCards[i]);
        ///}
        ///for (int i = 0; i < rightArmPart.gameObject.GetComponent<PartsManager>().partCards.Count; i++)
        ///{
        ///    deck.Add(rightArmPart.gameObject.GetComponent<PartsManager>().partCards[i]);
        ///}
        ///for (int i = 0; i < torsoPart.gameObject.GetComponent<PartsManager>().partCards.Count; i++)
        ///{
        ///    deck.Add(torsoPart.gameObject.GetComponent<PartsManager>().partCards[i]);
        ///}
        ///for (int i = 0; i < legsPart.gameObject.GetComponent<PartsManager>().partCards.Count; i++)
        ///{
        ///    deck.Add(legsPart.gameObject.GetComponent<PartsManager>().partCards[i]);
        ///}
        ///for (int i = 0; i < backPart.gameObject.GetComponent<PartsManager>().partCards.Count; i++)
        ///{
        ///    deck.Add(backPart.gameObject.GetComponent<PartsManager>().partCards[i]);
        ///}
        /// </summary>
    }


    public void buildEnemyDeck()
    {
        deck.Clear();

        for (int i = 0; i < enAI.enemyAIStruct.Count; i++)
        {
            if (enAI.enemyAIStruct[i].slot1 != null)
            {
                deck.Add(enAI.enemyAIStruct[i].slot1);
            }
            else
            {
                deck.Add(bufferCard);
            }
            if (enAI.enemyAIStruct[i].slot2 != null)
            {
                deck.Add(enAI.enemyAIStruct[i].slot2);
            }
            else
            {
                deck.Add(bufferCard);
            }
            if (enAI.enemyAIStruct[i].slot3 != null)
            {
                deck.Add(enAI.enemyAIStruct[i].slot3);
            }
            else
            {
                deck.Add(bufferCard);
            }
            if (enAI.enemyAIStruct[i].slot4 != null)
            {
                deck.Add(enAI.enemyAIStruct[i].slot4);
            }
            else
            {
                deck.Add(bufferCard);
            }

        }
    }



    public void grabEnemyDeck()
    {
        int idx = 0;

        for (int i = 0; i < enAI.enemyAIStruct.Count; i++)
        {
            //Debug.Log("i =" + i);
            //Debug.Log("idx =" + idx);

            enAI.enemyAIStruct[i].slot1 = deck[idx];
            idx++;
            //Debug.Log("idx =" + idx);
            enAI.enemyAIStruct[i].slot2 = deck[idx];
            idx++;
            //Debug.Log("idx =" + idx);
            enAI.enemyAIStruct[i].slot3 = deck[idx];
            idx++;
            //Debug.Log("idx =" + idx);
            enAI.enemyAIStruct[i].slot4 = deck[idx];
            idx++;
            //Debug.Log("idx =" + idx);
        }
    }

    // Returns card info from the current slot
    // UI button passes 0-4(5) into this, uses said info for discardUse or Abandon/ aka specific card situations
    public Card HandSlot(int cardSlot)
    {
        return hand[cardSlot];
    }

    public Card DiscardSlot(int discardSlot)
    {
        return discard[discardSlot];
    }

    public Card DeckSlot(int deckSlot)
    {
        return deck[deckSlot];
    }

    // Returns the card from the top of the deck
    public Card TopDeck()
    {
        if (deck.Count <= 0)
        {
            shuffleDiscardIntoDeck();
        }
        return deck[0];
    }

    // Puts card into your hand from top of deck, deletes card from top of the deck, plays card in hand and deletes it
    public void playTopCard()
    {
        hand.Add(TopDeck());
        deck.Remove(TopDeck());
        cardPlayer.playCard(hand[hand.Count - 1], state.PlayerHand, this);
        hand.RemoveAt(hand.Count - 1);
    }

    // If trap card is drawn, this should be called after
    // 
    public void trapCardDrawn()
    {
        hand.Add(deck[0]);
        deck.RemoveAt(0);
        cardPlayer.playCard(hand[hand.Count - 1], this, this);
    }

    // Normal shuffle not caused by cards, that makes sure finisher cards are at the bottom of the deck
    public void NaturalShuffle(List<Card> deck)
    {
        if (isEnemyDeck != true)
        {
            AudioManager.instance.DeckSounds(0);
        }
        List<Card> tempFinishers = new List<Card>();

        // Checks for finishers and pulls them into separate list and removes from deck
        for (int i = 0; i < deck.Count-1; i++)
        {
            if (deck[i].isFinisher)
            {
                tempFinishers.Add(deck[i]);
                deck.RemoveAt(i);
            }
        }


        // Shuffles deck without finishers in it
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIdx = Random.Range(i, deck.Count - 1);
            Card temp = deck[i];
            deck[i] = deck[randomIdx];
            deck[randomIdx] = temp;
        }

        // Adds finisher cards to back of list (bottom of the deck)
        for (int i = 0; i < tempFinishers.Count - 1; i++)
        {
            deck.Insert(0, tempFinishers[i]);
        }
    }

    // Forced Shuffle of deck (aka normal shuffle)
    public void ShuffleDeck()
    {
        if (isEnemyDeck != true)
        {
            // Checks for beginning of combat, so the deck shuffle sound does't play in the start of Combat
            if (beginningOfCombat != true)
            {
                AudioManager.instance.DeckSounds(0);
            }

            //audSource.PlayOneShot(audCollection.GetAudioClip(0));
            //FindObjectOfType<AudioManager>().playRandPitch("Deck Shuffle");
        }
        for (int i = 0; i < deck.Count; i++)
        {
            int randomIdx = Random.Range(i, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[randomIdx];
            deck[randomIdx] = temp;
        }
    }

    // Copy discard pile into deck, delete discard pile
    //call shuffle function
    public void shuffleDiscardIntoDeck()
    {
        AudioManager.instance.DeckSounds(0);
        for (int i = 0; i < discard.Count; i++)
        {
            deck.Add(discard[i]);
        }
        discard.Clear();
        ShuffleDeck();
    }

    public void enemyshuffleDiscardIntoDeck()
    {
        deck.Clear();
        discard.Clear();
        buildEnemyDeck();
    }

    //public void addCardOpponentDeck(Card card, Deck opDeck)
    //{
    //    opDeck.deck.Insert(Random.Range(0, opDeck.deck.Count), card);
    //}

    public void ConfuseAI()
    {
        statusList[10] = 3;
        ShuffleDeck();
    }

    // for drawing 
    public void Draw()
    {
        if (isEnemyDeck != true)
        {
            if (deck.Count > 0)
            {
                if (deck[0].isTrapCard == false)
                {
                    // Adds copy of the top card to hand then removes top card of the deck
                    hand.Add(deck[0]);
                    AudioManager.instance.DeckSounds(1);
                    deck.RemoveAt(0);
                }
                else
                {
                    trapCardDrawn();
                }
            }
            else
            {
                // If out of cards in deck, shuffle from dicsard into deck
                // Copy discard pile into deck, delete discard pile
                // call shuffle function
                shuffleDiscardIntoDeck();
                Draw();
            }
        }
        else
        {
            if (deck.Count > 0)
            {
                if (deck[0] != null)
                {
                    if (deck[0].isTrapCard == false)
                    {
                        // Adds copy of the top card to hand then removes top card of the deck
                        hand.Add(deck[0]);
                        //FindObjectOfType<AudioManager>().Play("Draw Card");
                        //Maybe add back at a later date (draw sound for AI)
                        deck.RemoveAt(0);
                    }
                    else
                    {
                        trapCardDrawn();
                    }
                }
                else
                {
                    deck.RemoveAt(0);
                }
            }
            else
            {
                enemyshuffleDiscardIntoDeck();
                Draw();
            }
        }

        //if (deck.Count > 0)
        //{
        //     if (deck[0].isTrapCard == false)
        //     {
        //         // Adds copy of the top card to hand then removes top card of the deck
        //         hand.Add(deck[0]);
        //         deck.RemoveAt(0);
        //     }
        //     else
        //     {
        //         // If trap card, play another card from top of the deck 
        //         trapCardDrawn();
        //     }
        // }
        //else
        //{
        //     // Copy discard pile into deck, delete discard pile
        //     //call shuffle function
        //     shuffleDiscardIntoDeck();
        //     Draw();
        //}

    }

    public void StartDraw()
    {
        if (isEnemyDeck != true)
        {
            if (beginningOfCombat != true)
            {
                AudioManager.instance.DeckSounds(2);
            }
            for (int i = 0; i < startDrawAmount; i++)
            {
                if (deck.Count > 0)
                {
                    if (deck[0].isTrapCard == false)
                    {
                        // Adds copy of the top card to hand then removes top card of the deck
                        hand.Add(deck[0]);


                        deck.RemoveAt(0);
                    }
                    else
                    {
                        trapCardDrawn();
                    }

                    // Check if its a trap card, add to hand, remove from deck


                    // If out of cards in deck, shuffle from dicsard into deck
                    // Copy discard pile into deck, delete discard pile
                    // call shuffle function
                    if (deck.Count <= 0)
                    {
                        shuffleDiscardIntoDeck();
                    }

                }
                else
                {
                    // If out of cards in deck, shuffle from dicsard into deck
                    // Copy discard pile into deck, delete discard pile
                    // call shuffle function
                    shuffleDiscardIntoDeck();
                    if (deck.Count > 0)
                    {
                        if (deck[0].isTrapCard == false)
                        {
                            // adds copy of the top card to hand then removes top card of the deck
                            hand.Add(deck[0]);
                            deck.RemoveAt(0);
                        }
                        else
                        {
                            trapCardDrawn();
                        }
                    }
                }
            }
            startDrawAmount = normalStartDrawAmount;
        }   // Start Draw for enemy AI
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (deck.Count > 0)
                {
                    if (deck[0] != null)
                    {
                        if (deck[0].isTrapCard == false)
                        {
                            // Adds copy of the top card to hand then removes top card of the deck
                            hand.Add(deck[0]);
                            deck.RemoveAt(0);
                        }
                        else
                        {
                            trapCardDrawn();
                        }
                    }
                    else
                    {
                        deck.RemoveAt(0);
                    }
                }
                else
                {

                }
            }

        }
    }

    // if Card has status call this
    //public void statusIncrement(Card card, int statusCount)
    //{
    //    if (card.cardEffects != null)
    //    {

    //        // MAKE A SWITCH CASE LATER PLEASE DEVON -Devon
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.None)
    //        {
    //            statusNone += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.SupportAttack)
    //        {
    //            statusSupportAttack += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.SupportBlock)
    //        {
    //            statusSupportBlock += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.SupportAllStats)
    //        {
    //            statusSupportAllStats += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.Potential)
    //        {
    //            statusPotential += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.AutoFire)
    //        {
    //            statusAutoFire += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.RevShield)
    //        {
    //            statusRevShield += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.Exposed)
    //        {
    //            statusExposed += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.WindUp)
    //        {
    //            statusWindUpsNone += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.HollowFrame)
    //        {
    //            statusHollowFrame += statusCount;
    //        }
    //        if (card.cardEffects[0].effectModifier == CardHelpers.ModifierVariable.TerrainScanners)
    //        {
    //            statusTerrainScanners += statusCount;
    //        }
    //        //CardHelpers.ModifierVariable.Exposed
    //        //  card.cardEffects[0].effect.
    //    }
    //}

    // Discards card after it's used

    /// <summary>
    /// checks if you have cards in hand, if so, adds card to discard pile and removes it from hand
    /// </summary>
    /// <param name="card"></param>
    public void DiscardUsedCard(Card card)
    {
        if (hand.Count <= 0)
        {
            Debug.Log("There are no cards left in your hand");
        }
        else
        {
            if (isEnemyDeck != true)
            {
                AudioManager.instance.DeckSounds(3);
            }

            if (AbandonPlayed != true)   
            {
                // Deletes same card from hand, is non specific between two cards with the same name
                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i] == card)
                    {

                        // Adds card to top of discard
                        discard.Add(card);

                        // Removes used card from hand
                        hand.Remove(card);

                        // Prevents deletion of multiple of the same card
                        break;
                    }
                }
            }
            AbandonPlayed = false;
        }
    }

    public void DrawFromDiscardRandom()
    {
        if (discard.Count > 0)
        {
            if (isEnemyDeck != true)
            {
                AudioManager.instance.DeckSounds(1);
            }
            int randomDiscardCard = Random.Range(0, discard.Count - 1);
            hand.Add(discard[randomDiscardCard]);
            discard.RemoveAt(randomDiscardCard);
        }
        else
        {
            Debug.Log("You have dumb.");
        }

    }



    /// <summary>
    /// Discards entire hand
    /// </summary>
    public void DiscardHand()
    {
        for (int i = 0; i <= hand.Count - 1; i++)
        {
            discard.Add(hand[i]);
        }
        hand.Clear();
    }

    // Pulls from top of discard pile into hand

    /// <summary>
    /// Pulls a card from the top of the discard pile and adds it to hand
    /// </summary>
    /// <param name="card"></param>
    public void DiscardPull()
    {
        if (isEnemyDeck != true)
        {
            AudioManager.instance.DeckSounds(1);
        }
        // Adds top card of deck to hand
        hand.Add(discard[0]);

        // Removes top card from discard
        discard.RemoveAt(0);
    }
    
    /// <summary>
    /// Adds given card to abandon pile, removes from player hand
    /// </summary>
    /// <param name="card"></param>
    public void Abandon(Card card)
    {
        AbandonPlayed = true;
        // Adds card to top of discard
        abandon.Add(card);
        if (isEnemyDeck != true)
        {
            AudioManager.instance.DeckSounds(4);
        }

        // Removes used card from hand
        hand.Remove(card);
    }

    public void AbandonDraw()
    {

        if (abandon.Count > 0)
        {
            hand.Add(abandon[0]);
            if (isEnemyDeck != true)
            {
                AudioManager.instance.DeckSounds(1);
            }
            abandon.RemoveAt(0);
        }
    }

    // Adds given card to given deck at a random position
    public void addCardOpponentDeck(Card card, Deck opDeck)
    {
        opDeck.deck.Insert(Random.Range(0, opDeck.deck.Count), card);
    }
}
