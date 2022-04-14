using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerTurnWidgetScript : MonoBehaviour, IPointerClickHandler
{
    public Image[] turnImages;
    public Image image;

    public MainGameplayWidgetScript mainWidget;
    public Vector2Int pos;

    public void Init(int posX, int posY, MainGameplayWidgetScript mainGameplayWidget)
    {
        pos = new Vector2Int(posX, posY);
        mainWidget = mainGameplayWidget;
    }

    public void SetImage(int index)
    {
        //if (imageParent.transform.childCount > 1)
        //    Destroy(imageParent.transform.GetChild(1));

        //Instantiate(turnImages[index]).transform.SetParent(imageParent.transform);

        image.sprite = turnImages[index].sprite;
        image.color = turnImages[index].color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        mainWidget.UserClicked(pos);
    }
}
