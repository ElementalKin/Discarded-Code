using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFunctionality : MonoBehaviour
{
    /// <summary>
    /// Reference to the stateMachine script.
    /// </summary>
    private StateMachine stateMachine;
    /// <summary>
    /// Reference to the vfxSpawner script.
    /// </summary>
    public vfxSpawner vfxSpawner;
    /// <summary>
    /// Reference to the uivfx script. 
    /// </summary>
    public UIVFX uivfx;
    /// <summary>
    /// Reference to the players PartsManager 
    /// </summary>
    public PartsManager playerPartManager;
    /// <summary>
    /// Used to determind if the sticker effect should be applied.
    /// </summary>
    bool ApplyStickerStatusEffects;

    [SerializeField] AudioCollection audCollection;
    private void Start()
    {
        // Grab the stateMachine and store it for later.
        stateMachine = gameObject.GetComponent<StateMachine>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cardToPlay">The card that will be used in the calculation.</param>
    /// <param name="target">The target that gets affected by the card.</param>
    /// <param name="attacker">The person who is playing the card.</param>
    public void playCard(Card cardToPlay, Deck target, Deck attacker)
    {
        // Set Bogo to false
        bool Bogo = false;
        // Check if the attacker has BOGO.
        if(attacker.statusList[7] > 0)
        {
            // Remove one stack of BOGO and save it.
            int x = attacker.statusList[7] - 1;
            // Set attackers BOGO to 0
            attacker.statusList[7] = 0;
            // Play the card again.
            playCard(cardToPlay, target, attacker);
            // Set the attackers BOGO to the saved variable.
            attacker.statusList[7] = x;
            // Make Bogo true.
            Bogo = true;
        }
        /// <summary>
        /// Bool that is used to check if potential needs to be cleared at the end of the round.
        /// </summary>
        bool ClearPotential = false;
        /// <summary>
        /// The amount of Potential that is saved for next round.
        /// </summary>
        int SavedPotential = 0;
        /// <summary>
        /// The amount of Dull that is saved for next round.
        /// </summary>
        int SavedDull = 0;
        // ApplyStickerStatusEffects bool is false by defualt.
        ApplyStickerStatusEffects = false;
        /// <summary>
        /// I think this is out of date. - Edward
        /// </summary>
        int stickerAttackBonuse = 0;
        /// <summary>
        /// I think this is out of date. - Edward
        /// </summary>
        int stickerBlockBonuse = 0;
        // If the attacker is the player.
        if (attacker == stateMachine.PlayerHand)
        {
            // Plays card sounds for player
            AudioManager.instance.CardSounds(cardToPlay);
            // FindObjectOfType<AudioManager>().PlayCardSound(cardToPlay);
            // Switch case for which body part is the card attached to.
            switch (cardToPlay.bodyPart)
            {
                case CardHelpers.BodyPart.Head:
                    //checking if the sticker is not null.
                    if (!(playerPartManager.allPartsComponents[0].sticker == null))
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                        // Play the sticker effect.
                        stickerCaseStaments(0, target, attacker, stickerBlockBonuse, stickerAttackBonuse, Bogo, cardToPlay.cardCost);
                    }
                    else
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                    }
                    break;
                case CardHelpers.BodyPart.Torso:
                    if (!(playerPartManager.allPartsComponents[1].sticker == null))
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                        stickerCaseStaments(1, target, attacker, stickerBlockBonuse, stickerAttackBonuse, Bogo, cardToPlay.cardCost);
                    }
                    else
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                    }
                    break;
                case CardHelpers.BodyPart.ArmL:
                    if (!(playerPartManager.allPartsComponents[2].sticker == null))
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                        stickerCaseStaments(2, target, attacker, stickerBlockBonuse, stickerAttackBonuse, Bogo, cardToPlay.cardCost);
                    }
                    else
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                    }
                    break;
                case CardHelpers.BodyPart.ArmR:
                    if (!(playerPartManager.allPartsComponents[3].sticker == null))
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                        stickerCaseStaments(3, target, attacker, stickerBlockBonuse, stickerAttackBonuse, Bogo, cardToPlay.cardCost);
                    }
                    else
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                    }
                    break;
                case CardHelpers.BodyPart.Legs:
                    if (!(playerPartManager.allPartsComponents[4].sticker == null))
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                        stickerCaseStaments(4, target, attacker, stickerBlockBonuse, stickerAttackBonuse, Bogo, cardToPlay.cardCost);
                    }
                    else
                    {
                        // Remove cardCost from the players Battery.
                        stateMachine.moves -= cardToPlay.cardCost;
                    }
                    break;
                default:
                    break;
            }
        }
        // Loops through the card effects and applies them according to their type
        for (int x = 0; cardToPlay.cardEffects.Count > x; x++)
        {
            // START DAMAGE CALCULATION ------------------------------------------------------------------------------------
            // Store the effect in a variable for easy access.
            CardEffect effect = cardToPlay.cardEffects[x];

            // Revenge calaculation.
            if (effect.effectType == CardHelpers.EffectType.Attack && effect.applyToSelf && attacker.statusList[6] > 0)
            {
                int calculatedDamage = attacker.statusList[6];
                calculatedDamage += attacker.statusList[1];
                calculatedDamage -= attacker.statusList[2];
                calculatedDamage += attacker.statusList[4];
                calculatedDamage -= attacker.statusList[5];
                if (calculatedDamage >= 0)
                {
                    //If fragile - reinforce is lower than the number of renvenge stacks
                    if (target.statusList[5] - target.statusList[4] < attacker.statusList[6])
                    {
                        //Damage = number of revenge stacks + (fragile - reinforce)
                        target.block -= attacker.statusList[6] + (target.statusList[4] - target.statusList[5]);
                        if (target.block < 0)
                        {
                            target.health += target.block;
                            if (target == stateMachine.PlayerHand)
                            {
                                uivfx.SpawnDamageText(-target.block, true);
                            }
                            else if(target == stateMachine.ai[0])
                            {
                                uivfx.SpawnDamageText(-target.block, false);
                            }
                            target.block = 0;
                        }

                    }
                }

            }
            else if (effect.effectType == CardHelpers.EffectType.Attack && !effect.applyToSelf && target.statusList[6] > 0)
            {
                int calculatedDamage = target.statusList[6];
                calculatedDamage += attacker.statusList[1];
                calculatedDamage -= attacker.statusList[2];
                calculatedDamage += target.statusList[4];
                calculatedDamage -= target.statusList[5];
                if (calculatedDamage >= 0)
                {
                    if (effect.applyMultipleTimes)
                    {
                        for (int i = 0; i < effect.effectNumberTimes; i++)
                        {
                            //If fragile - reinforce is lower than the number of renvenge stacks
                            if (attacker.statusList[5] - attacker.statusList[4] < target.statusList[6])
                            {
                                //Damage = number of revenge stacks + (fragile - reinforce)
                                attacker.block -= target.statusList[6] + (attacker.statusList[4] - attacker.statusList[5]);
                                if (attacker.block < 0)
                                {
                                    attacker.health += attacker.block;
                                    if (attacker == stateMachine.PlayerHand)
                                    {
                                        uivfx.SpawnDamageText(-attacker.block, true);
                                    }
                                    else if (attacker == stateMachine.ai[0].enemyDeck)
                                    {
                                        uivfx.SpawnDamageText(-attacker.block, false);
                                    }
                                    attacker.block = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        //If fragile - reinforce is lower than the number of renvenge stacks
                        if (attacker.statusList[5] - attacker.statusList[4] < target.statusList[6])
                        {
                            //Damage = number of revenge stacks + (fragile - reinforce)
                            attacker.block -= target.statusList[6] + (attacker.statusList[4] - attacker.statusList[5]);
                            if (attacker.block < 0)
                            {
                                attacker.health += attacker.block;
                                if (attacker == stateMachine.PlayerHand)
                                {
                                    uivfx.SpawnDamageText(-attacker.block, true);
                                }
                                else if (attacker == stateMachine.ai[0].enemyDeck)
                                {
                                    uivfx.SpawnDamageText(-attacker.block, false);
                                }
                                attacker.block = 0;
                            }
                        }
                    }
                }
            }
            // If we're applying this to ourselves, we are the target.
            if (effect.applyToSelf) target = attacker;

            // Calculate the atack number. Start with the appliedStacks number as a fallback.
            int calculatedStacks = effect.appliedStacks;

            // If we're actually referencing a variable, grab it here.
            if (effect.referenceVariable)
            {
                switch (effect.variable)
                {
                    case CardHelpers.ReferenceVariable.Attack:
                        // no idea that i was thinking when i put this tbh!
                        // real square brain moment right there.
                        break;
                    case CardHelpers.ReferenceVariable.Block:
                        calculatedStacks = attacker.block;
                        break;
                    case CardHelpers.ReferenceVariable.Health:
                        calculatedStacks = attacker.health;
                        break;
                    case CardHelpers.ReferenceVariable.AttackCardsInHand:
                        calculatedStacks = attacker.handCounterList[0];
                        break;
                    case CardHelpers.ReferenceVariable.BlockCardsInHand:
                        calculatedStacks = attacker.handCounterList[1];
                        break;
                    case CardHelpers.ReferenceVariable.SupportCardsInHand:
                        calculatedStacks = attacker.handCounterList[2];
                        break;
                    case CardHelpers.ReferenceVariable.CardsInDeck:
                        calculatedStacks = attacker.deck.Count;
                        break;
                    case CardHelpers.ReferenceVariable.CardsInDiscard:
                        calculatedStacks = attacker.discard.Count;
                        break;
                    default:
                        // Purposefully blank. This condition should never trigger.
                        Debug.LogWarning("If you see this text, something has gone very very wrong.\nCardFunctionality.cs switch on effect.variable, default was hit.");
                        break;
                }
            }
            //TODO: Make this into a void. -Edward
            if ((effect.effectType == CardHelpers.EffectType.Attack || effect.effectType == CardHelpers.EffectType.Block) && !cardToPlay.isTrapCard)
            {
                // Apply buffs to calculatedStacks
                // Potential
                calculatedStacks += attacker.statusList[1];

                // Dull
                calculatedStacks -= attacker.statusList[2];
            }

            //check whether it is attack card an apply fragile and reinforce if so.
            if (effect.effectType == CardHelpers.EffectType.Attack) {
                // Fragile
                calculatedStacks += target.statusList[4];

                // Reinforced
                calculatedStacks -= target.statusList[5];
            }
            // And finally, if we're executing this multiple times, multiply it here.
            if (effect.applyMultipleTimes) calculatedStacks *= effect.effectNumberTimes;

            calculatedStacks = Mathf.Max(calculatedStacks, 0);

            // END DAMAGE CALCULATION ------------------------------------------------------------------------------------

            // TODO: go through all the calcualtions and make sure it is all correct. - Edward
            // The Big Switch
            // Checks the effect type, and determines how exactly to apply it.
            switch (effect.effectType)
            {
                // Apply the damage to the intended target, damage is affected by dull, potential of the attacker and the fragile, and reinfoced of the target.
                case CardHelpers.EffectType.Attack:
                    ClearPotential = true;
                    // If block is greater than or equal to the amount of damage just do damage to block.
                    if (target.block >= calculatedStacks)
                    {
                        target.block -= calculatedStacks;
                    }
                    // Else if the targets block is less than the amount of damage and block is greater than 0.
                    else if (target.block < calculatedStacks && target.block > 0)
                    {
                        // Remove Block from the calculated damage.
                        int remainingAfterBlock = calculatedStacks - target.block;
                        // Set the block to zero.
                        target.block = 0;
                        // Remove the remaining damage from the players health.
                        target.health -= remainingAfterBlock;
                        // If the target was the player then spawn the damage text on the players health bar.
                        if (target == stateMachine.PlayerHand)
                        {
                            uivfx.SpawnDamageText(remainingAfterBlock, true);
                        }
                        // Else if the target was the enemy then spawn the damage text on the enemys health bar.
                        else if (target == stateMachine.ai[0].enemyDeck)
                        {
                            uivfx.SpawnDamageText(remainingAfterBlock, false);
                        }
                    }
                    // I don't remeber why this is here but it should catch anything that gets through.
                    else  
                    {
                        target.health -= calculatedStacks - target.block;
                        // If the target was the player then spawn the damage text on the players health bar.
                        if (target == stateMachine.PlayerHand)
                        {
                            uivfx.SpawnDamageText(calculatedStacks, true);
                        }
                        // Else if the target was the enemy then spawn the damage text on the enemys health bar.
                        else if (target == stateMachine.ai[0].enemyDeck)
                        {
                            uivfx.SpawnDamageText(calculatedStacks, false);
                        }
                    }
                    // Spawns the attack vfx for the player.
                    if (attacker == stateMachine.PlayerHand)
                    {
                        vfxSpawner.VFXSpawn(CardHelpers.EffectType.Attack, false, 0);


                    }
                    // Spawns the attack vfx for the enemy.
                    else
                    {
                        vfxSpawner.VFXSpawn(CardHelpers.EffectType.Attack, true, 0);
                    }
                    // Creates a sound affect depending on what torso the player has.
                    AudioManager.instance.TorsoSounds(playerPartManager.allPartsComponents[1].sc);
                    break;
                //Increase Block according to the number of stacks that the card has, is affected by Dull and Potential.
                case CardHelpers.EffectType.Block:
                    // Set ClearPotential to true.
                    ClearPotential = true;
                    // Adds the calculatedStacks to the targets block.
                    target.block += calculatedStacks;
                    // If the target is the player than spawn the defense vfx.
                    if (attacker == stateMachine.PlayerHand)
                    {
                        vfxSpawner.VFXSpawn(CardHelpers.EffectType.Block, true, 0);

                    }
                    // else spawn block vfx on the enemy.
                    else
                    {
                        vfxSpawner.VFXSpawn(CardHelpers.EffectType.Block, false, 0);
                    }
                    break;
                case CardHelpers.EffectType.Heal:
                    // Apply calculatedStacks directly to health.
                    // HEAL SHOULD ONLY BE POSSITVE. Should heal ever end up negitive,
                    // it would effectively act as damage that bypasses block. Which sounds
                    // kind of cool but deffinitely not an intended effect. Haha... unless?...
                    target.health += calculatedStacks;
                    break;
                    //Increase Battery according to the number of stacks that the card has, is not affected by anything.
                case CardHelpers.EffectType.Battery:
                    stateMachine.moves += calculatedStacks;
                    break;

                case CardHelpers.EffectType.Support:
                    // Add stacks to the index in the deck's statusList.
                    // Refer to the summary in Deck.cs for the list of which
                    // index corresponds to each status effect.
                    switch (effect.statusToEdit)
                    {
                        case CardHelpers.StatusEffect.Potential:
                            target.statusList[1] += calculatedStacks;
                            if (attacker == stateMachine.PlayerHand)
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, true, 1);
                            }
                            else
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, false, 1);
                            }
                            if (ClearPotential == true && target == stateMachine.PlayerHand)
                            {
                                SavedPotential = calculatedStacks;
                            }
                            break;
                        case CardHelpers.StatusEffect.Dull:
                            target.statusList[2] += calculatedStacks;
                            if (attacker == stateMachine.PlayerHand)
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, false, 2);
                            }
                            else
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, true, 2);
                            }
                            if (ClearPotential == true && target == stateMachine.PlayerHand)
                            {
                                SavedDull = calculatedStacks;
                            }
                            break;
                        case CardHelpers.StatusEffect.Squeeze:
                            target.statusList[3] += calculatedStacks;
                            if (attacker == stateMachine.PlayerHand)
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, true, 3);
                            }
                            else
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, false, 3);
                            }
                            break;
                        case CardHelpers.StatusEffect.Fragile:
                            target.statusList[4] += calculatedStacks;
                            //Debug.Log("Playing fragile vfx");
                            if (attacker == stateMachine.PlayerHand)
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, true, 4);
                            }
                            else
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, false, 4);
                            }
                            break;
                        case CardHelpers.StatusEffect.Reinforce:
                            target.statusList[5] += calculatedStacks;
                            if (attacker == stateMachine.PlayerHand)
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, true, 5);
                            }
                            else
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, false, 5);
                            }
                            break;
                        case CardHelpers.StatusEffect.Revenge:
                            target.statusList[6] += calculatedStacks;
                            if (attacker == stateMachine.PlayerHand)
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, true, 6);
                            }
                            else
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, false, 6);
                            }
                            break;
                        case CardHelpers.StatusEffect.BOGO:
                            target.statusList[7] += calculatedStacks;
                            if (attacker == stateMachine.PlayerHand)
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, true, 7);
                            }
                            else
                            {
                                vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, false, 7);
                            }
                            break;
                        default:
                            // Purposefully blank. This condition should never trigger.
                            Debug.LogWarning("If you see this text, something has gone very very wrong.\nCardFunctionality.cs switch on effect.statusToEdit, default was hit.");
                            break;
                    }
                    break;

                case CardHelpers.EffectType.DeckEdit:
                    // calculatedStacks becomes the amount of cards to draw/discard/etc,
                    // so we loop here until we've done the opperation this many times.
                    for (int i = 0; i < calculatedStacks; i++)
                    {
                        switch (effect.DeckAction)
                        {
                            case CardHelpers.DeckModify.DrawCard:
                                target.Draw();
                                break;
                            case CardHelpers.DeckModify.DrawFromDiscard:
                                target.DrawFromDiscardRandom();
                                break;
                            case CardHelpers.DeckModify.DrawFromAbandon:
                                target.AbandonDraw();
                                break;
                            case CardHelpers.DeckModify.ShuffleDeck:
                                target.ShuffleDeck();
                                break;
                            case CardHelpers.DeckModify.ShuffleDiscardIntoDeck:
                                target.shuffleDiscardIntoDeck();
                                break;
                            case CardHelpers.DeckModify.ForceDiscardCard:
                                target.DiscardSlot(Random.Range(0, target.hand.Count));
                                break;
                            case CardHelpers.DeckModify.ForceDiscardHand:
                                target.DiscardHand();
                                break;
                            case CardHelpers.DeckModify.PlayRandomFromHand:
                                playCard(target.HandSlot(Random.Range(0, target.hand.Count)), target, attacker);
                                break;
                            case CardHelpers.DeckModify.PlayRandomFromDeck:
                                playCard(target.DeckSlot(Random.Range(0, target.deck.Count)), target, attacker);
                                break;
                            case CardHelpers.DeckModify.PlayRandomFromDiscard:
                                playCard(target.DiscardSlot(Random.Range(0, target.discard.Count)), target, attacker);
                                break;
                            case CardHelpers.DeckModify.AddSpecificCardToDeck:
                                attacker.addCardOpponentDeck(effect.cardToAdd, target);
                                break;
                            case CardHelpers.DeckModify.AbandonThisCard:
                                attacker.Abandon(cardToPlay);
                                break;
                            default:
                                // Purposefully blank. This condition should never trigger.
                                Debug.LogWarning("If you see this text, something has gone very very wrong.\nCardFunctionality.cs switch on effect.DeckAction, default was hit.");
                                break;
                        }
                    }                    
                    break;

                case CardHelpers.EffectType.None:
                default:
                    // Nothing happens (on purpose);
                    Debug.Log("Played a card with the None effect- or hit default.");
                    break;
            }

            // CODE HERE FOR REFERENCE
            //switch (effect.effectType)
            //{
            //    case CardHelpers.EffectType.Attack:
            //        switch (effect.effectExecutions)
            //        {
            //            case CardHelpers.ModifierType.Single:
            //                if (target.block > effect.appliedStacks)
            //                {
            //                    target.block -= effect.appliedStacks;
            //                }
            //                else
            //                {
            //                    target.health -= (effect.appliedStacks - target.block);
            //                    target.block -= (effect.appliedStacks);
            //                }
            //                if (target.block < 0)
            //                {
            //                    target.block = 0;
            //                }
            //                break;
        }
        //Reset potential and dull.
        if (ClearPotential)
        {
            attacker.statusList[1] = 0 + SavedPotential;
            attacker.statusList[2] = 0 + SavedDull;
        }
        if (ApplyStickerStatusEffects)
        {
            int x = (int)cardToPlay.bodyPart;
            for (int i = 0; i < playerPartManager.allPartsComponents[x].sticker.stickerEffects.Count; i++)
            {
                switch (playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].statusToEdit)
                {
                    case CardHelpers.StatusEffect.Potential:
                        attacker.statusList[(int)playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].statusToEdit + 1] += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;
                        break;
                    case CardHelpers.StatusEffect.Dull:
                        stateMachine.ai[0].enemyDeck.statusList[(int)playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].statusToEdit + 1] += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;
                        break;
                    case CardHelpers.StatusEffect.Squeeze:
                        stateMachine.ai[0].enemyDeck.statusList[(int)playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].statusToEdit + 1] += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;
                        break;
                    case CardHelpers.StatusEffect.Fragile:
                        stateMachine.ai[0].enemyDeck.statusList[(int)playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].statusToEdit + 1] += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;
                        break;
                    case CardHelpers.StatusEffect.Reinforce:
                        attacker.statusList[(int)playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].statusToEdit + 1] += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;
                        break;
                    case CardHelpers.StatusEffect.Revenge:
                        attacker.statusList[(int)playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].statusToEdit + 1] += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;
                        break;
                    case CardHelpers.StatusEffect.BOGO:
                        attacker.statusList[(int)playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].statusToEdit + 1] += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;
                        break;
                    default:
                        break;

                }
            }
        }
        // Update the stats on the UI (maybe move?)
        stateMachine.cardHandUI.UpdatePlayerStats(stateMachine.PlayerHand.health, stateMachine.PlayerHand.block);
        if (stateMachine.ai.Length > 0) stateMachine.cardHandUI.UpdateEnemyStats(stateMachine.ai[0].enemyDeck.health, stateMachine.ai[0].enemyDeck.block);
    }
    /// <summary>
    /// Used to apply the sticker affects.
    /// </summary>
    /// <param name="x">Determinds what body part the sticker is on. </param>
    /// <param name="target">Who is the target of the sticker effect.</param>
    /// <param name="attacker">Who is the sticker is being applied from.</param>
    /// <param name="blockBonus">Not used may be deleted later.</param>
    /// <param name="attackBonus">Not used may be deleted later.</param>
    /// <param name="Bogo">BOGO was killed so is no longer in use.</param>
    /// <param name="cardCost">Increases the cost of the card.</param>
    private void stickerCaseStaments(int x, Deck target, Deck attacker, int blockBonus, int attackBonus, bool Bogo ,int cardCost)
    {
        //if not null go through the stickers effects
        for (int i = 0; i < playerPartManager.allPartsComponents[x].sticker.stickerEffects.Count; i++)
        {
            int calculatedStacks = 0;
            switch (playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].stickerEffectType)
            {
                case StickerEffectType.Attack:
                    // Apply buffs to calculatedStacks
                    // Potential
                    calculatedStacks += attacker.statusList[1];
                    
                    // Dull
                    calculatedStacks -= attacker.statusList[2];
                    
                    // Fragile
                    calculatedStacks += target.statusList[4];

                    // Reinforced
                    calculatedStacks -= target.statusList[5];

                    // Grab the number of stack that the sticker has.
                    calculatedStacks += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;

                    // If block is greater than or equal to the amount of damage just do damage to block.
                    if (target.block >= calculatedStacks)
                    {
                        target.block -= calculatedStacks;
                    }
                    // Else if the targets block is less than the amount of damage and block is greater than 0.
                    else if (target.block < calculatedStacks && target.block > 0)
                    {
                        // Remove Block from the calculated damage.
                        int remainingAfterBlock = calculatedStacks - target.block;
                        // Set the block to zero.
                        target.block = 0;
                        // Remove the remaining damage from the players health.
                        target.health -= remainingAfterBlock;
                        // If the target was the player then spawn the damage text on the players health bar.
                        if (target == stateMachine.PlayerHand)
                        {
                            uivfx.SpawnDamageText(remainingAfterBlock, true);
                        }
                        // Else if the target was the enemy then spawn the damage text on the enemys health bar.
                        else if (target == stateMachine.ai[0].enemyDeck)
                        {
                            uivfx.SpawnDamageText(remainingAfterBlock, false);
                        }
                    }
                    else
                    {
                        target.health -= calculatedStacks - target.block;
                        if (target == stateMachine.PlayerHand)
                        {
                            uivfx.SpawnDamageText(calculatedStacks, true);
                        }
                        else if (target == stateMachine.ai[0].enemyDeck)
                        {
                            uivfx.SpawnDamageText(calculatedStacks, false);
                        }
                    }
                    if (attacker == stateMachine.PlayerHand)
                    {
                        vfxSpawner.VFXSpawn(CardHelpers.EffectType.Attack, false, 0);

                    }
                    else
                    {
                        vfxSpawner.VFXSpawn(CardHelpers.EffectType.Attack, true, 0);
                    }
                    AudioManager.instance.TorsoSounds(playerPartManager.allPartsComponents[1].sc);
                    break;
                case StickerEffectType.Block:

                    // Apply buffs to calculatedStacks
                    // Potential
                    calculatedStacks += attacker.statusList[1];
                    
                    // Dull
                    calculatedStacks -= attacker.statusList[2];

                    calculatedStacks += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;

                    if (calculatedStacks < 0)
                    {
                        calculatedStacks = 0;
                    }
                    attacker.block += calculatedStacks;
                    if (attacker == stateMachine.PlayerHand)
                    {
                        vfxSpawner.VFXSpawn(CardHelpers.EffectType.Block, true, 0);

                    }
                    else
                    {
                        vfxSpawner.VFXSpawn(CardHelpers.EffectType.Block, false, 0);
                    }
                    break;
                case StickerEffectType.Heal:
                    attacker.health += playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks;
                    break;
                case StickerEffectType.Battery:
                    //TODO: add when battery exists.
                    break;
                case StickerEffectType.Support:
                    ApplyStickerStatusEffects = true;
                    break;
                case StickerEffectType.CardCost:
                    if (Bogo) {
                        stateMachine.moves -= (cardCost - playerPartManager.allPartsComponents[x].sticker.stickerEffects[i].appliedStacks);
                    }
                    break;
                case StickerEffectType.None:
                    break;
                default:
                    break;
            }
        }
    }

}