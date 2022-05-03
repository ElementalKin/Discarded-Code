using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSwap : MonoBehaviour
{
    public Texture2D cursorBasic;
    public Texture2D cursorClicked;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursorClicked, hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(cursorBasic, hotSpot, cursorMode);
        }

        if (Input.GetKeyDown(KeyCode.K) && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        {
            gameObject.GetComponent<StateMachine>().ai[0].enemyDeck.health = 0;
        }
    }
}
