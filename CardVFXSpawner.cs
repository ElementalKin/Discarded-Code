using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardVFXSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardHandUI cardHandUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardHandUI.selectedCardUI != null)
        {
            foreach (var VFXGameObject in cardHandUI.selectedCardUI.cardPlayingFX)
            {
                VFXGameObject.SetActive(true);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardHandUI.selectedCardUI != null)
        {
            foreach (var VFXGameObject in cardHandUI.selectedCardUI.cardPlayingFX)
            {
                VFXGameObject.SetActive(false);
            }
        }
    }
}
