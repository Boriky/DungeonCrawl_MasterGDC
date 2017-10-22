﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionIntermittence : MonoBehaviour
{
    [Header("Gameplay values")]
    [SerializeField] float m_lowestEmission = 0.0f;
    [SerializeField] float m_maxEmission = 0.5f;
    [SerializeField] float m_intermittenceVelocity = 0.1f;

    Texture2D emissionTexture = null;

    private void Start()
    {
        MeshRenderer m_meshRenderer = GetComponent<MeshRenderer>();
        Material material = m_meshRenderer.material;
        emissionTexture = (Texture2D)Instantiate(material.GetTexture("_Illum"));
        m_meshRenderer.material.SetTexture("_Illum", emissionTexture);
    }

	// Update is called once per frame
	void Update ()
    {
        MeshRenderer m_meshRenderer = GetComponent<MeshRenderer>();
        Material material = m_meshRenderer.material;
        float emission = m_lowestEmission + Mathf.PingPong(Time.time * m_intermittenceVelocity, m_maxEmission);
        
        /*Color baseColor = material.color;
        Color finalColor = baseColor * Mathf.LinearToGammaSpace(emission);
        material.SetColor("_Emission", finalColor);*/

        Color[] colorArray = emissionTexture.GetPixels();

        for (int index = 0; index < colorArray.Length; ++index)
        {
            Color color = colorArray[index];
            colorArray[index] = new Color(0, 0, 0, emission); 
        }

        emissionTexture.SetPixels(colorArray);
        emissionTexture.Apply();          
	}
}
