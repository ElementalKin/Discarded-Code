using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ZoomOnHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform myRect;

    //[HideInInspector]
    //public GameObject zoomedParent;

    [SerializeField]
    private float currTime;
    public float zoomTime = 1;

    [SerializeField]
    private float desiredScale;
    public float startScale = 1;
    public float endScale = 2;

    public bool disableZoom = false;

    private void Start()
    {
        myRect = GetComponent<RectTransform>();

        desiredScale = startScale;
        currTime = zoomTime;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!disableZoom)
        {
            desiredScale = endScale;
            currTime = 0;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!disableZoom)
        {
            desiredScale = startScale;
            currTime = 0;
        }
    }

    private void Update()
    {
        if (!disableZoom)
        {
            if (currTime < zoomTime)
            {
                Vector3 scale = Vector3.Lerp(myRect.localScale, new Vector3(desiredScale, desiredScale, desiredScale), currTime / zoomTime);
                currTime += Time.deltaTime;

                myRect.localScale = scale;
            }
        }
    }
}