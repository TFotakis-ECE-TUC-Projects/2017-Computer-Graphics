using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
namespace Assets.Scripts{
	public class AIPlayer: Player{
		private readonly AINavigation aiNavigation;
		public AIPlayer(): base(){
			aiNavigation = GameObject.Find("FPSController").GetComponent<AINavigation>();
		}
		public override void Play(){
			fpsController.GetComponent<FirstPersonController>().enabled = false;
			fpsController.GetComponent<AINavigation>().enabled = true;
			fpsController.GetComponent<NavMeshAgent>().enabled = true;
			ballController.isHuman = false;
			aiNavigation.Play();
		}
	}
}