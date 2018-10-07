using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressRing : MonoBehaviour {
	[SerializeField]
	private TextMeshProUGUI _timer;
	[SerializeField]
	private Image _ring_over;
	// Use this for initialization
	void Awake () {
		_ring_over.fillAmount = 0;
	}
	
	void Update () {}

	public void updateDisplay(string text,float percent) {
		_timer.text = text;
		_ring_over.fillAmount = percent;
	}
}
