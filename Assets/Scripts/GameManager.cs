using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _launched;

    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject trajectoryScreen;
    [SerializeField] private GameObject healthScreen;
    [SerializeField] private GameObject statusScreen;

    private void Start()
    {
        this._launched = false;
        this.SwitchScreen(Screen.Main);
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


    public void Launch()
    {
        this._launched = true;
        this.SwitchScreen(Screen.Trajectory);
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

    private enum Screen
    {
        Main,
        Trajectory,
        Health,
        Status
    }
}