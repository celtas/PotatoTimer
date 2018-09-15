using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FlashMenu : MonoBehaviour {
	private SpriteRenderer sprite_menu;
	[SerializeField, Range(0, 10)]
	public float display_cycle;

	private float countTime = -1;
	private float alpha = 1;

	// Use this for initialization
	void Start () {
		sprite_menu = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// 文字を点滅
	void Update () {
		countTime += Time.deltaTime * display_cycle;

		if (countTime > 1)
			countTime = -1;

		if (countTime < 0)
			alpha = -1 * countTime;
		else
			alpha = countTime;
		
		sprite_menu.color = new Color(1.0f, 1.0f, 1.0f, alpha);
	}
}
