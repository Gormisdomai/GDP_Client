using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
	[RequireComponent(typeof (PlatformerCharacter2D))]
	public class Control : MonoBehaviour
	{
		private Plane m_Character;

		private void Awake()
		{
			m_Character = GetComponent<Plane>();
		}

		// checks on exit if new score should be added to highscores
		void RegisterScore() {
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

		private void Update()
		{
			// temporary controls, no point adding ui until scale of other elements is set

			// back to menu
			if (Input.GetKeyDown("backspace")) {
				// maybe shouldnt register score here in future
				RegisterScore();

				Time.timeScale = 1.0f;
				SceneManager.LoadScene("MainMenu");
			}

			// pause
			if (Input.GetKeyDown("escape")) {
        		if (Time.timeScale == 1.0)
            		Time.timeScale = 0.0f;
        		else
            		Time.timeScale = 1.0f;
			}
		}


		private void FixedUpdate()
		{
			// Read the inputs.
			bool crouch = Input.GetKey(KeyCode.LeftControl);
			float v = CrossPlatformInputManager.GetAxis("Vertical");
			// Pass all parameters to the character control script.
			m_Character.Move(v, false, false);
		}
	}
}
