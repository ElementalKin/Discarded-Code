using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PartSelectionUI : MonoBehaviour
{
    [HideInInspector]
    public bool partUIIsOpen;

    [HideInInspector]
    public CardHandUI cardHandUI;

    public GameObject PartSelectMenu;

    public GameObject cardPreviewPrefab;

    public GameObject stickerView;

    public GameObject zoomParent;

    public GameObject confirmWindow;

    public GameObject particleEffect;

    public GameObject warningText;

    public GameObject gainedCardsParent;
    public GameObject lostCardsParent;
    public Button confirmButton;
    public Vector3 scaleCards = new Vector3(1.8f, 1.8f, 1.8f);

    public Image gainedPartPreview;
    public Image lostPartPreview;
    public Image stickerGainedConfirmPreview;
    public Image stickerPreviousConfirmPreview;

    public PartsManager player;
    public Image[] playerDisplayImages;

    public Button[] partButtons;
    public Image[] partButtonImages;
    public Image[] partButtonStickerImages;

    public GameObject[] PartRewardVFX;

    public HorizontalLayoutGroup[] cardPreviewParents;

    public Image stickerPreview;
    public TextMeshProUGUI PLACEHOLDERDESCRIPTION;

    public UnityEvent RemoveEnemyEvent;
    public UnityEvent SpawnEnemyEvent;

    private EnemyAI lastEnemyDefeated;

    [Range(1, 5)]
    public int dropAmount = 3;

    [Header("DEBUG")]
    public bool dropEverything;
    public Sprite PLACEHOLDER;
    public Sticker PLACEHOLDERSTICKER;

    public MinimapLerp minimapLerp;

    public void Start()
    {
        cardHandUI = FindObjectOfType<CardHandUI>();
        minimapLerp = FindObjectOfType<MinimapLerp>();

        particleEffect.SetActive(false);
    }

    public void OPENSTICKERDEBUG()
    {
        OpenAndPopulateStickerUI(PLACEHOLDERSTICKER);
    }

    public void SpawnEnemies()
    {
        partUIIsOpen = false;
        if (SceneManagment.numberOfBattles % 2 != 0 || stickerView.activeInHierarchy)
        {
            SpawnEnemyEvent.Invoke();
            minimapLerp.OpenMapUI();
            particleEffect.SetActive(false);
        }
    }

    public void SpawnEnemiesAnyways()
    {
        partUIIsOpen = false;
        SpawnEnemyEvent.Invoke();
        minimapLerp.OpenMapUI();
        particleEffect.SetActive(false);
    }

    public void OpenAndPopulateStickerUI(Sticker sticker)
    {
        // Open the part and sticker UI, remove the previous enemys, and toggle the flag for the ui being up on.
        PartSelectMenu.SetActive(true);
        stickerView.SetActive(true);
        RemoveEnemyEvent.Invoke();
        partUIIsOpen = true;

        // Clear out all the previous cards
        for (int i = 0; i < cardPreviewParents.Length; i++)
        {
            foreach (Transform child in cardPreviewParents[i].gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (GameObject VFX in PartRewardVFX)
        {
            VFX.SetActive(false);
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
            // Make the button not interactable, and remove all the functions it calls onClick.
            partButtons[i].interactable = false;
            partButtons[i].onClick.RemoveAllListeners();

            // Use player to populate the UI like the end of battle screen
            partButtonImages[i].sprite = player.allPartsComponents[i].imageInUI;

            if (player.allPartsComponents[i].sticker != null)
            {
                partButtonStickerImages[i].sprite = player.allPartsComponents[i].sticker.stickerArt;
            }
            partButtonStickerImages[i].color = partButtonStickerImages[i].sprite != null ? Color.white : Color.clear;

            // Instantiate the preview cards.
            for (int j = 0; j < player.allPartsComponents[i].partCards.Count; j++)
            {
                CardUI newCard = Instantiate(cardPreviewPrefab, cardPreviewParents[i].gameObject.transform).GetComponent<CardUI>();

                newCard.DisplayCard(player.allPartsComponents[i].partCards[j], cardHandUI);
                newCard.gameObject.GetComponent<ZoomOnHoverUI>().disableZoom = true;
            }

            int index = i;

            // Set the button to be interactable again, with a listener to open the sticker confirmation screen.
            partButtons[i].interactable = true;
            partButtons[i].onClick.AddListener(delegate { OpenConfirmationScreenSticker(index, sticker); });
        }

        // PLACEHOLDER, text element to display what the sticker does in place of tooltip
        PLACEHOLDERDESCRIPTION.text = sticker.stickerDescription;

        stickerPreview.sprite = sticker.stickerArt;
    }

    public void OpenAndPopulateUI(EnemyAI enemyDefeated)
    {
        // Open the part UI, remove the previous enemys, and toggle the flag for the ui being up on.
        confirmWindow.SetActive(false);
        RemoveEnemyEvent.Invoke();
        particleEffect.SetActive(true);
        partUIIsOpen = true;
        stickerView.SetActive(false);

        // DEBUG BEGIN
        if (dropEverything)
        {
            dropAmount = enemyDefeated.partDrops.Length;
        }
        // DEBUG END

        lastEnemyDefeated = enemyDefeated;

        // Clear out all the previous cards
        for (int i = 0; i < cardPreviewParents.Length; i++)
        {
            foreach (Transform child in cardPreviewParents[i].gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (GameObject VFX in PartRewardVFX)
        {
            VFX.SetActive(true);
        }

        PartSelectMenu.SetActive(true);

        // LEFT SIDE - Display the parts the player has via the tape sprites.
        for (int i = 0; (i < player.allPartsComponents.Length) && (i < playerDisplayImages.Length); i++)
        {
            // Hide the image by making it clear.
            playerDisplayImages[i].color = Color.clear;

            if (player.allPartsComponents[i].sticker != null)
            {
                partButtonStickerImages[i].sprite = player.allPartsComponents[i].sticker.stickerArt;
            }
            partButtonStickerImages[i].color = partButtonStickerImages[i].sprite != null ? Color.white : Color.clear;

            // If there is a sprite, unhide the image and set the sprite.
            if (player.allPartsComponents[i].imageInUI != null && !player.allPartsComponents[i].dontShowAsTapedPart)
            {
                playerDisplayImages[i].sprite = player.allPartsComponents[i].imageInUI;
                playerDisplayImages[i].color = Color.white;
            }
        }

        // RIGHT SIDE - the buttons to click to choose specific parts (with card previews).
        for (int i = 0; i < enemyDefeated.partDrops.Length; i++)
        {
            // If for some reason partDrops is null, continue because we don't want to be causing errors by grabing null things.
            if (enemyDefeated.partDrops[i] == null)
            {
                partButtonImages[i].sprite = PLACEHOLDER;

                continue;
            }

            // Grab the enemy part.
            enemyDefeated.partDrops[i].TryGetComponent<Part>(out Part currEnemyPart);

            // Display to UI
            if (currEnemyPart.imageInUI != null)
            {
                partButtonImages[i].sprite = currEnemyPart.imageInUI;
            }
            else /*Remove this once we have all the assets in*/
            {
                partButtonImages[i].sprite = PLACEHOLDER;
            }

            // Disable buttons and remove all functions called by onClick.
            partButtons[i].interactable = false;
            partButtons[i].onClick.RemoveAllListeners();
        }

        for (int i = 0; i < dropAmount; i++)
        {
            // Roll drops.
            // If the drop has been previously rolled, roll again.
            // DO WHILE BEING USEFUL???? YA LOVE TO SEE IT TBH.
            int drop = 0;
            do
            {
                drop = Random.Range(0, enemyDefeated.partDrops.Length);
            }
            while (partButtons[drop].interactable);


            // DEBUG BEGIN
            if (dropEverything)
            {
                drop = i;
            }
            // DEBUG END

            // If part drops are null, continue.
            if (enemyDefeated.partDrops[drop] == null)
            {
                continue;
            }

            // Enable and add the listeners to the rolled buttons.
            partButtons[drop].interactable = true;
            partButtons[drop].onClick.AddListener(delegate { OpenConfirmationScreenPart(drop); });

            Part newPart = enemyDefeated.partDrops[drop].GetComponent<Part>();

            // Add the cards from this part to the preview
            for (int j = 0; j < newPart.partCards.Count; j++)
            {
                CardUI newCard = Instantiate(cardPreviewPrefab, cardPreviewParents[drop].gameObject.transform).GetComponent<CardUI>();

                newCard.DisplayCard(newPart.partCards[j], cardHandUI);
                newCard.gameObject.GetComponent<ZoomOnHoverUI>().disableZoom = true;
            }
        }
    }

    public void OpenConfirmationScreenPart(int index)
    {
        confirmWindow.SetActive(true);

        // Discard any cards from last time in the gained cards;
        foreach (Transform child in gainedCardsParent.transform)
        {
            Destroy(child.gameObject);
        }

        Part playerPart = player.allParts[index].GetComponent<Part>();

        warningText.SetActive(false);

        // Add new gained cards.
        for (int i = 0; i < lastEnemyDefeated.partDrops[index].GetComponent<Part>().partCards.Count; i++)
        {
            CardUI newCard = Instantiate(cardPreviewPrefab, gainedCardsParent.transform).GetComponent<CardUI>();
            newCard.gameObject.GetComponent<ZoomOnHoverUI>().disableZoom = true;
            newCard.gameObject.GetComponent<RectTransform>().localScale = scaleCards;

            newCard.DisplayCard(lastEnemyDefeated.partDrops[index].GetComponent<Part>().partCards[i], cardHandUI);

            if (playerPart.sticker != null)
            {
                newCard.descriptionText.text += $" <color=#{ColorUtility.ToHtmlStringRGB(newCard.stickerTextColor)}>{playerPart.sticker.stickerDescription}</color>";
            }
        }

        // Discard any cards from last time in the lost cards;
        foreach (Transform child in lostCardsParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new lost cards.
        for (int i = 0; i < playerPart.partCards.Count; i++)
        {
            CardUI newCard = Instantiate(cardPreviewPrefab, lostCardsParent.transform).GetComponent<CardUI>();
            newCard.gameObject.GetComponent<ZoomOnHoverUI>().disableZoom = true;
            newCard.gameObject.GetComponent<RectTransform>().localScale = scaleCards;

            newCard.DisplayCard(playerPart.partCards[i], cardHandUI);

            if (playerPart.sticker != null)
            {
                newCard.descriptionText.text += $" <color=#{ColorUtility.ToHtmlStringRGB(newCard.stickerTextColor)}>{playerPart.sticker.stickerDescription}</color>";
            }
        }

        // Replace the sprites.
        lostPartPreview.sprite = player.allParts[index].GetComponent<Part>().imageInUI;
        lostPartPreview.gameObject.GetComponent<RectTransform>().sizeDelta = partButtonImages[index].GetComponent<RectTransform>().sizeDelta;

        gainedPartPreview.sprite = lastEnemyDefeated.partDrops[index].GetComponent<Part>().imageInUI;
        gainedPartPreview.gameObject.GetComponent<RectTransform>().sizeDelta = partButtonImages[index].GetComponent<RectTransform>().sizeDelta;

        stickerGainedConfirmPreview.sprite = null;
        stickerPreviousConfirmPreview.sprite = null;

        if (player.allPartsComponents[index].sticker != null)
        {
            stickerGainedConfirmPreview.sprite = player.allPartsComponents[index].sticker.stickerArt;
            stickerPreviousConfirmPreview.sprite = player.allPartsComponents[index].sticker.stickerArt;
        }
        stickerGainedConfirmPreview.color = stickerGainedConfirmPreview.sprite != null ? Color.white : Color.clear;
        stickerPreviousConfirmPreview.color = stickerPreviousConfirmPreview.sprite != null ? Color.white : Color.clear;

        confirmButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(delegate { 
            player.ReplacePart(index, lastEnemyDefeated.partDrops[index]); 
            PartSelectMenu.SetActive(false);
            confirmWindow.SetActive(false);
            particleEffect.SetActive(false);
            partUIIsOpen = false;
            if (SceneManagment.numberOfBattles % 2 != 0)
            {
                SpawnEnemies();
            }
        });
    }

    public void OpenConfirmationScreenSticker(int index, Sticker sticker)
    {
        confirmWindow.SetActive(true);

        // Discard any cards from last time in the gained cards;
        foreach (Transform child in gainedCardsParent.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new gained cards.
        for (int i = 0; i < player.allParts[index].GetComponent<Part>().partCards.Count; i++)
        {
            CardUI newCard = Instantiate(cardPreviewPrefab, gainedCardsParent.transform).GetComponent<CardUI>();

            newCard.DisplayCard(player.allParts[index].GetComponent<Part>().partCards[i], cardHandUI);
            newCard.gameObject.GetComponent<ZoomOnHoverUI>().disableZoom = true;
            newCard.gameObject.GetComponent<RectTransform>().localScale = scaleCards;

            newCard.descriptionText.text += $" <color=#{ColorUtility.ToHtmlStringRGB(newCard.stickerTextColor)}>{sticker.stickerDescription}</color>";
        }

        // Discard any cards from last time in the lost cards;
        foreach (Transform child in lostCardsParent.transform)
        {
            Destroy(child.gameObject);
        }

        Part playerPart = player.allParts[index].GetComponent<Part>();

        warningText.SetActive(playerPart.sticker != null);

        // Add new lost cards.
        for (int i = 0; i < playerPart.partCards.Count; i++)
        {
            CardUI newCard = Instantiate(cardPreviewPrefab, lostCardsParent.transform).GetComponent<CardUI>();

            newCard.DisplayCard(playerPart.partCards[i], cardHandUI);
            newCard.gameObject.GetComponent<ZoomOnHoverUI>().disableZoom = true;
            newCard.gameObject.GetComponent<RectTransform>().localScale = scaleCards;

            if (playerPart.sticker != null)
            {
                newCard.descriptionText.text += $" <color=#{ColorUtility.ToHtmlStringRGB(newCard.stickerTextColor)}>{playerPart.sticker.stickerDescription}</color>";
            }
        }

        // Replace the sprites.
        lostPartPreview.sprite = player.allParts[index].GetComponent<Part>().imageInUI;
        lostPartPreview.gameObject.GetComponent<RectTransform>().sizeDelta = partButtonImages[index].GetComponent<RectTransform>().sizeDelta;

        gainedPartPreview.sprite = player.allParts[index].GetComponent<Part>().imageInUI;
        gainedPartPreview.gameObject.GetComponent<RectTransform>().sizeDelta = partButtonImages[index].GetComponent<RectTransform>().sizeDelta;

        stickerGainedConfirmPreview.sprite = null;
        stickerPreviousConfirmPreview.sprite = null;

        stickerGainedConfirmPreview.sprite = sticker.stickerArt;
        stickerGainedConfirmPreview.color = stickerGainedConfirmPreview.sprite != null ? Color.white : Color.clear;

        if (player.allPartsComponents[index].sticker != null)
        {
            stickerPreviousConfirmPreview.sprite = player.allPartsComponents[index].sticker.stickerArt;
        }
        stickerPreviousConfirmPreview.color = stickerPreviousConfirmPreview.sprite != null ? Color.white : Color.clear;

        confirmButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(delegate {
            player.AddSticker(sticker, index);
            PartSelectMenu.SetActive(false);
            stickerView.SetActive(false);
            confirmWindow.SetActive(false);
            particleEffect.SetActive(false);
            partUIIsOpen = false;
            SpawnEnemiesAnyways();
        });
    }
}