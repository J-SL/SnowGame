using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class detectBtn : MonoBehaviour
{
    [SerializeField] private TMP_Text btnText;
    public bool IsDetect;

    public void Btn()
    {
        if(btnText.text== "¼ì²â£¨¿ª£©")
        {
            btnText.text = "¼ì²â£¨¹Ø£©";
            IsDetect = false;
        }
        else
        {
            btnText.text = "¼ì²â£¨¿ª£©";
            IsDetect = true;
        }
    }
}
