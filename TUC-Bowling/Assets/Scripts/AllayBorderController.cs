using UnityEngine;
public class AllayBorderController : MonoBehaviour {
	private void OnCollisionEnter(Collision collision) {
		if(collision.transform.tag == "Ball") {
			GetComponent<Collider>().enabled = false;
		}
	}
	private void OnCollisionExit(Collision collision) {
		if(collision.transform.tag == "Ball") {
			GetComponent<Collider>().enabled = true;
		}
	}
}
