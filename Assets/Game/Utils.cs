using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Utils
{
    public enum MainMenuUIPage { Base, Multiplayer}
    public static void SetActiveMenuUIPage(MainMenuUIPage page)
    {
        if(page == MainMenuUIPage.Base)
        {
            MainMenuUIManager.Singleton.style.display = DisplayStyle.Flex;
            MultiplayerMenuUIManager.Singleton.style.display = DisplayStyle.None;
        }
        else if(page == MainMenuUIPage.Multiplayer)
        {
            MultiplayerMenuUIManager.Singleton.style.display = DisplayStyle.Flex;
            MainMenuUIManager.Singleton.style.display = DisplayStyle.None;
        }
    }

    public static string[] GetAllLocalIPv4(NetworkInterfaceType _type)
    {
        List<string> ipAddrList = new List<string>();
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
            {
                foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddrList.Add(ip.Address.ToString());
                    }
                }
            }
        }
        return ipAddrList.ToArray();
    }

    public static void UpdateLang(string newLang)
    {
        PlayerPrefs.SetString("lang", newLang);
    }
    public static string GetLang()
    {
        return PlayerPrefs.HasKey("lang") ? PlayerPrefs.GetString("lang") : "en";
    }

    public static Dictionary<string, Dictionary<string, string>> LoadCSV(string name)
    {
        //var dataset = Resources.Load<TextAsset>(name);
        //var text = File.ReadAllText(name);

        var text =
@"
key,en,ru
,test,тест
";

        var dataLines = text.Split('\n');

        Dictionary<string, Dictionary<string, string>> data = new();

        var keys = dataLines[0].Split(',').Skip(1).ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            data.Add(keys[i], new Dictionary<string, string>());
        }

        for (int i = 1; i < dataLines.Length; i++)
        {
            var splData = dataLines[i].Split(',');
            for (int d = 1; d < splData.Length; d++)
            {
                data[keys[d]].Add(splData[0], splData[d]);
            }
        }

        return data;
    }

    public static GameProps CreateGameProps()
    {
        return new GameObject("game_props", typeof(GameProps)).GetComponent<GameProps>(); ;
    }
    public static GameProps GetGameProps()
    {
        return GameObject.FindObjectOfType<GameProps>();
    }

    public static void LoadGameplayScene()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    internal static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
