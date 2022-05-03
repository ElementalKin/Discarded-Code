using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHelpers : MonoBehaviour
{
    // This class will hold Enums and other variables needed
    // in scripts elsewhere across the project to ease of access!

    public enum EffectType { Attack, Block, Heal, Battery, Support, DeckEdit, None };

    public enum BodyPart { Head, Torso, ArmL, ArmR, Legs };

    public enum StatusEffect { Potential, Dull, Squeeze, Fragile, Reinforce, Revenge, BOGO };

    public enum ReferenceVariable { Attack, Block, Health, AttackCardsInHand, BlockCardsInHand, SupportCardsInHand, CardsInDeck, CardsInDiscard };

    public enum DeckModify
    {
        DrawCard, DrawFromDiscard, DrawFromAbandon, ShuffleDeck, ShuffleDiscardIntoDeck, ForceDiscardCard,
        ForceDiscardHand, PlayRandomFromHand, PlayRandomFromDeck, PlayRandomFromDiscard, AddSpecificCardToDeck, AbandonThisCard
    };

    public enum ToyAnimations
    {
        Head_Nod1,
        Head_Nod2,
        Torso_ChestBump,
        RH_AttackBasic,
        RH_BasicAtk_VooNinja,
        RH_AttackBlast,
        RH_FistPump,
        LH_BlockBasic,
        LH_BashThrow,
        Legs_ShuffleHop,
        Other_ChestFlex,
        Other_Recoil,
        Legs_TailSwim,
        LH_NailJab,
        Other_VictoryPlayer,
        Other_VictoryEnemy,
        RH_SwordSlash,
        Head_CrookedNod,
        Legs_Kick,
        Other_DeathBreak,
        Torso_TorsoTwist,
        Boss_HeadPuke,
        Boss_DoubleSlam,
        Boss_RHBite,
        Boss_Victory,
        Boss_CarRam
    }
}