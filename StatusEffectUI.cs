using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectUI : MonoBehaviour
{
    public Image statusImage;
    public TextMeshProUGUI stacksLeftText;

    public int statusIndex;

    public void UpdateStackAmount(Deck targetDeck)
    {
        stacksLeftText.text = targetDeck.statusList[statusIndex].ToString();

        gameObject.SetActive(targetDeck.statusList[statusIndex] > 0);
    }
}
