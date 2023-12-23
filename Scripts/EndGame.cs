using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public TextAnimatorPlayer textAnimatorPlayer;

    [SerializeField] private GameObject HP;
    [SerializeField] private TMP_Text warn;
    private bool isDead;
    void Update()
    {
        var hp= HP.GetComponent<Image>().material.GetFloat("_value");

        if (hp <= 0&&!isDead)
        {    
            StartCoroutine(End());
            isDead = true;      
        }
    }

    IEnumerator End()
    {
        warn.gameObject.SetActive(true);
        warn.transform.GetChild(0).gameObject.SetActive(false);
        warn.GetComponent<TextSetting>().Show("<wiggle>Game Over!</wiggle>");
        yield return new WaitForSeconds(6);
        Time.timeScale = 0;
    }
}
