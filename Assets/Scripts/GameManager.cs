using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Raw Image to Show Video Images [Assign from the Editor]
    //public RawImage image;
    //public VideoPlayer VP;
    //Set from the Editor
    public VideoGraph[] allVideos;
    public VideoGraph first;
    private VideoGraph currentVid;
    private VideoGraph lastVid;
    public Camera cam;
    VideoGraph nextVid;


    void Start()
    {
        int i = 0;
        for (; i < allVideos.Length; i++)
        {
            allVideos[i].init();
            Debug.Log("initialised "+ i);
            allVideos[i].gameObject.SetActive(false);
            
        }
        if (i <= 0) {
            Debug.Log("please put clips into the array");
                }
        currentVid = first;
        StartCoroutine(playVideo(true));

    }


    IEnumerator loadVideos(VideoGraph vid)
    {
        for(int i = 0; i < vid.nexts.Length; i++)
        {
            vid.nexts[i].gameObject.SetActive(true);
            vid.nexts[i].videoPlayer.Prepare();
            Debug.Log("Currently Preparing " +i);
        }
        
        yield return 0;
    }

    IEnumerator playVideo(bool first)
    {
        StartCoroutine(loadVideos(currentVid));
        Debug.Log("go go go " + currentVid);
        currentVid.gameObject.SetActive(true);
        currentVid.cam = cam;
        if (first)
        {
            currentVid.videoPlayer.Prepare();
            while (!currentVid.videoPlayer.isPrepared)
            {
                yield return null;
            }
        }
        //currentVid.nexts[0].gameObject.SetActive(true);
        //currentVid.nexts[0].videoPlayer.Prepare();
        Debug.Log("Done Preparing ");
        currentVid.videoPlayer.Play();
        if (currentVid.isStableState)
        {
            Debug.Log("Waiting for input");
            while (!Input.GetKeyDown(KeyCode.Q) &&!Input.GetKeyDown(KeyCode.W))
            {
                yield return null;
            }
            Debug.Log("Press Detected");
            if (Input.GetKeyDown(KeyCode.Q) || currentVid.nexts.Length<=1)
            {
                nextVid = currentVid.nexts[0];
            }else if(Input.GetKeyDown(KeyCode.W)&& currentVid.nexts.Length > 1)
            {
                nextVid = currentVid.nexts[1];
            }
        }
 
        Debug.LogWarning("Done Preparing current Video");
        if (!currentVid.isStableState)
        {
            while (currentVid.videoPlayer.isPlaying)
            {
                yield return null;
            }
            Debug.Log("No press necessary, no stable state");
            nextVid = currentVid.nexts[0];
        }
        while (!nextVid.videoPlayer.isPrepared)
        {
            Debug.Log("Preparing NEXT Video Index: " + nextVid);
            yield return null;
        }

        currentVid.videoPlayer.Stop();
        currentVid.cam = null;
        lastVid = currentVid;
        currentVid = nextVid;
        
        Debug.LogWarning("Now playing Next Video");

        StartCoroutine(playVideo(false));

    }
}
