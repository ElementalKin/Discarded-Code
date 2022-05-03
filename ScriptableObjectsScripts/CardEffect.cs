using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : ScriptableObject
{
    public bool applyToSelf;

    public CardHelpers.EffectType effectType; // What kind of effect this is.

    public bool referenceVariable;
    public bool applyMultipleTimes;
    public int appliedStacks;
    public CardHelpers.ReferenceVariable variable;
    public int effectNumberTimes;

    public CardHelpers.StatusEffect statusToEdit;

    public CardHelpers.DeckModify DeckAction;
    public Card cardToAdd;
}