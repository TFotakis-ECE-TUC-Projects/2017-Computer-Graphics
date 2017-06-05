using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
namespace Assets.Scripts{
	public class AllayController: MonoBehaviour{
		private readonly float[] allaysXPos = {-10.0f, -8.0f, -5.5f, -3.5f, -1.0f, 1.0f, 3.5f, 5.5f, 8.0f, 10.0f};
		private readonly float _pinYPos = 0.22f;
		private readonly float _pinZPos = 23.5f;
		private const float PlayerYPos = 1;
		private const float PlayerZPos = -5;
		private float allayLeftSide, allayRightSide;
		public int AllayNumber;
		public bool IsTaken;
		public Player[] players;
		private int physicalPlayersNum;
		private GameObject ball;
		private Text tv;
		private int frame, chance, playerTurn;
		private Transform PinSetPrefab;
		private GameObject pinSet;
		private int[][] scores;
		private ReceptionController reception;
		private GameObject fpsController;
		private Light allaySpotlight;
		private void Awake(){
			ball = GameObject.FindWithTag("Ball");
			fpsController = GameObject.Find("FPSController");
		}
		private void Start(){
			tv = GameObject.Find("TV (" + AllayNumber + ")").GetComponentInChildren<Text>();
			frame = 1;
			PinSetPrefab = ((GameObject) Resources.Load("Prefabs/PinSet", typeof(GameObject))).transform;
			reception = GameObject.Find("Reception").GetComponent<ReceptionController>();
			allaySpotlight = GameObject.Find("AllaySpotlight (" + AllayNumber + ")").GetComponent<Light>();
			allaySpotlight.enabled = false;
		}
		public void StartGame(int physicalPlayersNumber, int aiPlayersNumber){
			physicalPlayersNum = physicalPlayersNumber;
			players = new Player[physicalPlayersNumber + aiPlayersNumber];
			for(var i = 0; i < physicalPlayersNumber; i++){
				players[i] = new HumanPlayer();
			}
			for(var i = physicalPlayersNumber; i < physicalPlayersNumber + aiPlayersNumber; i++){
				players[i] = new AIPlayer();
			}
			frame = 0;
			chance = 0;
			playerTurn = 0;
			ball.SetActive(true);
			ball.GetComponent<MeshRenderer>().enabled = true;
			ball.GetComponent<BallController>().allay = this;
			ScoreUpdate();
			pinSet = Instantiate(PinSetPrefab, new Vector3(allaysXPos[AllayNumber - 1], _pinYPos, _pinZPos), Quaternion.identity).gameObject;
			allaySpotlight.enabled = true;
			players[0].Play();
		}
		private void ScoreUpdate(){
			tv.text = "Frame: " + (frame + 1) + "\n";
			tv.text += players[playerTurn].GetType() == typeof(HumanPlayer) ? "Human Player " + (playerTurn + 1) : "AI Player " + (playerTurn + 1 - physicalPlayersNum);
			tv.text += " turn, Chance: " + (chance + 1) + "\nScoreboard\n";
			for(var i = 0; i < players.Length; i++){
				tv.text += players[i].GetType() == typeof(HumanPlayer) ? "Human Player " + (i + 1) : "AI Player " + (i + 1 - physicalPlayersNum);
				tv.text += ": " + players[i].TotalScore() + "\n";
			}
		}
		public void ContinueGame(){
			var score = pinSet.GetComponent<PinSetController>().NumberOfFallenPins();
			players[playerTurn].SetScore(frame, chance, score);
			if(frame == 9 && (score == 10 || players[playerTurn].frameScore[9].TotalScore() == 10)){
				Destroy(pinSet);
				pinSet = Instantiate(PinSetPrefab, new Vector3(allaysXPos[AllayNumber - 1], _pinYPos, _pinZPos), Quaternion.identity).gameObject;
				frame = 10;
				chance = 0;
				players[playerTurn].Play();
			} else if(frame == 10 && players[playerTurn].frameScore[9].TotalScore() != 10){
				Destroy(pinSet);
				pinSet = Instantiate(PinSetPrefab, new Vector3(allaysXPos[AllayNumber - 1], _pinYPos, _pinZPos), Quaternion.identity).gameObject;
				frame = 11;
				chance = 0;
				players[playerTurn].Play();
			}else if(frame==10 || frame==11){
				frame = 9;
				Destroy(pinSet);
				if(playerTurn + 1 >= players.Length){
					frame++;
					playerTurn = 0;
				} else{
					playerTurn++;
				}
				if(frame < 10){
					pinSet = Instantiate(PinSetPrefab, new Vector3(allaysXPos[AllayNumber - 1], _pinYPos, _pinZPos), Quaternion.identity).gameObject;
					players[playerTurn].Play();
				} else{
					GameEnded();
					return;
				}
			} else if((chance == 0 && score == 10) || chance == 1){
				chance = 0;
				Destroy(pinSet);
				if(playerTurn + 1 >= players.Length){
					frame++;
					playerTurn = 0;
				} else{
					playerTurn++;
				}
				if(frame < 10){
					pinSet = Instantiate(PinSetPrefab, new Vector3(allaysXPos[AllayNumber - 1], _pinYPos, _pinZPos), Quaternion.identity).gameObject;
					players[playerTurn].Play();
				} else{
					GameEnded();
					return;
				}
			} else{
				pinSet.GetComponent<PinSetController>().DeactivateFallenPins();
				chance = 1;
				players[playerTurn].Play();
			}
			ScoreUpdate();
			ResetPlayersPosition();
		}
		public void GameEnded(){
			ball.GetComponent<MeshRenderer>().enabled = false;
			reception.State = ReceptionController.ReceptionState.Idle;
			tv.text = "Game Ended\n";
			var max = players.Select(player => player.TotalScore()).Concat(new[]{-1}).Max();
			foreach(var player in players){
				if(player.TotalScore() != max) continue;
				tv.text += player.GetType() == typeof(HumanPlayer) ? "Human Player " + (playerTurn + 1) : "AI Player " + (playerTurn + 1 - physicalPlayersNum);
				tv.text += " Wins\n";
				break;
			}
			tv.text += "Scoreboard\n";
			for(var i = 0; i < players.Length; i++){
				tv.text += players[i].GetType() == typeof(HumanPlayer) ? "Human Player " + (i + 1) : "AI Player " + (i + 1 - physicalPlayersNum);
				tv.text += ": " + players[i].TotalScore() + "\n";
			}
			fpsController.GetComponent<FirstPersonController>().enabled = true;
			fpsController.GetComponent<AINavigation>().enabled = false;
			fpsController.GetComponent<NavMeshAgent>().enabled = false;
			allaySpotlight.enabled = false;
		}
		public void ResetPlayersPosition(){
			fpsController.transform.position = new Vector3(Random.Range(allaysXPos[AllayNumber - 1] - 0.5f, allaysXPos[AllayNumber - 1] + 0.5f), PlayerYPos, PlayerZPos);
		}
	}
}