using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreUpdate : MonoBehaviour {
	public float score;
	Text text;

	// Use this for initialization
	void Start () {
		score = 0;
		text = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		score = score + (Time.deltaTime);
		text.text = "Score: " + ((int)score).ToString();
	}
}
