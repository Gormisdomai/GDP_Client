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
		bool contact = false;
		bool wasContact = false;
		SpikeSpawner spikes;

		private void Start() {
			spikes = GameObject.Find("SpikeSpawner").GetComponent<SpikeSpawner>();
		}

		private void Awake()
		{
			m_Character = GetComponent<Plane>();
		}

		void OnTriggerEnter2D(Collider2D col){
		}

		void OnTriggerStay2D(Collider2D col) {
			contact = true;
			wasContact = true;
			//transform.Rotate (Vector3.forward * -10);
			if (col.gameObject.tag == "Hazard Top") {
				spikes.colorTop();
			}
			//GetComponent<Death>().PlayerCrashes();
			if (col.gameObject.tag == "Hazard Bottom") {
				spikes.colorBottom();
			}
		}

		/*void OnTriggerExit2D(Collider2D col){
			if (col.gameObject.tag == "Hazard Top" || col.gameObject.tag == "Hazard Bottom") {
				GameObject.Find("SpikeSpawner").GetComponent<SpikeSpawner>().decolorTop();
				//col.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				//transform.Rotate (Vector3.forward * +10);
			}
		}*/

		private void FixedUpdate()
		{
			if (wasContact && !contact) {
				wasContact = false;
				spikes.decolorTop();
				spikes.decolorBottom();
			}
			float v = CrossPlatformInputManager.GetAxis("Vertical");
			// Pass all parameters to the character control script.
			m_Character.Move(v, flappy);
			contact = false;
		}
	}
}
