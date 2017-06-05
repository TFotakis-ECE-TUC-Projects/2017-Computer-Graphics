using System;
using UnityEngine;
namespace Assets.Scripts{
	public class PinController: MonoBehaviour {
		public bool IsDown() {
			return Math.Abs(transform.localRotation.eulerAngles.x-270) > 2;
		}
	}
}