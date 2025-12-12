using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    MeshRenderer mr;
    [SerializeField] GameObject endPoint;

    Material[] mr_materials;
    [SerializeField] Material[] materials;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        endPoint.SetActive(false);
        mr_materials = mr.materials;
    }

    public void OpenPortal()
    {
        endPoint.SetActive(true);
        mr_materials[0] = materials[1];
    }

    public void ClosePortal()
    {
        endPoint.SetActive(false);
        mr_materials[0] = materials[0];
    }
}
