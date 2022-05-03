using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateHealText : MonoBehaviour {
    public StateMachine state;
    public int healAmount;
    private void OnEnable() {
        gameObject.GetComponent<TextMeshProUGUI>().SetText("Take a rest\n <i>Heal " + healAmount + " HP. Health " + state.PlayerHand.health + "/" + state.PlayerHand.maxHealth + "</i>");
    }
}
