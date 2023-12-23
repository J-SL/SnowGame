using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] TMP_Text info;
    [SerializeField] private GameObject HP;
    [SerializeField] private FireRange SubHPMethod;

    public GameObject[] prefabs; // 预制体数组
    public float minX; // 平面的最小x坐标
    public float maxX; // 平面的最大x坐标
    public float minZ; // 平面的最小z坐标
    public float maxZ; // 平面的最大z坐标
    public int numPrefabs; // 预制体的数量

    private void Start()
    {
        HP.GetComponent<Image>().material.SetFloat("_value", 1);
        GeneratePrefabs();
    }
    public Coroutine coroutine;
    private void GeneratePrefabs()
    {
        for (int i = 0; i < numPrefabs; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)]; // 随机选择一个预制体
            Vector3 position = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ)); // 在平面上随机生成位置

            Instantiate(prefab, position, Quaternion.Euler(-90,0,0)); // 创建预制体实例
        }
        info.gameObject.SetActive(true);
        info.GetComponent<TextSetting>().Show("<color=red>Hello ^_^</color>\n" +
            "<shake>这天气太冷了</shake>\n" +
            "<color=red>先找个火堆烤烤火吧</color>");
        coroutine = SubHPMethod.StartCoroutine(SubHP());
    }
    [HideInInspector] public bool Isstop;
    IEnumerator SubHP()
    {
        while(HP.GetComponent<Image>().material.GetFloat("_value")>0)
        {
            var hp = HP.GetComponent<Image>().material.GetFloat("_value") - 0.001f;
            HP.GetComponent<Image>().material.SetFloat("_value", hp);
            yield return new WaitForSeconds(0.5f);
            if (Isstop) yield break;
        }      
    }
}
