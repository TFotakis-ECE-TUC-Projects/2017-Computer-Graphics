using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
namespace Assets.Scripts{
	public class HumanPlayer: Player{
		public HumanPlayer(): base(){
		
		}
		public override void Play(){
			fpsController.GetComponent<FirstPersonController>().enabled = true;
			fpsController.GetComponent<AINavigation>().enabled = false;
			fpsController.GetComponent<NavMeshAgent>().enabled = false;
			ballController.isHuman = true;
		}
	}
}