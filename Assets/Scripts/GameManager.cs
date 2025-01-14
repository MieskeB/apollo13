using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private bool _launched;

    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject trajectoryScreen;
    [SerializeField] private GameObject healthScreen;
    [SerializeField] private GameObject statusScreen;

    [SerializeField] private RocketLocation rocketLocation;

    [SerializeField][Range(1,10)] private float secondsBetweenScreenSwitches = 5f;

    [SerializeField] private float gameDurationSeconds = 3600f;

    public UnityEvent m_GameFinished;

    private Screen currentScreen;

    private void Start()
    {
        this.m_GameFinished ??= new UnityEvent();
        
        this._launched = false;
        this.SwitchScreen(Screen.Main);
        this.currentScreen = Screen.Main;
    }


    private void Update()
    {
        if (!_launched) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.SwitchScreen(Screen.Trajectory);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.SwitchScreen(Screen.Health);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            this.SwitchScreen(Screen.Status);
        }
    }

    private void StartScreenSwitching()
    {
        InvokeRepeating(nameof(SwitchToNextScreen), 0f, secondsBetweenScreenSwitches);
    }

    private void SwitchToNextScreen()
    {
        this.currentScreen = GetNextScreen(this.currentScreen);
        this.SwitchScreen(this.currentScreen);
    }


    public void Launch()
    {
        this._launched = true;
        this.rocketLocation.launch();
        this.StartScreenSwitching();
    }

    public void Land()
    {
        CancelInvoke(nameof(SwitchToNextScreen));
        this.SwitchScreen(Screen.Main);
        this.m_GameFinished.Invoke();
    }


    public Screen GetActiveScreen()
    {
        return this.currentScreen;
    }


    private void SwitchScreen(Screen screen)
    {
        mainScreen.SetActive(false);
        trajectoryScreen.SetActive(false);
        healthScreen.SetActive(false);
        statusScreen.SetActive(false);

        switch (screen)
        {
            case Screen.Main:
                mainScreen.SetActive(true);
                break;
            case Screen.Trajectory:
                trajectoryScreen.SetActive(true);
                break;
            case Screen.Health:
                healthScreen.SetActive(true);
                break;
            case Screen.Status:
                statusScreen.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
        }
    }
    
    private static Screen GetNextScreen(Screen current)
    {
        return current switch
        {
            Screen.Main => Screen.Trajectory,
            Screen.Trajectory => Screen.Health,
            Screen.Health => Screen.Status,
            Screen.Status => Screen.Trajectory,
            _ => Screen.Main
        };
    }

    public enum Screen
    {
        Main,
        Trajectory,
        Health,
        Status
    }

    public float GetGameDurationSeconds()
    {
        return this.gameDurationSeconds;
    }
}