using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionEffect : MonoBehaviour {

    MeshRenderer m_meshRenderer = null;

	// Use this for initialization
	void Start ()
    {
        MeshRenderer m_meshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        MeshRenderer m_meshRenderer = GetComponent<MeshRenderer>();
        Material material = m_meshRenderer.material;
        material.color = new Color(Random.Range(0.1f, 0.3f), Random.Range(0.1f, 0.3f), 1.0f);
        m_meshRenderer.material = material;
	}
}
