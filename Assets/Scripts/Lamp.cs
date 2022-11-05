using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Lamp : MonoBehaviour
{
    [SerializeField] public HDAdditionalLightData light;
    [SerializeField] float minRange = 0;
    [SerializeField] float maxRange = 50;
    [SerializeField] float minIntensity = 0;
    [SerializeField] float maxIntensity = 2.5f;

    [SerializeField] float lifeSpan = 100f;
    [SerializeField] float depletionRate = 0.8f;
    [SerializeField] float gainRate = 2.5f;
    public float gainAmt = 0;

    void Start() {
        light = GetComponent<HDAdditionalLightData>();
        float lerp_val = Mathf.Log10(Mathf.Lerp(1, 10, lifeSpan / 100));
        light.intensity = Mathf.Lerp(minIntensity, maxIntensity, lerp_val);
    }

    // Update is called once per frame
    void Update() {
        if (gainAmt > 0) {
            float gain = gainRate * Time.deltaTime;
            lifeSpan += gain;
            if (lifeSpan > 100) {
                gainAmt = 0;
            } else {
                gainAmt -= gain;
            }
        }

        if (lifeSpan > 0) {
            lifeSpan -= depletionRate * Time.deltaTime;
        }

        float lerp_val = Mathf.Log10(Mathf.Lerp(1, 10, lifeSpan / 100));
        light.intensity = Mathf.Lerp(minIntensity, maxIntensity, lerp_val);
        // No need to adjust range
        //light.range = Mathf.Lerp(minRange, maxRange, lerp_val);
    }

    public void DepleteLamp(float depleteAmt) {
        lifeSpan -= depleteAmt;
    }
}
