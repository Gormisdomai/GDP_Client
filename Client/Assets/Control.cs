using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
	[RequireComponent(typeof (PlatformerCharacter2D))]
	public class Control : MonoBehaviour
	{
		private Plane m_Character;
		public bool flappy = false;

		private void Awake()
		{
			m_Character = GetComponent<Plane>();
		}

		void OnTriggerEnter2D(Collider2D col){
			if (col.gameObject.tag == "Hazard") {
				col.gameObject.GetComponent<SpriteRenderer> ().color = Color.blue;
				transform.Rotate (Vector3.forward * -10);
				GetComponent<Death>().PlayerCrashes();
			}
		}

		void OnTriggerExit2D(Collider2D col){
			if (col.gameObject.tag == "Hazard") {
				col.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				transform.Rotate (Vector3.forward * +10);
			}
		}

		private void FixedUpdate()
		{
			float v = CrossPlatformInputManager.GetAxis("Vertical");
			// Pass all parameters to the character control script.
			m_Character.Move(v, flappy);
		}
	}
}
