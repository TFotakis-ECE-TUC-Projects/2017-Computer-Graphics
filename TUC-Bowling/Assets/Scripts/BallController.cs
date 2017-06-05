using UnityEngine;
namespace Assets.Scripts{
	public class BallController: MonoBehaviour{
		private bool thrown, rolled, striked;
		public int Force;
		private Camera fpsCam;
		private GameObject followingCam;
		public Vector3 FollowingCamOffset;
		public Vector3 FpsCamOffset;
		public AllayController allay;
		public Collider border;
		public bool isHuman;
		public AudioClip strike, ballRolling;
		private void Awake(){
			fpsCam = GameObject.Find("FPSCamera").GetComponent<Camera>();
			followingCam = GameObject.Find("FollowingCamera");
			followingCam.GetComponent<Camera>().enabled = true;
			followingCam.SetActive(false);
		}
		private void Start(){
			thrown = false;
			rolled = false;
			striked = false;
//			gameObject.SetActive(false);
			GetComponent<MeshRenderer>().enabled = false;
			border = GameObject.Find("Border").GetComponent<BoxCollider>();
		}
		private void Update(){
			if(!(Input.GetButtonDown("Throw") && isHuman)) return;
			if(thrown) Recover();
			else Throw();
		}
		public void Throw(){
			transform.parent = null;
			GetComponent<Collider>().enabled = true;
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward.normalized * Force);
			fpsCam.enabled = false;
			followingCam.SetActive(true);
			thrown = true;
			border.enabled = false;
		}
		public void Recover(){
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			GetComponent<Collider>().enabled = false;
			transform.parent = fpsCam.transform;
			transform.localPosition = FpsCamOffset;
			followingCam.SetActive(false);
			fpsCam.enabled = true;
			thrown = false;
			rolled = false;
			striked = false;
			border.enabled = true;
			allay.ContinueGame();
		}
		private void LateUpdate(){
			if(!thrown) return;
			if(transform.position.x > 11.1) transform.position = new Vector3(transform.position.x, transform.position.y, 11.1f);
			else if(transform.position.x < -11.1) transform.position = new Vector3(transform.position.x, transform.position.y, -11.1f);
			if(transform.position.y > 7.85) transform.position = new Vector3(transform.position.x, transform.position.y, 7.8f);
			else if(transform.position.y < -0.3) transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
			if(transform.position.z > 24.9) transform.position = new Vector3(transform.position.x, transform.position.y, 24.9f);
			else if(transform.position.z < -13.3) transform.position = new Vector3(transform.position.x, transform.position.y, -13.3f);
			followingCam.transform.position = transform.position + FollowingCamOffset;
		}
		private void OnCollisionEnter(Collision collision){
			if(collision.transform.gameObject.tag == "Allay" && !rolled){
				rolled = true;
				GetComponent<AudioSource>().clip = ballRolling;
				GetComponent<AudioSource>().Play();
			} else if(collision.transform.gameObject.tag == "Pin" && !striked){
				striked = true;
				GetComponent<AudioSource>().clip = strike;
				GetComponent<AudioSource>().Play();
			}
		}
	}
}