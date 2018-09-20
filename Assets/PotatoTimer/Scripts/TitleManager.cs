using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour {
	public static TitleManager instance;
	
	public GameObject[] sprites;
	public Image fadeout;

	void Start () {
		//入力イベントの登録
		InputManager.inputEvent += new InputEventHandler(onInput);
	}
	
	void Update () {
	}

	void onInput(object sender, InputEventArgs e) {
		if (e.State != InputState.SINGLE_TOUCH)
			return;

		foreach (GameObject sprite in sprites) {
			sprite.SetActive(false);
		}

		FrechflySpawner.instance.gameObject.SetActive(true);
	}

	public void toggleScene() {
		DOTween.ToAlpha(
			() => fadeout.color,
			color => fadeout.color = color,
			1f,
			1.5f
		);
		Invoke("nextScene", 1.5f);
	}

	void nextScene() {
		SceneManager.LoadScene("Main");
	}

	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}
