﻿using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderEffect_CorruptedVram : MonoBehaviour {

	public float shift = 10;
	private Texture texture;
	private Material material;
	[SerializeField] Shader shader;

	void Awake ()
	{
		material = new Material(shader);
		texture = Resources.Load<Texture>("Checkerboard-big");
	}
		
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_ValueX", shift);
		material.SetTexture("_Texture", texture);
		Graphics.Blit (source, destination, material);
	}
}
