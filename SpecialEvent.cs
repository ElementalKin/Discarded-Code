using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialEvent : MonoBehaviour {
    public Sticker stickerToAdd;
    public Sticker secondStickerToAdd;
    public PartSelectionUI partSelectionUI;

    public Image stickerOne;
    public Image stickerTwo;

    private void OnEnable()
    {
        if (stickerToAdd != null && stickerOne != null)
        {
            stickerOne.sprite = stickerToAdd.stickerArt;
        }

        if (secondStickerToAdd != null && stickerTwo != null)
        {
            stickerTwo.sprite = secondStickerToAdd.stickerArt;
        }
    }

    public void openStickerRewardScreen() {
        partSelectionUI.OpenAndPopulateStickerUI(stickerToAdd);
        gameObject.SetActive(false);
    }

    public void openStickerRewardScreenTwo()
    {
        partSelectionUI.OpenAndPopulateStickerUI(secondStickerToAdd);
        gameObject.SetActive(false);
    }
}
