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
		Rigidbody2D phys;
		Transform tran;

		// scale factors to do with scoring
		public float wallPen; // backward speed when touching walls
		public float forwardSF; // speed at origin
		float speedx;

		private void Start() {
			tran = GetComponent<Transform>();
			//phys = GetComponent<Rigidbody2D>();
			//wallPenVec = new Vector2(-wallPenalty,0);
			spikes = GameObject.Find("SpikeSpawner").GetComponent<SpikeSpawner>();
			setSpeedx();
		}

		private void Awake()
		{
			m_Character = GetComponent<Plane>();
		}

		void OnTriggerEnter2D(Collider2D col) {
			if (col.gameObject.tag == "Death Edge") {
				GetComponent<Death>().PlayerCrashes();
			}
		}

		void OnTriggerStay2D(Collider2D col) {
			//score stuff
			if (!wasContact) {
			//	phys.velocity = wallPenVec;
				speedx = -wallPen;
				//print(phys.velocity.x);
			}
			contact = true;
			wasContact = true;
			if (col.gameObject.tag == "Hazard Top") {
				spikes.colorTop();
			}
			if (col.gameObject.tag == "Hazard Bottom") {
				spikes.colorBottom();
			}
		}

		void setSpeedx() {
			speedx = (8 - tran.localPosition.x) * forwardSF/8; // 8 is max position on right
		}

		private void FixedUpdate()
		{
			if (!contact) {
				setSpeedx();
				if (wasContact) { // bit inefficient
					wasContact = false;
					spikes.decolorTop();
					spikes.decolorBottom();
				}
			}
			float v = CrossPlatformInputManager.GetAxis("Vertical");
			// Pass all parameters to the character control script.
			m_Character.Move(speedx, v, flappy); ///////////////////////////////////////////
			contact = false;
		}
	}
}
