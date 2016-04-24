using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour {

	public Button MenuButton;
	public Button TryAgainButton;
	public Text DeathText;

	// checks on exit if new score should be added to highscores
	public void RegisterScore() {
		GameObject scoreDisplay = GameObject.Find("ScoreDisplay");
    	ScoreUpdate script = scoreDisplay.GetComponent<ScoreUpdate>();
    	int newScore = (int) script.score;
		int i = 0;
		do {
			if (PlayerPrefs.GetInt("score" + i.ToString()) < newScore) break; // new score belongs at position i
			i ++;
		} while (i < 5);
		for (int j = 3; j >= i; j--) {
			PlayerPrefs.SetInt("score" + (j+1).ToString(), PlayerPrefs.GetInt("score" + j.ToString()));
		}
		if (i != 5) PlayerPrefs.SetInt("score" + i.ToString(), newScore);
	}

	void reload() {
		SceneManager.LoadScene("Test_Scene");
		Time.timeScale = 1.0f;
	}

	void toMenu() {
		SceneManager.LoadScene("MainMenu");
		Time.timeScale = 1.0f;
	}

	public void PlayerCrashes() {
		RegisterScore();
		Time.timeScale = 0.0f;
		Button Menu = (Button) Object.Instantiate(MenuButton,new Vector3(-3.5f,-1.5f,0.0f),Quaternion.identity);
		Button Again = (Button) Object.Instantiate(TryAgainButton,new Vector3(3.5f,-1.5f,0.0f),Quaternion.identity);
		Text text = (Text) Object.Instantiate(DeathText,new Vector3(0.0f,1.5f,0.0f),Quaternion.identity);
		text.transform.parent = GameObject.Find("Canvas").transform;
		Menu.transform.parent = GameObject.Find("Canvas").transform;
		Again.transform.parent = GameObject.Find("Canvas").transform;
		Menu.onClick.AddListener(toMenu);
		Again.onClick.AddListener(reload);
	}
}
