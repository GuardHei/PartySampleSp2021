using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraEffectController : MonoBehaviour {

	public Material mat;
	public WarpManager warpManager;
	public bool activated;
	public float borderWidth;
	public Color borderColor;
	public bool autoScroll = true;
	[Range(-1, 1)]
	public float scrollX;
	[Range(-1, 1)]
	public float scrollY;

	private int _activatedKey = Shader.PropertyToID("_Activated");
	private int _scrollKey = Shader.PropertyToID("_Scroll");
	private int _conjunctionKey = Shader.PropertyToID("_Conjunction");
	private int _widthKey = Shader.PropertyToID("_BorderWidth");
	private int _colorKey = Shader.PropertyToID("_BorderColor");

	public void OnRenderImage(RenderTexture src, RenderTexture dest) {
		if (mat != null && warpManager != null) {
			print(warpManager._ScreenPanVC);
			mat.SetFloat(_activatedKey, activated ? 1f : 0f);
			mat.SetVector(_scrollKey, autoScroll ? -warpManager._ScreenPanVC : -new Vector2(scrollX, scrollY));
			mat.SetVector(_conjunctionKey, warpManager._GridOriginVC);
			mat.SetFloat(_widthKey, borderWidth);
			mat.SetColor(_colorKey, borderColor);
			Graphics.Blit(src, dest, mat);
		}
	}
}
