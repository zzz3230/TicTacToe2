using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshProLocalizer : MonoBehaviour
{
    public string key;
    private void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = Localizer.Localize(key);
    }
}