using UnityEngine;
using System.Collections;

public class MenuLightFlicker : MonoBehaviour {

    public Light light1;
    public Light light2;
    public Light light3;

    public float minIntensity = 0.1f;
    public float maxIntensity = 1.0f;

    void Start () {
        StartCoroutine("Flicker");
    }

    IEnumerator Flicker () {
        while (true) {
            light1.intensity = Random.Range(minIntensity, maxIntensity);
            light2.intensity = Random.Range(minIntensity, maxIntensity);
            light3.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
    }
}

