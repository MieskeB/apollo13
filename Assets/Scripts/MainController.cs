using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class MainController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private VideoPlayer loop;
    [SerializeField] private VideoPlayer launch;
    [SerializeField] private VideoPlayer end;

    private void Start()
    {
        this.loop.Play();
        this.launch.Stop();

        this.launch.loopPointReached += EndReached;
        this.gameManager.m_GameFinished.AddListener(OnEndReached);
    }

    private void Update()
    {
        if (!loop.isPlaying) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.loop.Stop();
            this.launch.Play();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            EndReached(null);
        }
    }

    private void EndReached(VideoPlayer vp)
    {
        this.gameManager.Launch();
    }

    void OnEndReached()
    {
        this.end.Play();
    }
}