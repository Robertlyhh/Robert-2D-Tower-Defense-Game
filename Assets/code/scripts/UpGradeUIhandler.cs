using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpGradeUIhandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        UIManager.main.SetHoveringUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        UIManager.main.SetHoveringUI(false);
        gameObject.SetActive(false);
    }
}
