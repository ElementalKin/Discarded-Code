using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Discarded/Card", fileName = "New Card")]
public class Card : ScriptableObject
{
    // Name of the Card as it will apear in the UI.
    public string cardName;

    // The decription of the
    [TextArea(3, 5)]
    public string cardDescription;

    public Sprite cardArt;
    public Sprite cardWatermark;
    public CardHelpers.BodyPart bodyPart;
    public Sprite cardFrame;
    public CardHelpers.ToyAnimations animationOnPlay;

    public SoundControls[] attackSounds;

    public int cardCost;

    public int cardID;

    public List<CardEffect> cardEffects;

    public Card cardUpgrade;

    public bool isFinisher;

    public bool isTrapCard;

    private void OnEnable()
    {
        // If cardEffects is null, we know this is the first time
        if (cardEffects == null)
        {
            cardEffects = new List<CardEffect>();

            // Default cost is 1.
            cardCost = 1;

            isFinisher = false;

            isTrapCard = false;
        }
    }
}
