using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

	public AudioClip bgm;
	public AudioClip bgm2;
	public AudioClip bossBgm;
	public AudioSource source;
	
	public static int Count => idleAudioSources.Count;

	private static AudioManager instance;
	private static readonly Queue<AudioSource> idleAudioSources = new Queue<AudioSource>(5);
	private static int busyCount;
	
	private void Awake() {
		instance = this;
		DontDestroyOnLoad(this);
		SceneManager.sceneLoaded += (scene, mode) => { Time.timeScale = 1; };
	}

	public void Start() {
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
	
	public static void PlayAtPoint(AudioClip clip, Vector3 position, float volume = 1f) {
		if (clip == null || busyCount > 100) return;
		AudioSource source = GetAudioSource();
		source.transform.position = position;
		source.clip = clip;
		source.volume = volume;
		source.Play();
		instance.StartCoroutine(ExeRecycleCoroutine(source));
	}

	private static AudioSource GetAudioSource() {
		AudioSource source;
		if (idleAudioSources.Count > 0) {
			source = idleAudioSources.Dequeue();
			source.gameObject.SetActive(true);
		} else {
			GameObject gameObject = new GameObject("Public Audio Source");
			gameObject.transform.parent = instance.transform;
			source = gameObject.AddComponent<AudioSource>();
			source.spatialBlend = 1f;
			source.loop = false;
		}

		return source;
	}

	private static IEnumerator ExeRecycleCoroutine(AudioSource source) {
		busyCount++;
		float time = source.clip.length;
		yield return new WaitForSeconds(time);
		source.Stop();
		source.gameObject.SetActive(false);
		idleAudioSources.Enqueue(source);
		busyCount--;
	}
}
