using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;
using System.Linq;
using System;

public class MainMenuUIManager : VisualElement
{
    private static MainMenuUIManager _singleton;
    public static MainMenuUIManager Singleton
    {
        get => _singleton;
        private set
        {
            _singleton = value;
        }
    }

    VisualElement m_mainPage;
    VisualElement m_setupPage;
    VisualElement m_settingsPage;

    VisualElement[] m_playersNickFields;
    VisualElement[] m_playersCountBoxs;
    VisualElement[] m_gameFieldSizes;

    //const int maxPlayersCount = 4;
    //readonly int[] gameFieldSizes = new int[] {3, 7, 10, 13};
    void LocalizeButton(string name)
    {
        this.Q<Button>(name).text = Localizer.Localize(name);
    }
    void LocalizeLabel(string name)
    {
        this.Q<Label>(name).text = Localizer.Localize(name);
    }


    int playersCount = -1;
    int fieldSize = -1;

    bool isCreated = false;

    public MainMenuUIManager()
    {
        Singleton = this;
        
        RegisterCallback<GeometryChangedEvent>((ev) =>
        {
            if (isCreated)
                return;
            isCreated = true;

            Utils.SetActiveMenuUIPage(Utils.MainMenuUIPage.Base);


            m_mainPage = this.Q("main-page");
            m_setupPage = this.Q("setup-page");
            m_settingsPage = this.Q("settings-page");


            LocalizeButton("main-play-btn");
            this.Q("main-play-btn")?.RegisterCallback<ClickEvent>((ev) => 
            {
                m_mainPage.style.display = DisplayStyle.None;
                m_setupPage.style.display = DisplayStyle.Flex;
            });

            LocalizeButton("setup-start");
            this.Q("setup-start")?.RegisterCallback<ClickEvent>((ev) =>
            {
                Start();
            });


            this.Q("main-multiplayer-btn")?.RegisterCallback<ClickEvent>((ev) =>
            {
                Utils.SetActiveMenuUIPage(Utils.MainMenuUIPage.Multiplayer);
            });

            LocalizeLabel("field-size");
            LocalizeLabel("player-count");

            m_gameFieldSizes = this.Q("fields-boxes").Children().Cast<VisualElement>().ToArray();
            m_playersCountBoxs = this.Q("player-count-boxes").Children().Cast<VisualElement>().ToArray();
            m_playersNickFields = this.Q("players-nicks-fields").Children().Cast<VisualElement>().ToArray();

            for (int i = 0; i < m_gameFieldSizes.Length; i++)
            {
                m_gameFieldSizes[i].RegisterCallback<ClickEvent>((ev) =>
                {
                    var self = (VisualElement)ev.currentTarget;

                    foreach (var f in m_gameFieldSizes)
                    {
                        f.RemoveFromClassList("menu-field-box-selected");
                    }
                    //Debug.Log(self.name); // .. ..
                    self.AddToClassList("menu-field-box-selected");

                    fieldSize = int.Parse(self.viewDataKey);
                });
            }

            for (int i = 0; i < m_gameFieldSizes.Length; i++)
            {
                if (i > m_playersCountBoxs.Length - 1)
                    continue;

                m_playersCountBoxs[i]?.RegisterCallback<ClickEvent>((ev) =>
                {
                    var self = (VisualElement)ev.currentTarget;

                    foreach (var f in m_playersCountBoxs)
                    {
                        f.RemoveFromClassList("menu-field-box-selected");
                    }
                    //Debug.Log(self.name);
                    self.AddToClassList("menu-field-box-selected");

                    var showNicksCount = int.Parse(self.viewDataKey);
                    //(new Dictionary<string, int> { 
                    //    { "", 2 }, 
                    //    { "", 3 }, 
                    //    { "", 4 } 
                    //}this.Q($"lang-{langs[i]}-btn")?
                    //)[self.name];

                    for (int i = 0; i < m_playersNickFields.Length; i++)
                    {
                        m_playersNickFields[i].style.display = DisplayStyle.None;
                    }
                    for (int i = 0; i < showNicksCount; i++)
                    {
                        m_playersNickFields[i].style.display = DisplayStyle.Flex;
                    }

                    playersCount = showNicksCount;
                });
            }

            /*string[] langs = { "en", "ru" };
            for (int i = 0; i < langs.Length; i++)
            {
                var btn = this.Q($"lang-{langs[i]}-btn");

                if(btn != null)
                {
                    btn.userData = i;

                    btn.RegisterCallback<ClickEvent>((ev) =>
                    {
                        ev.target.
                    });
                }
                
            }*/

            this.Q($"lang-en-btn").RegisterCallback<ClickEvent>((ev) => 
            {
                Localizer.langName = "en";
                Utils.UpdateLang("en");
                Utils.ReloadScene();
            });

            this.Q($"lang-ru-btn").RegisterCallback<ClickEvent>((ev) =>
            {
                Localizer.langName = "ru";
                Utils.UpdateLang("ru");
                Utils.ReloadScene();
            });

            this.Q($"lang-zh-btn").RegisterCallback<ClickEvent>((ev) =>
            {
                Localizer.langName = "zh";
                Utils.UpdateLang("zh");
                Utils.ReloadScene();
            }); 
            
            this.Q($"lang-fr-btn").RegisterCallback<ClickEvent>((ev) =>
            {
                Localizer.langName = "fr";
                Utils.UpdateLang("fr");
                Utils.ReloadScene();
            });
        });
    }

    void Start()
    {
        if(playersCount == -1 || fieldSize == -1)
        {
            Debug.Log("select!");
        }
        else
        {
            var props = Utils.CreateGameProps();
            props.SetPlayersNicks(new string[4]
            {
                this.Q<TextField>("player1-nick")?.text,
                this.Q<TextField>("player2-nick")?.text,
                this.Q<TextField>("player3-nick")?.text,
                this.Q<TextField>("player4-nick")?.text,
            });

            props.totalRounds = 1;

            props.fieldSize = fieldSize;
            props.playersCount = playersCount;

            Utils.LoadGameplayScene();
        }
    }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<MainMenuUIManager, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
