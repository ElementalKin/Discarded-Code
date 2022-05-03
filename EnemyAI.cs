using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class EnemyAI : MonoBehaviour
{
    [Header("Card Slots")]
    [HideInInspector]
    public List<EnemyAIStruct> enemyAIStruct = new List<EnemyAIStruct>();

    public Deck enemyDeck;

    public GameObject[] partDrops;

    public StateMachine state;
    public CardFunctionality cardPlayer;

    public void Start()
    {
        enemyDeck.isEnemyDeck = true;
    }

    /// <summary>
    /// Checks if the card in the enemy slots is null, if not it plays the card in said slot
    /// </summary>
    /// <param name="turn"></param>
    public void EnemyTurn(int turn)
    {
        // TODO: confuse mechanic.
        // (Scraped for now).

        int deckCount = enemyDeck.hand.Count;

        for (int i = 0; i < deckCount; i++)
        {
            if (enemyDeck.hand.Count <= 0) break;

            if (enemyDeck.hand[0] != null)
            {
                if (gameObject.TryGetComponent<PartsManager>(out PartsManager enemyPartsMan))
                {
                    enemyPartsMan.PlayAnim(enemyDeck.hand[0].animationOnPlay.ToString());
                    //Debug.Log($"Enemy animation length on animation number {i}: {enemyPartsMan.allAnimators[0].GetCurrentAnimatorStateInfo(0).length} whos name is {enemyPartsMan.allAnimators[0].GetCurrentAnimatorStateInfo(0).shortNameHash}");
                    //timer = enemyPartsMan.allAnimators[0].GetCurrentAnimatorStateInfo(0).length;
                }
                cardPlayer.playCard(enemyDeck.hand[0], state.PlayerHand, enemyDeck);
            }
            // Safety net for self abandoning cards
            if (enemyDeck.hand.Count >= 1)
            {
                enemyDeck.discard.Add(enemyDeck.hand[0]);
                enemyDeck.hand.RemoveAt(0);
            }

            // TODO: Trap card exception for abandon
            // (Scraped for now).
        }

        // If final turn of enemy deck, "shuffle" the discard back into the deck
        if (enemyDeck.deck.Count <= 0)
        {
            enemyDeck.enemyshuffleDiscardIntoDeck();
        }

        // Resets the turn counter so the enemy hand loops
        if (enemyAIStruct != null)
        {
            //StateMachine state = gameObject.GetComponent<StateMachine>();
            if (state.turn >= enemyAIStruct.Count - 1)
            {
                // -1 instead of 0 because it's incrimented at the end of the turn after this call in the state machine
                state.turn = -1;
            }
        }
        else
        {
            Debug.Log("Enemy AI struct is null! This shouldn't happen ever. (EnemyAI.cs line 97).");
        }
    }
}