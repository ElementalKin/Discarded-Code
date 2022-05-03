using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//What is being changed.
public enum StickerEffectType { Attack, Block, Heal, Battery, Support, CardCost, None }
public class StickerEffect : ScriptableObject {
    public StickerEffectType stickerEffectType;
    public CardHelpers.StatusEffect statusToEdit;
    public bool applyToSelf;
    public int appliedStacks;
}
