using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDetection : MonoBehaviour
{
    [SerializeField] ShaderObjects shaderObjects;
    private void Start()
    {
        shaderObjects.OverShading.AddListener(Recover);
    }



    [SerializeField] private float length;
    public static GameObject currentTarget, lastTarget;
    [SerializeField] GameObject ShaderBtn;
    void RayCast()
    {
        RaycastHit info;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(
            new Vector3(Screen.width, Screen.height) * 0.5f),
            out info, length, LayerMask.GetMask("Interactive")))
        {
            ShaderBtn.SetActive(true);
            currentTarget = info.collider.gameObject;
            if (lastTarget != null)
            {
                Recover();
                lastTarget = null;
            }
            if (currentTarget != null)
            {
                Highlight();
                lastTarget = currentTarget;
            }
        }
        else
        {
            ShaderBtn.SetActive(false);
            currentTarget = null;
            if (lastTarget)
                Recover();
        }
    }

    [SerializeField] detectBtn isDetect;
    private void Update()
    {
        if (isDetect.IsDetect)
        {
            RayCast();
        }
    }

    [SerializeField] Material highlightMaterial;
    public static Material[] originalMaterials;
    private void Highlight()
    {
        MeshRenderer meshRenderer = currentTarget.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterials = meshRenderer.materials;
            Material[] materials = new Material[originalMaterials.Length];
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = highlightMaterial;
            }
            meshRenderer.materials = materials;
        }
    }

    public void Recover()
    {
        MeshRenderer meshRenderer = lastTarget.GetComponent<MeshRenderer>();
        if (meshRenderer != null && originalMaterials != null)
        {
            meshRenderer.materials = originalMaterials;
            originalMaterials = null;
        }
    }
}
