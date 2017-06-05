using UnityEngine;
public class ColorChanger: MonoBehaviour{
	private GameObject bigPointLight;
	private GameObject[] pointLights;
	private AudioSource audioSource;
	private float[] samples = new float[64];
	private Color color;
	private GameObject[] pointLightMaterials;
	void Start(){
		pointLights = GameObject.FindGameObjectsWithTag("MiddlePointLight");
		bigPointLight = GameObject.Find("Area Light");
		pointLightMaterials = GameObject.FindGameObjectsWithTag("MiddlePointLightMaterial");
		audioSource = GameObject.Find("Ceilling").GetComponent<AudioSource>();
	}
	void Update(){
		float t = Mathf.PingPong(Time.time, 2) / 2;
		audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
		float r = 0, g = 0, b = 0;
		for(var i = 0; i < 21; i++){
			r += samples[i];
			g += samples[21 + 1];
			b += samples[42 + 1];
		}
		r = Mathf.Clamp01(r * 25.5f / 21);
		g = Mathf.Clamp01(g * 500 / 21);
		b = Mathf.Clamp01(b * 5000 / 21);
//		Debug.Log("R=" + r + " G=" + g + " B=" + b);
		if(r > 0.3 || b > 0.7){
			color = new Color(r, g, b);
//			foreach(var pointLight in pointLights){
//				pointLight.GetComponent<Light>().color = color;
//			}
			bigPointLight.GetComponent<Light>().color = color;
			foreach(var pointLightMaterial in pointLightMaterials){
				pointLightMaterial.GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
			}
		} else{
//			foreach(var pointLight in pointLights){
//				pointLight.GetComponent<Light>().color = Color.Lerp(color, new Color(0, 0, 0.3f), t);
//			}
			foreach(var pointLightMaterial in pointLightMaterials){
				pointLightMaterial.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(color, new Color(0, 0, 0.3f), t));
			}
			bigPointLight.GetComponent<Light>().color = Color.Lerp(color, new Color(0, 0, 0.3f), t);
		}
	}
}