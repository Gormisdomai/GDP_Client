using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

	public Text text;

	public void backToMain() {
		SceneManager.LoadScene("MainMenu");
		//SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameScene));
		//SceneManager.LoadScene(gameScene);
	}

	// Use this for initialization
	void Start () {
		string scores = "High Scores:\n";
		for (int i = 0; i<5; i++){
			int score = PlayerPrefs.GetInt("score" + i.ToString());
			if (score != 0) scores += score.ToString() + "\n";
		}
		text.text = scores;
	}

	private void Update() {
	}

}
