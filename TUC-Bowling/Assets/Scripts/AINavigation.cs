using System.Collections;
using UnityEngine;
using UnityEngine.AI;
namespace Assets.Scripts{
	public class AINavigation: MonoBehaviour{
		private readonly float[] allaysXPos = {-10.0f, -8.0f, -5.5f, -3.5f, -1.0f, 1.0f, 3.5f, 5.5f, 8.0f, 10.0f};
		public int allayNumber;
		private BallController ballController;
		void Start(){
			ballController = GameObject.FindWithTag("Ball").GetComponent<BallController>();
		}
		public void Play(){
			StartCoroutine(ThrowAndRecover());
		}
		private IEnumerator ThrowAndRecover(){
			GetComponent<NavMeshAgent>().SetDestination(new Vector3(Random.Range(allaysXPos[allayNumber] - 0.4f, allaysXPos[allayNumber] + 0.4f), 0, 0));
			yield return new WaitForSeconds(5);
			transform.Rotate(new Vector3(0,-transform.rotation.eulerAngles.y,0));
			ballController.Throw();
			yield return new WaitForSeconds(5);
			ballController.Recover();
		}
	}
}