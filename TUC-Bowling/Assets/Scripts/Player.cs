using Assets.Scripts;
using UnityEngine;
public class Player{	
	public FrameScore[] frameScore;
	public int completedFrames;
	public GameObject fpsController;
	public BallController ballController;
	public Player(){
		frameScore = new FrameScore[12];
		for(int i = 0; i < frameScore.Length; i++){
			frameScore[i] = new FrameScore();
		}
		completedFrames = 0;
		fpsController=GameObject.Find("FPSController");
		ballController = GameObject.FindWithTag("Ball").GetComponent<BallController>();
	}
	public int TotalScore(){
		var score = 0;
		for(var i = 0; i < completedFrames; i++){
			score += frameScore[i].TotalScore();
		}
		return score;
	}
	public void SetScore(int frameId, int chance, int score){
		if(chance == 0) SetScore1(frameId, score);
		else SetScore2(frameId, score);
	}
	private void SetScore1(int frameId, int score){
		frameScore[frameId].score[0] = score;
		if(frameId <= 0) return;
		if(frameScore[frameId - 1].score[1] == -1){
			SetScore2(frameId - 1, score);
		} else if(frameScore[frameId - 1].score[2] == -1){
			frameScore[frameId - 1].score[2] = score;
			completedFrames++;
		}
	}
	private void SetScore2(int frameId, int score){
		frameScore[frameId].score[1] = score;
		if(frameScore[frameId].score[0] < 10 && frameScore[frameId].PartialScore() != 10){
			frameScore[frameId].score[2] = 0;
			completedFrames++;
		}
		if(frameId <= 0 || frameScore[frameId - 1].score[2] != -1) return;
		frameScore[frameId - 1].score[2] = score;
		completedFrames++;
	}
	public virtual void Play(){}
}