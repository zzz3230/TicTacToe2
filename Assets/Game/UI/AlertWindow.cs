using Lean.Gui;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertWindow : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] LeanWindow alertWindow;
    [SerializeField] Image alertImage;
    [SerializeField] Image alertBackgroundImage;
    Action _onClose;

    public void Show(string _text, Sprite imageSprite, Action onClose)
    {
        _onClose = onClose;
        text.text = _text;
        alertWindow.TurnOn();
        if(imageSprite != null)
        {
            alertImage.sprite = imageSprite;
        }
        else
        {
            alertBackgroundImage.enabled = false;
        }
    }

    public void OnOff()
    {
        _onClose();
    }
}
