using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightFlicker : MonoBehaviour
{
    public HDAdditionalLightData lightSource;
    public LightEnemyRadius lightEnemyRadius;
    public float enemiesAroundLightMinDistance = 5;
    public float enemiesAroundLightMaxDistance = 50;
    public float minInterval = 5;
    public float maxInterval = 10;
    public float minFlickerInterval = 0.1f;
    public float maxFlickerInterval = 0.2f;
    float next;

    public AudioSource audioSource;
    private Renderer rendererMaterial;
    private float _emissiveIntensity;
    private Color _emissiveColor;

    private float allEnemiesAroundDistanceNormalized;

    // Start is called before the first frame update
    void Start()
    {
        lightEnemyRadius.Setup(lightSource);
        rendererMaterial = GetComponent<Renderer>();
        _emissiveIntensity = rendererMaterial.sharedMaterial.GetFloat("_EmissiveIntensity");
        _emissiveColor = rendererMaterial.sharedMaterial.GetColor("_EmissiveColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= next)
        {
            next = Time.time + Random.Range(minInterval, maxInterval);
            StartCoroutine(Flicker(Random.Range(minFlickerInterval, maxFlickerInterval), 2));
        }

        if (lightSource.gameObject.activeSelf)
        {
            allEnemiesAroundDistanceNormalized = lightEnemyRadius.AllEnemiesAroundDistanceNormalized(enemiesAroundLightMinDistance, enemiesAroundLightMaxDistance);
            print(name);
            print(allEnemiesAroundDistanceNormalized);
            rendererMaterial.material.SetColor("_EmissiveColor", _emissiveColor * allEnemiesAroundDistanceNormalized);
            lightEnemyRadius.UpdateLight(allEnemiesAroundDistanceNormalized * lightEnemyRadius.originalIntensity);
        }
    }

    private IEnumerator Flicker(float delay, int times)
    {
        audioSource.Play();
        for (int i = 0; i < times; i++)
        {
            rendererMaterial.material.SetColor("_EmissiveColor", _emissiveColor * 0);
            lightSource.gameObject.SetActive(false);
            yield return new WaitForSeconds(delay);
            lightSource.gameObject.SetActive(true);
            rendererMaterial.material.SetColor("_EmissiveColor", _emissiveColor * _emissiveIntensity * allEnemiesAroundDistanceNormalized);
            if (i + 1 < times)
            {
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
