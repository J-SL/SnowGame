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
    public float duration = 15f; // ��ֵ����ʱ��


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
            info.GetComponent<TextSetting>().Show("<rainb><pend>Ŀ���ɣ�лл��Ϸ��</pend></rainb>");
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

        // ��ȡRenderer����ϵ����в�����
        Material[] materials = RayDetection.originalMaterials;

        // ����ÿ��������
        for (int i = 0; i < materials.Length; i++)
        {
            // ���ѡ��һ����ɫ
            Color randomColor = new Color(Random.value, Random.value, Random.value, 1f);

            // ���ò������Base Map��ɫ
            materials[i].SetColor("_BaseColor", randomColor);
        }
    }

    public void StartShading()
    {
        StartCoroutine(Shad());
    }
}
