using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using Lean.Gui;
using TMPro;
// 
public class MainGameplayWidgetScript : MonoBehaviour
{
    [SerializeField] GridLayoutGroup playingField;
    [SerializeField] PlayerTurnWidgetScript playerTurnWidgetOriginal;
    [SerializeField] Slider forWinSlider;
    [SerializeField] TextMeshProUGUI forWinTextMesh;
    [SerializeField] TextMeshProUGUI currentRoundTextMesh;
    [SerializeField] PlayerTurnWidgetScript previewNextCell;
    [SerializeField] AlertWindow alertWindow;
    [SerializeField] GameplayMenuWidgetScript gameplayMenuWidgetScript;
    [SerializeField] GridLayoutGroup scorePanel;
    [SerializeField] PlayerScoreWidgetScript playerScoreWidgetOriginal;


    PlayerTurnWidgetScript[,] playersCells;
    int[,] field;

    public int fieldSize = 10;
    public int maxPlayers = 3;
    public int forWin = 4;
    bool isPlaying = true;

    void Start()
    {
        MakeScore();

        var props = Utils.GetGameProps();
        fieldSize = props.fieldSize;
        maxPlayers = props.playersCount;
        switch (fieldSize)
        {
            case 3:
                forWin = 3;
                break;
            case 7:
                forWin = 4;
                if (maxPlayers == 2)
                    forWin += 1;
                break;
            case 10:
                forWin = 4;
                if (maxPlayers == 2)
                    forWin += 1;
                else if (maxPlayers == 4)
                    forWin -= 1;
                break;
            case 13:
                forWin = 5;
                if (maxPlayers == 2)
                    forWin += 2;
                else if (maxPlayers == 4)
                    forWin -= 1;
                break;
        }

        //Debug.Log();
        forWinTextMesh.text = Localizer.Localize("inLine", forWin.ToString());
        currentRoundTextMesh.text = Localizer.Localize("curRound", props.totalRounds.ToString());

        playingField.constraintCount = fieldSize;
        var fieldWidth = Screen.width;
        float slotWidth = fieldWidth / fieldSize - playingField.spacing.x;
        //slotWidth *= 0.87f;

        playingField.cellSize = new Vector2(slotWidth, slotWidth);

        playersCells = new PlayerTurnWidgetScript[fieldSize, fieldSize];
        field = new int[fieldSize, fieldSize];

        previewNextCell.Init(-1, -1, this);
        previewNextCell.SetImage(1);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                var turn = Instantiate(playerTurnWidgetOriginal);
                turn.Init(i, j, this);
                turn.SetImage(0);
                turn.transform.SetParent(playingField.transform);
                playersCells[i, j] = turn;

                field[i, j] = -1;
            }
        }
    }

    void MakeScore()
    {
        for (int i = 0; i < scorePanel.transform.childCount; i++)
        {
            Destroy(scorePanel.transform.GetChild(i).gameObject);
        }

        var props = Utils.GetGameProps();
        for (int i = 0; i < props.playersCount; i++)
        {
            var scoreWidget = Instantiate(playerScoreWidgetOriginal);
            scoreWidget.transform.SetParent(scorePanel.transform);
            scoreWidget.Init(
                playerTurnWidgetOriginal.turnImages[i + 1].sprite, 
                props.playersNicks[i], 
                props.playersScore[props.playersNicks[i]]
                );
        }
        
    }

    int currentPlayerIndex = 0;

    void NextPlayer()
    {
        currentPlayerIndex++;
        if(currentPlayerIndex >= maxPlayers)
            currentPlayerIndex = 0;
    }

    public void Restart()
    {
        gameplayMenuWidgetScript.ReplayClick();
    }

    public void OpenMenu()
    {
        gameplayMenuWidgetScript.Show();
    }

    public void ChangeForWin(int f)
    {
        forWin = (int)forWinSlider.value;
    }

    
    void ApplyWin(int player)
    {
        alertWindow.Show(
            Localizer.Localize("won"), 
            playerTurnWidgetOriginal.turnImages[player + 1].sprite ,// class playerTurnWidget contain player images,
            () => Restart()
            );

        Utils.GetGameProps().ApplyPlayerWin(player);
        Utils.GetGameProps().AddRound();
        MakeScore();

        isPlaying = false;
    }

    void CheckForWin()
    {
        bool Check(int x, int y, int xm, int ym, int count, int checking)
        {
            if (x + xm < 0 | x + xm >= fieldSize | y + ym >= fieldSize | y + ym < 0)
                return false;

            if(field[x + xm, y + ym] == checking)
            {
                if (count + 3 > forWin /*- 1*/)
                    return true;
                else
                    return Check(x + xm, y + ym, xm, ym, count + 1, checking);
            }
            else
                return false;
        }
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                if(field[i, j] != -1)
                {
                    if (
                        Check(i, j, 1, 0, 0, field[i, j]) |
                        Check(i, j, -1, 0, 0, field[i, j]) |
                        Check(i, j, 0, 1, 0, field[i, j]) |
                        Check(i, j, 0, -1, 0, field[i, j]) |
                        Check(i, j, 1, 1, 0, field[i, j]) |
                        Check(i, j, -1, -1, 0, field[i, j]) |
                        Check(i, j, 1, -1, 0, field[i, j]) |
                        Check(i, j, -1, 1, 0, field[i, j])
                        )
                    {
                        ApplyWin(field[i, j]);
                        return;
                    }
                    //print(field[i, j] + " won!");''

                }
            }
        }
    }

    void CheckForDraw()
    {
        if (!isPlaying)
            return;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                if (field[i, j] == -1)
                {
                    return;
                }
            }
        }
        Utils.GetGameProps().AddRound();
        Restart();
    }

    public void UserClicked(Vector2Int pos)
    {
        if (!isPlaying)
            return;

        //print(pos);
        if(pos.x >= 0 && pos.y >= 0)
            if(field[pos.x, pos.y] == -1)
            {
                playersCells[pos.x, pos.y].SetImage(currentPlayerIndex + 1);
                field[pos.x, pos.y] = currentPlayerIndex;
                CheckForWin();
                NextPlayer();
                CheckForDraw();

                previewNextCell.SetImage(currentPlayerIndex + 1);
            }
    }

    void Update()
    {
        
    }
}
