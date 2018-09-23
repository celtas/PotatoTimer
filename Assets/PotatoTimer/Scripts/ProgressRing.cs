using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressRing : MonoBehaviour {
	private float _initSeconds = 0;
	[SerializeField]
	private TextMeshProUGUI _timer;
	[SerializeField]
	private Image _ring,_ring_over;

	// Use this for initialization
	void Awake () {
		_timer.text = "0";
		_ring_over.fillAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateDisplay(float second,float percent) {
		_timer.text = Convert.ToString(Mathf.Ceil(second));
		_ring_over.fillAmount = percent;
	}
}
