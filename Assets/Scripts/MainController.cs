using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class MainController : MonoBehaviour
{
    [FormerlySerializedAs("screenController")] [SerializeField] private GameManager gameManager;

    [SerializeField] private VideoPlayer loop;
    [SerializeField] private VideoPlayer launch;

    private void Start()
    {
        this.loop.Play();
        this.launch.Stop();

        this.launch.loopPointReached += EndReached;
    }

    private void Update()
    {
        if (!loop.isPlaying) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.loop.Stop();
            this.launch.Play();
        }
    }

    private void EndReached(VideoPlayer vp)
    {
        this.gameManager.Launch();
    }
}