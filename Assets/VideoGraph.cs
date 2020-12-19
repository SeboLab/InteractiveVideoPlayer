using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoGraph : MonoBehaviour
{
    public VideoClip clip;
    public VideoGraph[] nexts;
    public bool isStableState;

    [HideInInspector] public Camera cam;

    [HideInInspector] public VideoPlayer videoPlayer;

    public void init()
    {
        Debug.Log("I'm Being initialised");
        videoPlayer = GetComponent<VideoPlayer>();
        //initialize the variables
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        //We want to play from video clip not from url
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.targetCamera = cam;
        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.aspectRatio = VideoAspectRatio.FitVertically;
        //Set video Clip To Play 
        videoPlayer.clip = clip;

    }
}
