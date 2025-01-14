using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class RocketLocation : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject rocket;
    
    [SerializeField] private TextMeshProUGUI counterDisplay;
    
    [SerializeField] private Transform[] points;
    private float flightDurationSeconds;
    private float moveSpeed = 5;
    private int currentPointIndex = 0;

    private float totalDistance = 0;

    private bool launched = false;
    private bool landed = false;
    
    private float elapsedTime = 0f;

    private void Start()
    {
        this.flightDurationSeconds = this.gameManager.GetGameDurationSeconds();
        
        this.transform.position = points[0].position;
        for (int i = 0; i < points.Length - 1; i++)
        {
            totalDistance += Vector2.Distance(points[i].position, points[i + 1].position);
        }

        moveSpeed = totalDistance / flightDurationSeconds;
    }

    private void Update()
    {
        if (launched && !landed) {
            elapsedTime += Time.deltaTime;
            int hours = (int)(elapsedTime / 3600);
            int minutes = (int)(elapsedTime % 3600 / 60);
            int seconds = (int)(elapsedTime % 60);
            counterDisplay.text = $"T+{hours:00}:{minutes:00}:{seconds:00}";
        }
        
        if (!launched) return;
        
        if (this.rocket.activeSelf)
        {
            this.rocket.transform.position = this.transform.position;
            this.rocket.transform.rotation = this.transform.rotation;
        }

        if (currentPointIndex >= points.Length - 1)
        {
            if (!this.landed)
            {
                this.gameManager.Land();
                this.landed = true;
            }

            return;
        }
        
        this.transform.position = Vector2.MoveTowards(this.transform.position, points[currentPointIndex].position,
            moveSpeed * Time.deltaTime);

        if (!Mathf.Approximately(this.transform.position.x, points[currentPointIndex].position.x) ||
            !Mathf.Approximately(this.transform.position.y, points[currentPointIndex].position.y)) return;
        
        // Rotate the ship
        Vector2 direction = points[currentPointIndex + 1].position - points[currentPointIndex].position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                
        currentPointIndex++;
    }

    public void launch()
    {
        this.launched = true;
    }
}