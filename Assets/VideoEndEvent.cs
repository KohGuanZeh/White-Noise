using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class VideoEndEvent : MonoBehaviour
{
    public VideoPlayer video;
	public UnityEngine.Video.VideoPlayer.EventHandler eventHandlers;

	void Start(){
		eventHandlers = delegate {
SceneManager.LoadScene("Game_Level_1");
};
		video.loopPointReached += eventHandlers;
	}
}
