using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSetting : MonoBehaviour
{
    [SerializeField]
    TextAnimatorPlayer textPlayer;
    

    public void Show(string text)
    {
        textPlayer.ShowText(text);
        
    }

    public void Close()
    {
        transform.gameObject.SetActive(false);
    }
}
