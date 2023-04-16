using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnlyZoomUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float zoomSize;//Àı∑≈¥Û–°

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        transform.localScale = Vector3.one;
    }


}
