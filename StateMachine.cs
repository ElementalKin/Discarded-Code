using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Used to determind the current state of battle.
/// </summary>
public enum BattleState {START, PTURN, ETURN, WON, LOST }
public enum State {EnemySelection, Combat, Reward, Event}
public class StateMachine : MonoBehaviour
{
    //TODO: Clean up the code of StateMachine and remove all unused voids.

    public EnemyAI[] ai;
    public Deck PlayerHand;
    public EnemySpawnProto enemySpawnScript;
    public SceneManagment sm;

    [Tooltip("Current turn of the combat.")]
    /// <summary>
    /// The current turn.
    /// </summary>
    public int turn;
    /// <summary>
    /// The number of battles that have been faught.
    /// </summary>
    public int numberOfBattle;

    // Tutorial flags and Game Objects.
    private bool firstTurnTutorial;
    public GameObject attackBatteryTutorial;
    private bool secondTurnTutorial;
    public GameObject blockIntentTutorial;
    private bool thirdTurnTutorial;
    public GameObject supportTutorial;
    private bool firstDeckShuffleTutorial;
    public GameObject DeckShuffleTutorial;
    private bool firstSilhouetteTutorial;
    public GameObject silhouetteTutorial;
    private bool firstStickerTutorial;
    public GameObject stickerTutorial;
    private bool partPickingTutorial;
    public GameObject partPickingTutorialPrefab;

    // UI for Hand, Win/Lose Screens, and Reward Screen.
    public CardHandUI cardHandUI;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject rewardScreen;
    private PartSelectionUI partSelection;

    // UI for Events.
    /// <summary>
    /// The first event to appear.
    /// </summary>
    public GameObject EventOne;
    /// <summary>
    /// The second event to appear.
    /// </summary>
    public GameObject EventTwo;
    /// <summary>
    /// The third event to appear.
    /// </summary>
    public GameObject EventThree;
    /// <summary>
    /// The fourth event to appear.
    /// </summary>
    public GameObject EventFour;
    /// <summary>
    /// The camera Animator.
    /// </summary>
    public Animator cameraAnim;

    public bool useNewCameraDEBUG = false;
    public GameObject ShortCameraIntroTimeline;
    public GameObject LongCameraIntroTimeline;
    public GameObject BossCameraEndingTimeline;

    /// <summary>
    /// The current state of combat.
    /// </summary>
    public BattleState battleState;
    public State state;

    [HideInInspector]
    public CardFunctionality cardFunc;

    // Used for the player, determines how many moves they can make in 1 turn.
    public int maxMoves = 3;
    [HideInInspector]
    public int moves;

    private bool CheckEventsAfterReward;

    private bool bossCamDone = false;
    /// <summary>
    /// Disables all the bools for the Tutorials.
    /// </summary>
    public void DisableTurorialBools()
    {
        firstTurnTutorial = false;
        secondTurnTutorial = false;
        thirdTurnTutorial = false;
        firstDeckShuffleTutorial = false;
        firstSilhouetteTutorial = false;
        firstStickerTutorial = false;
        partPickingTutorial = false;
    }
    /// <summary>
    /// Heals the player
    /// </summary>
    /// <param name="healthToHeal">The amount of health to heal the player by.</param>
    public void HealPlayer(int healthToHeal)
    {
        // Add healthToHeal to the players health.
        PlayerHand.health += healthToHeal;
        //Cap the players health at playerHand.maxHealth.
        if (PlayerHand.health > PlayerHand.maxHealth) PlayerHand.health = PlayerHand.maxHealth;
    }

    void Start()
    {
        // If cardHandUI isn't active, set it to active.
        if (!cardHandUI.gameObject.activeInHierarchy)
        {
            cardHandUI.gameObject.SetActive(true);
        }

        // Find and save needed components .
        PlayerHand = gameObject.GetComponent<Deck>();
        cardFunc = gameObject.GetComponent<CardFunctionality>();
        partSelection = FindObjectOfType<PartSelectionUI>();

        // Set tutorial bools to true if PlayePrefs Turorial isn't set or is set to 1 (True).
        if (!PlayerPrefs.HasKey("Tutorial") || (PlayerPrefs.GetInt("Tutorial") == 1))
        {
            firstTurnTutorial = true;
            secondTurnTutorial = true;
            thirdTurnTutorial = true;
            firstDeckShuffleTutorial = true;
            firstSilhouetteTutorial = true;
            firstStickerTutorial = true;
            partPickingTutorial = true;
        }

        if (useNewCameraDEBUG)
        {
            if (ShortCameraIntroTimeline.activeInHierarchy) ShortCameraIntroTimeline.SetActive(false);
            if (LongCameraIntroTimeline.activeInHierarchy) LongCameraIntroTimeline.SetActive(false);
        }
    }

