using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
namespace Assets.Scripts{
	public class ReceptionController: MonoBehaviour{
		private AllayController[] allay;
		private GameObject infoText;
		private int physicalPlayersNumber, AIPlayersNumber;
		private const int MaxPlayers = 5;
		public enum ReceptionState{
			Idle,
			PhysicalPlayersSetup,
			AIPlayersSetup,
			GameStarted
		}
		public ReceptionState State;
		private void Awake(){
			infoText = GameObject.Find("InfoBackground");
			InitializeAllays();
		}
		private void Start(){
			physicalPlayersNumber = 0;
			AIPlayersNumber = 0;
			State = ReceptionState.Idle;
		}
		private void InitializeAllays(){
			allay = new AllayController[10];
			for(var i = 0; i < allay.Length; i++){
				allay[i] = GameObject.Find("Allay" + (i + 1)).GetComponent<AllayController>();
				allay[i].IsTaken = true;
				allay[i].AllayNumber = i + 1;
			}
			allay[Random.Range(0, 9)].IsTaken = false;
		}
		private void Update(){
			if(Input.GetKey("escape")) Application.Quit();
			if(State == ReceptionState.PhysicalPlayersSetup || State == ReceptionState.AIPlayersSetup) ReceptionMenu();
		}
		private void ReceptionMenu(){
			switch(State){
				case ReceptionState.PhysicalPlayersSetup:
					if(Input.GetButtonDown("UpMenu") && physicalPlayersNumber < MaxPlayers)
						physicalPlayersNumber++;
					else if(Input.GetButtonDown("DownMenu") && physicalPlayersNumber > 0)
						physicalPlayersNumber--;
					else if(Input.GetButtonDown("Submit")){
						if(physicalPlayersNumber == MaxPlayers){
							StartGame();
							return;
						}
						State = ReceptionState.AIPlayersSetup;
						return;
					}
					infoText.GetComponentInChildren<Text>().text = "Welcome to our Bowling! #Players to Join: " + physicalPlayersNumber;
					break;
				case ReceptionState.AIPlayersSetup:
					if(Input.GetButtonDown("UpMenu") && AIPlayersNumber < MaxPlayers - physicalPlayersNumber)
						AIPlayersNumber++;
					else if(Input.GetButtonDown("DownMenu") && AIPlayersNumber > 0)
						AIPlayersNumber--;
					else if(Input.GetButtonDown("Submit")){
						StartGame();
						return;
					}
					infoText.GetComponentInChildren<Text>().text = "Welcome to our Bowling! #Artificially Intelligent Players to Join: " + AIPlayersNumber;
					break;
				case ReceptionState.Idle:
					break;
				case ReceptionState.GameStarted:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		private void OnTriggerEnter(Collider other){
			if(State == ReceptionState.GameStarted) return;
			infoText.SetActive(true);
			if(State == ReceptionState.Idle){
				physicalPlayersNumber = 0;
				AIPlayersNumber = 0;
				State = ReceptionState.PhysicalPlayersSetup;
			}
		}
		private void OnTriggerExit(Collider other){
			infoText.SetActive(false);
		}
		private void StartGame(){
			for(var i = 0; i < allay.Length; i++){
				if(allay[i].IsTaken)
					continue;
				GameObject.Find("FPSController").GetComponent<AINavigation>().allayNumber = i;
				infoText.GetComponentInChildren<Text>().text = "You are ready to start your game on allay " + (i + 1);
				allay[i].StartGame(physicalPlayersNumber, AIPlayersNumber);
				State = ReceptionState.GameStarted;
				return;
			}
		}
	}
}