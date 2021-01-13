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
    private KeyCode[] keyCodes = {
         KeyCode.Alpha0,
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };
    private int selectedIndex = 0;

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
            while (!(Input.GetKey(KeyCode.Q)) || selectedIndex == 0 || selectedIndex > currentVid.nexts.Length)
                {
                    for (int i = 0; i < keyCodes.Length; i++)
                    {
                        if (Input.GetKeyDown(keyCodes[i]))
                        {
                            int numberPressed = i;
                            selectedIndex = selectedIndex * 10 + numberPressed;
                            Debug.Log(numberPressed + " total: " + selectedIndex);
                        }
                    }
                if (selectedIndex > currentVid.nexts.Length)
                {
                   //Debug.Log("Current Selected Number too high");
                }
                if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete))
                 {
                    Debug.Log("Input deleted");
                    selectedIndex = 0;
                 }
                yield return null;
            }
            Debug.Log("Press Detected with number " + selectedIndex);
            nextVid = currentVid.nexts[selectedIndex - 1];
            selectedIndex = 0;
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
