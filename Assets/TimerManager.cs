﻿using System;
using System.Timers;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour {
	public ProgaressBar progress;
	
	[SerializeField] private float _countdown;
	[SerializeField] private TextMeshProUGUI _uiMinutes;
	[SerializeField] private TextMeshProUGUI _uiSeconds;
	
	private bool _enableTimer;
	private AudioClip _soundPotato;
	private AudioSource _audioSource;
	
	private void Awake() {
		_enableTimer = false;
		_audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void StartTimer(int second) {
		_audioSource.Stop();
		_countdown = second;
		_enableTimer = true;
	}

	// タイマーの文字更新
	void updateTimerDisplay() {
		if (_countdown > 0)
			_uiMinutes.text = Convert.ToString((int) Mathf.Floor(_countdown/60));
		else
			_uiMinutes.text = "0";
		// 一桁の場合,左を0で埋める.
		_uiSeconds.text = Convert.ToString((int) _countdown % 60).PadLeft(2,'0');
	}
	
	// Update is called once per frame
	void Update () {
		//タイマーが無効の場合
		if (!_enableTimer)
			return;

		_countdown -= Time.deltaTime;
		updateTimerDisplay();
		
		if (_countdown <= 0)
			complate();
	}

	void complate() {
		_enableTimer = false;
		_audioSource.clip = _soundPotato;
		_audioSource.Play ();
	}
}
