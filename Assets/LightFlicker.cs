using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lightSource;
    public float minInterval;
    public float maxInterval;
    public float minFlickerInterval;
    public float maxFlickerInterval;
    float next;

    public AudioSource audioSource;
    private Renderer rendererMaterial;
    private float _emissiveIntensity;
    private Color _emissiveColor;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    private IEnumerator Flicker(float delay, int times)
    {
        audioSource.Play();
        for (int i = 0; i < times; i++)
        {
            rendererMaterial.material.SetColor("_EmissiveColor", _emissiveColor * 0);
            lightSource.gameObject.SetActive(false);
            print(i);
            yield return new WaitForSeconds(delay);
            print(i);
            lightSource.gameObject.SetActive(true);
            rendererMaterial.material.SetColor("_EmissiveColor", _emissiveColor * _emissiveIntensity);
            if (i + 1 < times)
            {
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
