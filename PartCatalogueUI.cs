using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartCatalogueUI : MonoBehaviour
{
    [HideInInspector]
    public CardHandUI cardHandUI;

    public GameObject cardPreviewPrefab;

    public PartsManager player;
    public Image[] playerDisplayImages;

    public Image[] partCardPageImages;
    public Image[] partStickerImages;

    public HorizontalLayoutGroup[] cardPreviewParents;

    public void Start()
    {
        cardHandUI = FindObjectOfType<CardHandUI>();
    }

    private void OnEnable()
    {
        if (cardHandUI == null)
        {
            cardHandUI = FindObjectOfType<CardHandUI>();
        }

        PopulateUI();
    }

    public void PopulateUI()
    {
        // Clear out all the previous cards
        for (int i = 0; i < cardPreviewParents.Length; i++)
        {
            foreach (Transform child in cardPreviewParents[i].gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // LEFT SIDE - Display the parts the player has via the tape sprites.
        for (int i = 0; (i < player.allPartsComponents.Length) && (i < playerDisplayImages.Length); i++)
        {
            // Hide the image by making it clear.
            playerDisplayImages[i].color = Color.clear;

            // If there is a sprite, unhide the image and set the sprite.
            if (player.allPartsComponents[i].imageInUI != null && !player.allPartsComponents[i].dontShowAsTapedPart)
            {
                playerDisplayImages[i].sprite = player.allPartsComponents[i].imageInUI;
                playerDisplayImages[i].color = Color.white;
            }
        }

        // RIGHT SIDE - the buttons to click to choose specific parts (with card previews).
        // For loop for as long as the amount of parts
        for (int i = 0; i < player.allPartsComponents.Length; i++)
        {
            // Use player to populate the UI like the end of battle screen
            partCardPageImages[i].sprite = player.allPartsComponents[i].imageInUI;

            if (player.allPartsComponents[i].sticker != null)
            {
                partStickerImages[i].sprite = player.allPartsComponents[i].sticker.stickerArt;
            }
            partStickerImages[i].color = partStickerImages[i].sprite != null ? Color.white : Color.clear;

            // Instantiate the preview cards.
            for (int j = 0; j < player.allPartsComponents[i].partCards.Count; j++)
            {
                CardUI newCard = Instantiate(cardPreviewPrefab, cardPreviewParents[i].gameObject.transform).GetComponent<CardUI>();

                newCard.DisplayCard(player.allPartsComponents[i].partCards[j], cardHandUI);
                newCard.gameObject.GetComponent<ZoomOnHoverUI>().disableZoom = true;

                if (player.allPartsComponents[i].sticker != null)
                {
                    newCard.descriptionText.text += $" <color=#{ColorUtility.ToHtmlStringRGB(newCard.stickerTextColor)}>{player.allPartsComponents[i].sticker.stickerDescription}</color>";
                }
            }
        }
    }
}