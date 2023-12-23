using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireRange : MonoBehaviour
{
    public ParticleSystem particleSystem;
    [SerializeField] private Button btn;
    [SerializeField] private GameObject HP;
    [SerializeField] ProceduralGeneration stopcoroutine;

    [SerializeField] private GameObject snowflake;
    private void Start()
    {
        particleSystem.Stop(); // ��ʼֹͣ����Ч��
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            //Debug.Log("����");

            if (particleSystem.isPlaying)
            {
                snowflake.SetActive(false);
                if (_SubHPCoroutine != null) StopCoroutine(_SubHPCoroutine);
                _AddHPCoroutine=StartCoroutine(AddHP());
            }         
            else
                btn.gameObject.SetActive(true);

        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            //Debug.Log("��ȥ");
            if (!particleSystem.isPlaying)
                btn.gameObject.SetActive(false);
            snowflake.SetActive(true);

            if (_AddHPCoroutine != null) StopCoroutine(_AddHPCoroutine);
            _SubHPCoroutine=StartCoroutine(SubHP());
        }
    }

    [SerializeField] MusicPlayer player;
    public void StartFire()
    {
        particleSystem.Play();
        player.PlayMusic();
        btn.gameObject.SetActive(false);
        snowflake.SetActive(false);
        if (_SubHPCoroutine != null) StopCoroutine(_SubHPCoroutine);
        _AddHPCoroutine=StartCoroutine(AddHP());
        stopcoroutine.Isstop = true;
    }
    private Coroutine _AddHPCoroutine;
    IEnumerator AddHP()
    {
        while (HP.GetComponent<Image>().material.GetFloat("_value") < 1)
        {
            //Debug.Log("add");
            var hp = HP.GetComponent<Image>().material.GetFloat("_value") + 0.05f;
            HP.GetComponent<Image>().material.SetFloat("_value", hp);
            yield return new WaitForSeconds(0.7f);
        }  
    }
    private Coroutine _SubHPCoroutine;
    IEnumerator SubHP()
    {
        while (HP.GetComponent<Image>().material.GetFloat("_value") > 0)
        {
            //Debug.Log("sub");
            var hp = HP.GetComponent<Image>().material.GetFloat("_value") - 0.004f;
            HP.GetComponent<Image>().material.SetFloat("_value", hp);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
