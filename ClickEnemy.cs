using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEnemy : MonoBehaviour
{
    [SerializeField]
    private LayerMask clickableLayer;
    public static bool rayEnter = false;
    public static bool rayExit = true;
    public static bool rayCastOn = true;

    private void Update()
    {
        // There will ever only be 1 instance of PartSelectionUI in the scene,
        // so this opperation should be safe.
        // If the UI isn't open...
        if (!(FindObjectOfType<PartSelectionUI>().partUIIsOpen))
        {
            if (rayCastOn)
            {
                RaycastHit rayHit;

                //When hovering object with clickable Layer
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickableLayer))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        rayHit.collider.GetComponent<ClickedOn>().Clicked();
                    }
                    else if (rayEnter == false)
                    {
                        rayHit.collider.GetComponent<ClickedOn>().Hovered();
                        rayEnter = true;
                        rayExit = true;
                    }
                }
                else
                {
                    rayEnter = false;
                }
            }

            //// Check for mouse down...
            //if (Input.GetMouseButtonDown(0))
            //{
            //    RaycastHit rayHit;

            //    // And check to see if the raycast has hit the collider of a clickable object.
            //    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickableLayer))
            //    {
            //        // Call that collider's ClickedOn.Clicked.
            //        rayHit.collider.GetComponent<ClickedOn>().Clicked();
            //    }
            //}
        }
    }

}
