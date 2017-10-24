using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionEffect : MonoBehaviour
{
    MeshRenderer m_meshRenderer = null;

	void Start ()
    {
        MeshRenderer m_meshRenderer = GetComponent<MeshRenderer>();
	}
	
	void Update ()
    {
        ChangeMaterialEmission();
    }

    void ChangeMaterialEmission()
    {
        MeshRenderer m_meshRenderer = GetComponent<MeshRenderer>();
        Material material = m_meshRenderer.material;
        material.color = new Color(Random.Range(0.1f, 0.3f), Random.Range(0.1f, 0.3f), 1.0f);
        m_meshRenderer.material = material;
    }
}
