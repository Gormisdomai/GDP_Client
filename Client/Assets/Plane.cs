using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
	public class Plane : MonoBehaviour
	{
		[SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
		private Rigidbody2D m_Rigidbody2D;

		private void Awake()
		{
				m_Rigidbody2D = GetComponent<Rigidbody2D>();
		}


		private void FixedUpdate()
		{


		}


		public void Move(float x, float move, bool flap)
		{
			if (!flap) {
				m_Rigidbody2D.velocity = new Vector2 (x, move * m_MaxSpeed);
			} else {
				if (m_Rigidbody2D.velocity.y < 0) {
					m_Rigidbody2D.AddForce( new Vector2(0, 10*((move>0) ? 1 : 0)), ForceMode2D.Impulse);
				}
			}
			Vector2 vel = m_Rigidbody2D.velocity + (new Vector2(GameObject.Find ("SpikeSpawner").GetComponent <SpikeSpawner>().speed, 0));
			if (vel != Vector2.zero)
			{
				float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle - 60, Vector3.forward);
			}

		}

	}
}
