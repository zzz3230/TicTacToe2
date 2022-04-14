using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProps : MonoBehaviour
{
    public int fieldSize;
    public int playersCount;

    public string[] playersNicks = new string[0];
    public Dictionary<string, int> playersScore = new();
    public int totalRounds;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AddRound()
    {
        totalRounds++;
    }

    public void SetPlayersNicks(string[] nicks)
    {
        playersScore.Clear();
        playersNicks = nicks;
        for (int i = 0; i < nicks.Length; i++)
        {
            //print(i + "   " + nicks[i]);

            if(nicks[i] != null)
                playersScore.Add(nicks[i], 0);
        }
    }

    public void ApplyPlayerWin(int player)
    {
        playersScore[playersNicks[player]]++;
        print("win applyed");
    }
}
