using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class MultiplayerMenuUIManager : VisualElement
{
    private static MultiplayerMenuUIManager _singleton;
    public static MultiplayerMenuUIManager Singleton
    {
        get => _singleton;
        private set
        {
            _singleton = value;
        }
    }

    bool isCreated = false;
    private VisualElement m_hostBox;

    public MultiplayerMenuUIManager()
    {
        Singleton = this;
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            if (isCreated)
                return;
            isCreated = true;

            m_hostBox = this.Q("hosts-box");

            this.Q("back-btn")?.RegisterCallback<ClickEvent>((ev) =>
            {
                Utils.SetActiveMenuUIPage(Utils.MainMenuUIPage.Base);
            });

            this.Q("createlan-btn")?.RegisterCallback<ClickEvent>((ev) =>
            {
                NetworkManager.Singleton.StartHost();
            });

            this.Q("connectlan-btn")?.RegisterCallback<ClickEvent>((ev) =>
            {
                Setup();
                NetworkManager.Singleton.JoinGame("");
            });


            

        });
    }

    void Setup()
    {
        string ip = "";
        try
        {
            ip = NetworkManager.Singleton.lanManager.GetIpInLan().ToString();
        }
        catch (Exception ex)
        {
            ip = ex.Message;
        }
        var l = new Label(ip);
        l.style.color = Color.blue;
        l.style.fontSize = 20;
        m_hostBox.Add(l);


        NetworkManager.Singleton.ipCheckingFailed += (ip) =>
        {
            var l = new Label(ip);
            l.style.color = Color.red;
            l.style.fontSize = 50;
            m_hostBox.Add(l);
        };

        NetworkManager.Singleton.ipCheckingSucceeded += (ip) =>
        {
            var l = new Label(ip);
            l.style.color = Color.green;
            l.style.fontSize = 50;
            m_hostBox.Add(l);
        };
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<MultiplayerMenuUIManager, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