    public void Update()
    {
        // If cardHandUI ins't null.
        if (cardHandUI != null)
        {
            //if CardHandUI is open.
            if (cardHandUI.UIIsUp)
            {
                if (firstTurnTutorial == true && turn == 0)
                {
                    if (attackBatteryTutorial != null)
                    {
                        attackBatteryTutorial.SetActive(true);
                    }
                    firstTurnTutorial = false;
                }
                else if (thirdTurnTutorial == true && secondTurnTutorial == false && turn == 0)
                {
                    if (attackBatteryTutorial != null)
                    {
                        supportTutorial.SetActive(true);
                    }
                    thirdTurnTutorial = false;
                }
                if (secondTurnTutorial == true && turn == 1)
                {
                    if (attackBatteryTutorial != null)
                    {
                        blockIntentTutorial.SetActive(true);
                    }
                    secondTurnTutorial = false;
                }
            }
        }

        if (CheckEventsAfterReward == true && partSelection.partUIIsOpen == false)
        {
            switch (SceneManagment.numberOfBattles)
            {
                case 2:
                    if (EventOne != null)
                    {
                        CheckStickerTutorial();
                        EventOne.SetActive(true);
                        state = State.Event;
                    }
                    break;
                case 4:
                    if (EventTwo != null)
                    {
                        EventTwo.SetActive(true);
                        state = State.Event;
                    }
                    break;
                case 6:
                    if (EventThree != null)
                    {
                        EventThree.SetActive(true);
                        state = State.Event;
                    }
                    break;
                case 8:
                    if (EventFour != null)
                    {
                        EventFour.SetActive(true);
                        state = State.Event;
                    }
                    break;
                default:
                    break;

            }
            CheckEventsAfterReward = false;
        }

        if (ai.Length > 0)
        {
            if (ai[0] != null)
            {
                if (ai[0].enemyDeck.health <= 0)
                {

                    if (SceneManagment.numberOfBattles > 8) // Win at the final boss.
                    {
                        //winScreen.SetActive(true);
                        //AudioManager.instance.DeckSounds(9);

                        if (!bossCamDone)
                        {
                            if (BossCameraEndingTimeline != null)
                            {
                                BossCameraEndingTimeline.SetActive(true);
                            }

                            if (ai[0].gameObject.TryGetComponent(out PartsManager enemyMan))
                            {
                                enemyMan.PlayAnim("Boss_Death");
                            }

                            cardHandUI.ToggleBattleUI(false);
                            moves = 0;

                            bossCamDone = true;
                        }
                    }
                    else if (battleState != BattleState.WON) // Win in any other battle.
                    {
                        battleState = BattleState.WON;
                        switch (SceneManagment.currentSceneLoaded)
                        {
                            case SceneManagment.SceneLoaded.basement:
                                AudioManager.instance.DeckSounds(5);
                                break;
                            case SceneManagment.SceneLoaded.bedroom:
                                AudioManager.instance.DeckSounds(6);
                                break;
                            case SceneManagment.SceneLoaded.attic:
                                AudioManager.instance.DeckSounds(7);
                                break;
                            default:
                                break;
                        }
                        //cameraAnim.SetTrigger("ResetPos");
                        //if (SceneManagment.numberOfBattles != 3 && SceneManagment.numberOfBattles != 7 && SceneManagment.numberOfBattles != 8)
                        //{
                        //    // Enemy Death Sound
                        //    //AudioManager.instance.DeckSounds(9);
                        //}

                        PlayerHand.DiscardHand();
                        PlayerHand.hand.Clear();
                        PlayerHand.abandon.Clear();
                        PlayerHand.discard.Clear();

                        ResetStatus(PlayerHand);
                        //ResetStatus(ai[0].enemyDeck);

                        if (PlayerHand.gameObject.TryGetComponent(out PartsManager partsMan))
                        {
                            partsMan.PlayAnim("Other_VictoryPlayer");
                        }

                        if (ai[0].gameObject.TryGetComponent(out PartsManager enemyMan))
                        {
                            enemyMan.PlayAnim("Other_DeathBreak");
                        }

                        cardHandUI.ToggleBattleUI(false);
                        moves = 0;

                        sm.numberOfBattleUp();
                        state = State.EnemySelection;
                    }
                }
            }
            // If the player health is less or equal to zero.
            if (PlayerHand.health <= 0)
            {
                // Player Death Sound
                AudioManager.instance.DeckSounds(8);
                // Set the state to LOST.
                battleState = BattleState.LOST;
                if (battleState == BattleState.LOST)
                {
                    // Disable the BattleUI.
                    cardHandUI.ToggleBattleUI(false);
                    //  Activate the lose screen.
                    loseScreen.SetActive(true);
                    if (PlayerHand.gameObject.TryGetComponent(out PartsManager playerMan))
                    {
                        playerMan.PlayAnim("Other_DeathBreak");
                    }
                }
            }
        }
    }
    /// <summary>
    /// Opens the reward screen.
    /// </summary>
    public void OpenRewardsScreen()
    {
        // Activate the rewardScreen.
        rewardScreen.SetActive(true);
        // Calls a void to populate the UI with the parts of the enemy.
        partSelection.OpenAndPopulateUI(ai[0]);
        state = State.Reward;
        //
        CheckEventsAfterReward = true;
        // Check if the turorial isn't null.
        if (partPickingTutorialPrefab != null)
        {
            // Check if partPickingTutorial bool is true.
            if (partPickingTutorial == true)
            {
                //Activate the partPickingTurorial.
                partPickingTutorialPrefab.SetActive(true);
                // Set partPickingTutorial to false so it doen't trigger again.
                partPickingTutorial = false;
            }
        }
    }

