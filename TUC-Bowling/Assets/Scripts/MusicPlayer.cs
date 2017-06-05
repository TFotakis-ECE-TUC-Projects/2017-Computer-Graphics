using UnityEngine;
namespace Assets.Scripts{
	public class MusicPlayer: MonoBehaviour{
		public AudioSource MusicToPlay;
		private void Start(){
			MusicToPlay.time = Random.Range(0.0f, 2580.0f);
			MusicToPlay.Play();
		}
	}
}