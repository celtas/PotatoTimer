using System;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour {
	public ProgaressBar progress;
	[SerializeField] private float _nowTime;
	[SerializeField, Range(0, 600)] private int _initTime;
	
	[SerializeField] private float _countTime;
	[SerializeField] private TextMeshProUGUI _mTimer;
	[SerializeField] private TextMeshProUGUI _sTimer;

	public GameObject[] backgrounds;
	public GameObject[] canvass;

	private bool _isTimerActive;
	
	public AudioClip ac_alert;
	private AudioSource audioSource;
	
	private void Awake() {
		_nowTime = 0;	//initialize
		_isTimerActive = false;
		audioSource = gameObject.GetComponent<AudioSource>();
		initScene();
	}

	public void StartTimer(int second) {
		nextScene();
		_initTime = second;
		_nowTime = 0;
		_isTimerActive = true;
		progress.start(second);
		audioSource.Stop();
	}

	void nextScene() {
		foreach (GameObject obj in backgrounds) {
			obj.SetActive(true);
		}
		foreach (GameObject obj in canvass) {
			obj.SetActive(false);
		}
	}

	void initScene() {
		foreach (GameObject obj in backgrounds) {
			obj.SetActive(false);
		}
		foreach (GameObject obj in canvass) {
			obj.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!_isTimerActive) {
			_mTimer.text = _countTime.ToString();
			return;
		}

		_nowTime += Time.deltaTime;
		_countTime = _initTime - (int)Mathf.Floor(_nowTime);
		
		_mTimer.text = Convert.ToString(Mathf.Floor(_countTime/60));
		float s = _countTime % 60;
		_sTimer.text = Convert.ToString(s);
		
		//タイマーカウントしたら
		if (_countTime <= 0) {
			alert();
			_isTimerActive = false;
		}
	}

	void alert() {
		audioSource.clip = ac_alert;
		audioSource.Play ();
	}
}
