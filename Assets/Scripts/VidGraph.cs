using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VidGraph : MonoBehaviour
{
    public VideoClip clip;
    public VidGraph[] nexts;
    public bool isStableState;

    [HideInInspector] public Camera cam;

    [HideInInspector] public VideoPlayer videoPlayer;

    public void init()
    {
        //initialize the variables
        if (!isStableState && nexts.Length != 1)
        {
            Debug.LogError("Non Stable State needs exactly 1 next video");
        }
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
