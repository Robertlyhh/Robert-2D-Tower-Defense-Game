using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    private bool isHoveringUI = false;

    public void Awake()
    {
        main = this;
    }

    public void SetHoveringUI(bool value)
    {
        isHoveringUI = value;
    }

    public bool GetHoveringUI()
    {
        return isHoveringUI;
    }
}
