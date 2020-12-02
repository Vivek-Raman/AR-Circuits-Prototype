using System;
using UnityEngine;

public class ManageLightState : MonoBehaviour
{
    [SerializeField] private Material unlitMaterial;
    [SerializeField] private Material litMaterial;
    private Renderer meshRenderer = null;

    private void Awake()
    {
        meshRenderer = this.GetComponent<Renderer>();
    }
    public void EvaluateState(bool isLit)
    {
        if (isLit)
        {
            var x = meshRenderer.sharedMaterials;
            x[1] = litMaterial;
            meshRenderer.sharedMaterials = x;
        }
        else
        {
            var x = meshRenderer.sharedMaterials;
            x[1] = unlitMaterial;
            meshRenderer.sharedMaterials = x;
        }
    }
}
