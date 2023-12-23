using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShaderObjects : MonoBehaviour
{
    public UnityEvent OverShading;

    [SerializeField] private GameObject HP;
    [SerializeField] TMP_Text info;
    [SerializeField] ParticleSystem particleSystem;

    [SerializeField] TMP_Text t;
    private int shaderNum;
    [SerializeField] Image circle;
    public float duration = 15f; // 插值的总时长


    IEnumerator Shad()
    {
        float elapsedTime = 0f;
        circle.gameObject.SetActive(true);
        while (elapsedTime < duration)
        {
            circle.fillAmount = Mathf.Lerp(0, 1, elapsedTime / duration);


            elapsedTime += Time.deltaTime;

            yield return null;
        }
        Shading();
        OverShading?.Invoke();
        RayDetection.currentTarget.layer = LayerMask.NameToLayer("Default");
        RayDetection.currentTarget = null;
        RayDetection.lastTarget = null;
        circle.gameObject.SetActive(false);
        shaderNum++;
        t.text = shaderNum.ToString();
        if (shaderNum == 25)
        {
            info.gameObject.SetActive(true);
            info.GetComponent<TextSetting>().Show("<rainb><pend>目标达成，谢谢游戏！</pend></rainb>");
            particleSystem.startColor = new Color(255, 0, 194, 255);
            HP.GetComponent<Image>().material.SetFloat("_value", 1);
            GameObject.Find("Fire/Collision").SetActive(false);
        }else
            DisplayWords(words);
        yield break;
    }

    [SerializeField] List<string> words=new();
    private void DisplayWords(List<string> words)
    {
        string Sentence;
        if ( words.Count > 0)
        {
            Sentence = words[0];

            info.gameObject.SetActive(true);
            info.GetComponent<TextSetting>().Show(Sentence);

            words.Remove(Sentence);
        }
    }

    private void Shading()
    {

        // 获取Renderer组件上的所有材质球
        Material[] materials = RayDetection.originalMaterials;

        // 遍历每个材质球
        for (int i = 0; i < materials.Length; i++)
        {
            // 随机选择一个颜色
            Color randomColor = new Color(Random.value, Random.value, Random.value, 1f);

            // 设置材质球的Base Map颜色
            materials[i].SetColor("_BaseColor", randomColor);
        }
    }

    public void StartShading()
    {
        StartCoroutine(Shad());
    }
}
