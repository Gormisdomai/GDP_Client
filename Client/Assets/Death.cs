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
		Text text = (Text) Object.Instantiate(DeathText,Vector3.zero,Quaternion.identity);
		Button menu = (Button) Object.Instantiate(MenuButton,Vector3.zero,Quaternion.identity);
		Button again = (Button) Object.Instantiate(TryAgainButton,Vector3.zero,Quaternion.identity);
		Transform canvas = GameObject.Find("Canvas").transform;
		text.transform.parent = canvas;
		menu.transform.parent = canvas;
		again.transform.parent = canvas;
		text.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		text.transform.localPosition = new Vector3(0.0f,150.0f,0.0f);
		menu.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		menu.transform.localPosition = new Vector3(-300.0f,-150.0f,0.0f);
		again.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		again.transform.localPosition = new Vector3(300.0f,-150.0f,0.0f);
		menu.onClick.AddListener(toMenu);
		again.onClick.AddListener(reload);
	}
}
