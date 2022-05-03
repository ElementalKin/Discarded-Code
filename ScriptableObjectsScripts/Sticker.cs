using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Discarded/Sticker", fileName = "New Sticker")]
public class Sticker : ScriptableObject {
    //Name of the Sticker as it will apear in the UI.
    public string stickerName;

    // The decription of the Sticker.
    [TextArea(3, 5)]
    public string stickerDescription;

    public Sprite stickerArt;

    public List<StickerEffect> stickerEffects;

    private void OnEnable() {
        // If stickerEffects is null, we know this is the first time
        if (stickerEffects == null) {
            stickerEffects = new List<StickerEffect>();
        }
    }
}