    public void SwitchToEnemySelectCam()
    {
        if (useNewCameraDEBUG)
        {
            if (ShortCameraIntroTimeline.activeInHierarchy) ShortCameraIntroTimeline.SetActive(false);
            if (LongCameraIntroTimeline.activeInHierarchy) LongCameraIntroTimeline.SetActive(false);
        }
    }
    /// <summary>
    /// Start a new round of combat.
    /// </summary>
    public void beginNewCombat()
    {
        state = State.Combat;
        PlayerHand.beginningOfCombat = true;
        // Find all EnemyAI spawned by ClickedOn.Clicked.
        ai = FindObjectsOfType<EnemyAI>();

        // Rebuild the player deck.
        PlayerHand.BuildDeck();

        // Build the enemy deck and set accompanying bool.
        ai[0].enemyDeck.buildEnemyDeck();
        ai[0].enemyDeck.isEnemyDeck = true;

        // Begin the transition animation, and disable the reward screen if open.
        //cameraAnim.SetTrigger("StartCamSwing");
        rewardScreen.SetActive(false);
        //cardHandUI.gameObject.SetActive(false);

        // Reset turn.
        turn = 0;

        // Officially begin the combat.
        PlayerTurn();

        if (useNewCameraDEBUG)
        {
            // Change Active camera
            if (SceneManagment.numberOfBattles == 0 || SceneManagment.numberOfBattles == 4)
            {
                LongCameraIntroTimeline.SetActive(true);
            }
            else
            {
                ShortCameraIntroTimeline.SetActive(true);
            }
        }
        PlayerHand.beginningOfCombat = false;
    }

    /// <summary>
    /// Turns state to PTURN, resets players block, decrease or reset the stacks of certain status effects, call StartDraw for the player and enemy, reset player moves to their max, updates the players hand display, 
    /// updates the healt and block UI for both player and enemys.
    /// </summary>
    void PlayerTurn()
    {
        // Set turn to 0, state to PTURN.
        battleState = BattleState.PTURN;

        // Reset PLAYER block.
        PlayerHand.block = 0;

        // Resolve any status effects that resolve at this point.
        CheckStatusEffect(PlayerHand);

        // Start the draw for both the player and enemy
        PlayerHand.StartDraw();
        ai[0].enemyDeck.StartDraw();

        // Set the player's moves to their max, then display the player's hand.
        moves = maxMoves;
        cardHandUI.DisplayHand(PlayerHand.hand);

        // Update the player's and enemy's health bar.
        cardHandUI.UpdatePlayerStats(PlayerHand.health, PlayerHand.block);
        if (ai.Length > 0) cardHandUI.UpdateEnemyStats(ai[0].enemyDeck.health, ai[0].enemyDeck.block);
    }

    /// <summary>
    /// Decrease or reset the stacks of certain status effects, set state to ETURN, reset enemys block, run EnemyTurn void.
    /// </summary>
    public void EnemyTurn()
    {
        // Resolve any status effects that resolve at that point for the enemy.
        CheckStatusEffect(ai[0].enemyDeck);

        // Change the turn to the enemy turn.
        battleState = BattleState.ETURN;

        // For 1 time...
        for (int i = 0; 1 > i; i++)
        {
            // Reset block
            ai[i].enemyDeck.block = 0;

            // Enemy plays cards and accompanying animations for those cards here.
            ai[i].EnemyTurn(turn);

            // Resolve any status effects that resolve at that point for the player.
            CheckStatusEffect(PlayerHand);

            // TODO: Move this to be in the ENEMYAI, since this will be per-card.
            cardHandUI.UpdatePlayerStats(PlayerHand.health, PlayerHand.block);
            if (ai.Length > 0) cardHandUI.UpdateEnemyStats(ai[0].enemyDeck.health, ai[0].enemyDeck.block);
        }

        // If we haven't yet won OR lost,
        if (battleState != BattleState.WON && battleState != BattleState.LOST)
        {
            turn++;
            PlayerHand.DiscardHand();
            PlayerTurn();
        }
    }
    /// <summary>
    /// Resets all the Target's status effects, and block.
    /// </summary>
    /// <param name="Target"> the Deck to have status effects reset.</param>
    public void ResetStatus(Deck Target)
    {
        for (int i = 0; i < Target.statusList.Count; i++)
        {
            Target.statusList[i] = 0;
        }

        Target.block = 0;
    }

