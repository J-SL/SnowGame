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
        if(btnText.text== "��⣨����")
        {
            btnText.text = "��⣨�أ�";
            IsDetect = false;
        }
        else
        {
            btnText.text = "��⣨����";
            IsDetect = true;
        }
    }
}
