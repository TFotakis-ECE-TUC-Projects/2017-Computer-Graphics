using Assets.Scripts;
using UnityEngine;
public class PinSetController: MonoBehaviour {
	public int pinsDownNumber;
	private PinController[] pins;
	void Start() {
		pinsDownNumber = 0;
		pins = new PinController[10];
		for(int i = 0; i < pins.Length; i++) {
			pins[i] = transform.Find("Pin (" + (i + 1) + ")").GetComponent<PinController>();
		}
	}
	public int NumberOfFallenPins() {
		var count = 0;
		for(int i = 0; i < pins.Length; i++) {
			if(pins[i].gameObject.activeSelf && pins[i].IsDown()) count++;
		}
		return count;
	}
	public void DeactivateFallenPins() {
		for(int i = 0; i < pins.Length; i++) {
			if(pins[i].IsDown()) pins[i].gameObject.SetActive(false);
		}
	}
}