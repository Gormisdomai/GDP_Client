using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {
	public float score = 0;
	public float scoreSF;
	Text text;
	Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Character").GetComponent<Transform>();
		text = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		score = score + Time.deltaTime * scoreSF/(8 - player.localPosition.x);
		text.text = "Score: " + ((int)score).ToString();
	}
}