    /// <summary>
    /// Check the current Status effect and what they should be doing.
    /// </summary>
    public void CheckStatusEffect(Deck Target)
    {
        //check the status of the effect and wether it should be applied.
        for (int i = 0; i < Target.statusList.Count; i++)
        {
            if (Target.statusList[i] > 0)
            {
                if (Target.statusList[i] > 0)
                {
                    switch (i)
                    {
                        case 0:
                            break;
                        //case 1:
                        //    Target.statusList[1] = 0;
                        //    break;
                        //case 2:
                        //    Target.statusList[2] = 0;
                        //    break;
                        case 3:
                            //Removes a stack of squeeze
                            Target.statusList[3] -= 1;
                            //If fragile - reinforce is lower than 3 (base damage of squeeze) do damage.
                            if (Target.statusList[5] - Target.statusList[4] < 3)
                            {
                                //Damage = 3 + (fragile - reinforce)
                                Target.health -= 3 + (Target.statusList[4] - Target.statusList[5]);
                            }
                            if (Target.statusList[6] > 0)
                            {
                                //Check if target is equal to enemys deck to figure out wether target it the player or enemy.
                                if (Target == ai[0].enemyDeck)
                                {
                                    //If fragile - reinforce is lower than the number of renvenge stacks
                                    if (PlayerHand.statusList[5] - PlayerHand.statusList[4] < Target.statusList[6])
                                    {
                                        //Damage = number of revenge stacks + (fragile - reinforce)
                                        Target.block -= PlayerHand.statusList[6] + (Target.statusList[4] - Target.statusList[5]);
                                        if (Target.block < 0)
                                        {
                                            Target.health += Target.block;
                                            Target.block = 0;
                                        }
                                    }

                                }
                                //Checking wether it is actually the player.
                                else if (Target == PlayerHand)
                                {
                                    //If fragile - reinforce is lower than the number of renvenge stackss
                                    if (ai[0].enemyDeck.statusList[5] - ai[0].enemyDeck.statusList[4] < Target.statusList[6])
                                    {
                                        //Damage = number of revenge stacks + (fragile - reinforce)
                                        ai[0].enemyDeck.block -= Target.statusList[6] + (ai[0].enemyDeck.statusList[4] - ai[0].enemyDeck.statusList[5]);
                                        if (ai[0].enemyDeck.block < 0)
                                        {
                                            ai[0].enemyDeck.health += ai[0].enemyDeck.block;
                                            ai[0].enemyDeck.block = 0;
                                        }
                                    }
                                }
                            }
                            break;
                        case 4:
                            Target.statusList[4] = 0;
                            break;
                        case 5:
                            Target.statusList[5] = 0;
                            break;
                        case 6:
                            Target.statusList[6] = 0;
                            break;
                        case 7:
                            Target.statusList[7] = 0;
                            break;
                        case 8:
                            if (battleState == BattleState.PTURN)
                            {
                                Target.statusList[8] -= 1;
                                PlayerHand.health -= 2;
                            }
                            break;
                        case 9:
                            if (battleState == BattleState.PTURN)
                            {
                                Target.statusList[9] -= 1;
                                HealPlayer(1);
                            }
                            break;
                        case 10:
                            if (battleState == BattleState.PTURN) 
                            {
                                Target.statusList[10] -= 1;
                                if (Target == PlayerHand)
                                {
                                    Target.startDrawAmount += 1;
                                }
                                else if (Target == ai[0].enemyDeck)
                                {
                                    // Currently enemy doesn't have a deck to choose from, leave empty untill they do.
                                }
                               
                            }
                            break;
                    }
                }
            }
        }
    }

    public void CheckStickerTutorial()
    {
        if (firstStickerTutorial == true)
        {
            stickerTutorial.SetActive(true);
            firstStickerTutorial = false;
        }
    }

    // TODO, perhaps plug this in Deck.cs's NaturalShuffle?
    public void CheckShuffleTutorial()
    {
        if (firstDeckShuffleTutorial == true && DeckShuffleTutorial != null)
        {
            DeckShuffleTutorial.SetActive(true);
            firstDeckShuffleTutorial = false;
        }
    }

    // TODO, perhaps plug this in on Start?
    public void CheckSilhouetteTutorial()
    {
        if (firstSilhouetteTutorial == true && silhouetteTutorial != null)
        {
            silhouetteTutorial.SetActive(true);
            firstSilhouetteTutorial = false;
        }
    }
}