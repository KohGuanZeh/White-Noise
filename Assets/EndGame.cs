using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    LoadingScreen loadingScreen;
    public int nextLevel;

    private void Start()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            loadingScreen.LoadNextLevel(nextLevel);
        }
    }
}
