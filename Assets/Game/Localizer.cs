using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class Localizer
{
    static Dictionary<string, Dictionary<string, string>> data = new()
    {
        {   
            "en", 
            new() 
            {
                { "inLine", "{0} in a line to win" },
                { "nextPlayer", "NEXT PLAYER" },
                { "main-play-btn", "Play" },
                { "setup-start", "Start" },
                { "field-size", "Field size" },
                { "player-count", "Player count" },
                { "won", "WON" },
                { "continue", "Continue" },
                { "replay", "Replay" },
                { "exit", "Exit" },
                { "curRound", "ROUND<br><size=700>{0}" },
            } 
        },
        {
            "ru",
            new()
            {
                { "inLine", "{0} в ряд для победы" },
                { "nextPlayer", "СЛЕДУЮЩИЙ ИГРОК" },
                { "main-play-btn", "Играть" },
                { "setup-start", "Начать" },
                { "field-size", "Размер поля" },
                { "player-count", "Количество игроков" },
                { "won", "ПОБЕДИЛ" },
                { "continue", "Продолжить" },
                { "replay", "Переиграть" },
                { "exit", "Выйти" },
                { "curRound", "РАУНД<br><size=700>{0}" },
            }
        },
        {
            "fr",
            new()
            {
                { "inLine", "{0} en ligne pour gagner" },
                { "nextPlayer", "JOUEUR SUIVANT" },
                { "main-play-btn", "Jouer" },
                { "setup-start", "Commencer" },
                { "field-size", "Taille du champ" },
                { "player-count", "Nombre de joueurs" },
                { "won", "GAGNÉ" },
                { "continue", "Continuez" },
                { "replay", "Rejouer" },
                { "exit", "Sortir" },
            }
        },
        {
            "zh",
            new()
            {
                { "inLine", "{0} x" },
                { "nextPlayer", "x" },
                { "main-play-btn", "x" },
                { "setup-start", "x" },
                { "field-size", "x" },
                { "player-count", "x" },
                { "won", "x" },
                { "continue", "x" },
                { "replay", "x" },
                { "exit", "x" },
            }

        }
    };

    public static void ChangeLang(string newLang)
    {
        langName = newLang;
    }

    public static string langName = Utils.GetLang();//"ru";
    public static string Localize(string key, params string[] args)
    {
        var str = data[langName][key];
        if(args != null)
            return string.Format(str, args);
        return str;
    }
}
