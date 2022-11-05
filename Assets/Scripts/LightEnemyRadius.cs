using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightEnemyRadius : MonoBehaviour
{
    public float originalIntensity;
    HDAdditionalLightData lightSource;
    List<float> enemiesAround = new List<float>();

    public void Setup(HDAdditionalLightData light)
    {
        lightSource = light;
        originalIntensity = light.intensity;
    }

    public List<float> EnemiesAroundDistanceNormalized(float minDistance, float maxDistance)
    {
        enemiesAround.Clear();
        Collider[] colls = Physics.OverlapSphere(transform.position, maxDistance);
        foreach (Collider c in colls)
        {
            if (c.tag == "Enemy")
            {
                float scale = (Vector3.Distance(transform.position, c.transform.position) - minDistance) / (maxDistance - minDistance);
                enemiesAround.Add(scale);
            }
        }
        return enemiesAround;
    }

    public float AllEnemiesAroundDistanceNormalized(float minDistance, float maxDistance)
    {
        float overallScale = 1f;
        Collider[] colls = Physics.OverlapSphere(transform.position, maxDistance);
        foreach (Collider c in colls)
        {
            if (c.tag == "Enemy")
            {
                float scale = (Vector3.Distance(transform.position, c.transform.position) - minDistance) / (maxDistance - minDistance);
                overallScale *= scale;
            }
        }
        return overallScale;
    }

    public void UpdateLight(float intensity)
    {
        lightSource.intensity = intensity;
    }
}
