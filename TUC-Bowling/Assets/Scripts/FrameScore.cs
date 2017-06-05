namespace Assets.Scripts{
	public class FrameScore {
		public int[] score;
		public FrameScore() {
			score = new int[3];
			score[0] = -1;
			score[1] = -1;
			score[2] = -1;
		}
		public int TotalScore() {
			return score[0] + score[1] + score[2];
		}
		public int PartialScore() {
			return score[0] + score[1];
		}
	}
}