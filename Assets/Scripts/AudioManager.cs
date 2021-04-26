﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

	public AudioClip bgm;
	public AudioClip bgm2;
	public AudioClip bossBgm;
	public AudioSource source;

	public void Start() {
		
		DontDestroyOnLoad(this);
		
		if (bgm != null) {
			source.clip = bgm;
			source.loop = true;
			source.Play();
		}

		if (bgm2 != null)
		{
			SceneManager.sceneLoaded += (scene, mode) => {
				if (scene.name.Contains("WCScene1"))
				{
					source.clip = bgm2;
					source.loop = true;
					source.Play();
				}
			};
		}

		if (bossBgm != null) {
			SceneManager.sceneLoaded += (scene, mode) => {
				if (scene.name.Contains("Boss"))
				{
					source.clip = bossBgm;
					source.loop = true;
					source.Play();
				}
			};
		}
	}
}
