using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Sprite earthSprite;
    [SerializeField] private Sprite moonSprite;
    private RectTransform graphContainer;

    private float _totalDistance;

    private void Start()
    {
        this._totalDistance = 0;
        this.ShowGraph();
    }

    
    private void Awake()
    {
        graphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
    }


    private void Update()
    {
        
    }

    
    private void ShowGraph()
    {
        this.CreatePlanets();
        
        GameObject firstCircleGameObject = null;
        GameObject lastCircleGameObject = null;

        for (float i = -1.5f; i < 2.0f * Mathf.PI + 0.5f; i += 0.001f)
        {
            Vector2 anchoredPosition = this.GetRightPositionOfPoint(i, true);
            GameObject circleGameObject = this.CreateCircle(anchoredPosition);
            if (firstCircleGameObject == null)
            {
                firstCircleGameObject = circleGameObject;
            }

            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition,
                    circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }

            lastCircleGameObject = circleGameObject;
        }

        for (float i = 2.0f * Mathf.PI + 0.5f; i > -1.5f; i -= 0.001f)
        {
            Vector2 anchoredPosition = this.GetRightPositionOfPoint(i, false);
            GameObject circleGameObject = this.CreateCircle(anchoredPosition);
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition,
                    circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }

            lastCircleGameObject = circleGameObject;
        }

        CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition,
            firstCircleGameObject.GetComponent<RectTransform>().anchoredPosition);
    }

    private Vector2 GetRightPositionOfPoint(float x, bool firstSegment)
    {

        Vector2 res = new Vector2(x, this.CalculateValueOnGraph(x, firstSegment));

        return this.ToWorldSpace(res);
    }

    private Vector2 ToWorldSpace(Vector2 localSpace)
    {
        Vector2 worldSpace = new();
        {
            const float inputMax = 2.0f * Mathf.PI + 0.5f + 1.5f;
            float outputMax = graphContainer.rect.width;
            worldSpace.x = (localSpace.x + 1.5f) * outputMax / inputMax;
        }
        {
            const float inputMax = 3.0f;
            float outputMax = graphContainer.rect.height;
            worldSpace.y = localSpace.y * outputMax / inputMax + outputMax / 2;
        }
        return worldSpace;
    }

    private float CalculateValueOnGraph(float x, bool firstSegment)
    {
        if (x is < -1.5f or > 2.0f * Mathf.PI + 0.5f)
        {
            Debug.Log("x cannot have value: " + x);
            return 0.0f;
        }

        float v = 0.0f;
        switch (x)
        {
            case < 0.0f:
                v = Mathf.Sqrt(2.25f - Mathf.Pow(x, 2));
                v = firstSegment ? v : v * -1;
                break;
            case >= 0.0f and <= 2 * Mathf.PI:
                v = Mathf.Cos(0.5f * x);
                v = firstSegment ? v + 0.5f : (v - 0.5f) * -1 - 1f;
                break;
            case > 2 * Mathf.PI:
                v = Mathf.Sqrt(0.25f - Mathf.Pow(x - 2.0f * Mathf.PI, 2.0f));
                v = firstSegment ? v * -1 : v;
                break;
        }

        return v;
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("Point", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("DotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        this._totalDistance += distance;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(100, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
    }

    private void CreatePlanets()
    {
        Vector2 earthPos = this.ToWorldSpace(new Vector2(0, 0));
        this.CreateEarth(earthPos);
        Vector2 moonPos = this.ToWorldSpace(new Vector2(2.0f * Mathf.PI, 0));
        this.CreateMoon(moonPos);
    }

    private void CreateEarth(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("Earth", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = earthSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(500, 500);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private void CreateMoon(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("Moon", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = moonSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(120, 120);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private static float GetAngleFromVectorFloat(Vector2 normalizedDirection)
    {
        float angleRadians = Mathf.Atan2(normalizedDirection.y, normalizedDirection.x);

        // Convert radians to degrees and ensure the angle is between 0 and 360 degrees
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        if (angleDegrees < 0)
        {
            angleDegrees += 360f;
        }

        return angleDegrees;
    }
}