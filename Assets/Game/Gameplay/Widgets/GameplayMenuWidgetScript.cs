using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMenuWidgetScript : MonoBehaviour
{
    [SerializeField] Canvas mainCanvas;

    public void Show()
    {
        mainCanvas.enabled = true;
    }

    public void Hide()
    {
        mainCanvas.enabled = false;
    }

    public void ContinueClick()
    {
        Hide();
    }

    public void ReplayClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitClick()
    {
        SceneManager.LoadScene(0);
    }
}
