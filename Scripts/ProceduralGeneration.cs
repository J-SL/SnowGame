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

    public GameObject[] prefabs; // Ԥ��������
    public float minX; // ƽ�����Сx����
    public float maxX; // ƽ������x����
    public float minZ; // ƽ�����Сz����
    public float maxZ; // ƽ������z����
    public int numPrefabs; // Ԥ���������

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
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)]; // ���ѡ��һ��Ԥ����
            Vector3 position = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ)); // ��ƽ�����������λ��

            Instantiate(prefab, position, Quaternion.Euler(-90,0,0)); // ����Ԥ����ʵ��
        }
        info.gameObject.SetActive(true);
        info.GetComponent<TextSetting>().Show("<color=red>Hello ^_^</color>\n" +
            "<shake>������̫����</shake>\n" +
            "<color=red>���Ҹ���ѿ������</color>");
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
