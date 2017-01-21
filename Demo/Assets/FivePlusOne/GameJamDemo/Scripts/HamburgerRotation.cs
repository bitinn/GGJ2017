
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FivePlusOne.GameJamDemo {

	public class HamburgerRotation : MonoBehaviour {
		// rotation speed in degrees
		[SerializeField]
		float _angularSpeed = 5f;

		/*
			on game loop
		*/

		void Update () {
			transform.Rotate(Vector3.up, _angularSpeed * Time.deltaTime, Space.World);
		}
	}
}
