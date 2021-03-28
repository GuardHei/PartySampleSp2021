using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioClip bgm;
	public AudioSource source;

	public void Start() {
		
		DontDestroyOnLoad(this);
		
		if (bgm != null) {
			source.clip = bgm;
			source.loop = true;
			source.Play();
		}
	}
}
