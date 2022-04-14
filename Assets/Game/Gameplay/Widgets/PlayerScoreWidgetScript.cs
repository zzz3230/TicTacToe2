using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScoreWidgetScript : MonoBehaviour
{
    [SerializeField] Image _playerImage;
    [SerializeField] TextMeshProUGUI _playerInfo;

    public void Init(Sprite playerSprite, string playerNick, int playerScore)
    {
        _playerImage.sprite = playerSprite;

        _playerInfo.text = string.Format(_playerInfo.text, playerNick, playerScore);
    }
}
