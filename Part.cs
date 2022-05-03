using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public PartsManager myManager;

    public Sticker sticker;
    public Sprite imageInUI;
    public bool dontShowAsTapedPart = false;
    public List<Card> partCards;

    public SoundControls[] sc;

    //[HideInInspector]
    //public Card cardEffectsToPlay;

    private void Start()
    {
        myManager = gameObject.GetComponentInParent<PartsManager>();
    }

    public void PassCards()
    {
        if (myManager != null)
        {
            foreach (Card card in partCards)
            {
                myManager.partCards.Add(card);
            }
        }
    }

    private void OnDestroy()
    {
        if (myManager != null)
        {
            myManager.RefreshParts();
        }
    }

    public void Start_AnimEvent()
    {

    }

    public void FX_AnimEvent()
    {
        //if (myManager.head == this.gameObject)
        //{
        //    if (myManager.CompareTag("Player"))
        //    {
        //        foreach (CardEffect effect in cardEffectsToPlay.cardEffects)
        //        {
        //            if (effect.effectType == CardHelpers.EffectType.Support)
        //            {

        //            }
        //        }
        //    }
        //    else // Enemy
        //    {

        //    }
        //}

        //AudioManager.instance.VFXSounds(0); // Potential SFX
        //myManager.stateMachine.cardFunc.vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, true, 1); // Potential for Player
        //myManager.stateMachine.cardFunc.vfxSpawner.VFXSpawn(CardHelpers.EffectType.Support, false, 2); // Dull for Player (dull is the only one to work like this)
    }

    public void End_AnimEvent()
    {
        //Debug.Log("hello");
    }

    public void Victory_AnimEvent()
    {
        if (SceneManagment.numberOfBattles <= 8)
        {
            // If our parent is the Player, and we are the head part...
            if (myManager.gameObject.CompareTag("Player") && myManager.head == this.gameObject)
            {
                myManager.stateMachine.OpenRewardsScreen();
            }
        }
    }
}
