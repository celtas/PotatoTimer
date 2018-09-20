using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ProgaressBar : MonoBehaviour {
	private Vector3 _startPos,_target_pos;
	private float _endPosX;
	private RectTransform _rect;
	
	void Awake () {
		_rect = gameObject.GetComponent<RectTransform>();
		_startPos = new Vector3(-14.65f,0,0);
		_endPosX = -4.28f;
	}

	public void start(int countdown) {
		_rect.position = _startPos;
		_rect.DOMoveX (
			_endPosX,　　//移動後の座標
			countdown　　　　//時間
		);
	}
}
