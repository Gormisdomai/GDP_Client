using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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


		private void Update()
		{
			
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
