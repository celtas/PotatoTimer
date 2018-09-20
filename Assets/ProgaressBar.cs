using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProgaressBar : MonoBehaviour {
	RectTransform rectTran;
	private Vector3 init_pos;
	private Vector3 target_pos;
	private RectTransform _rectTransform;
	
	void Awake () {
		_rectTransform = gameObject.GetComponent<RectTransform>();
		init_pos = new Vector3(-14.65f,0,0);
		_rectTransform.position = init_pos;
	}

	public void start(int time) {
		_rectTransform.position = init_pos;
		_rectTransform.DOMoveX (
			-4.28f,　　//移動後の座標
			time　　　　//時間
		);
	}
}
