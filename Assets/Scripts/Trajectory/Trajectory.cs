using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] public float maxBulletDistance = 4f;
    [SerializeField] public float startOffset = 0.4f;
    [SerializeField] public LayerMask wallLayer;

    [Header("Line Visualization")]
    [SerializeField] private Color lineColor = new Color(1f, 0f, 0f, 0.7f);
    [SerializeField] private float lineWidth = 0.05f;

    [Header("End Marker")]
    [SerializeField] private GameObject endMarker;
    [SerializeField] private float markerSize = 1.0f;

    [Header("References")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private PlayerController playerController;

    private Camera mainCamera;
    public Vector2 shootDirection;
    private Vector2 trajectoryEndPoint;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        mainCamera = Camera.main;

        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
        }

        CreateLineRenderer();

        if (endMarker == null)
        {
            CreateEndMarker();
        }
    }

    private void Update()
    {
        bool isPlayerMoving = playerController.IsMoving();

        if (isPlayerMoving)
        {
            SetTrajectoryVisible(false);
        }
        else
        {
            SetTrajectoryVisible(true);
            CalculateTrajectory();
            UpdateLineVisualization();
        }
    }

    private void CalculateTrajectory()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 startPoint = shootPoint.position;
        shootDirection = ((Vector2)mouseWorldPos - startPoint).normalized;

        Vector2 raycastStart = startPoint + shootDirection * startOffset;

        RaycastHit2D hit = Physics2D.Raycast(
            raycastStart,
            shootDirection,
            maxBulletDistance - startOffset,
            wallLayer
        );

        if (hit.collider != null)
        {
            trajectoryEndPoint = hit.point;
        }
        else
        {
            trajectoryEndPoint = raycastStart + shootDirection * (maxBulletDistance - startOffset);
        }
    }

    private void UpdateLineVisualization()
    {
        Vector2 startPoint = (Vector2)shootPoint.position + shootDirection * startOffset;

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, trajectoryEndPoint);

        if (endMarker != null)
        {
            endMarker.transform.position = trajectoryEndPoint;
        }
    }

    private void CreateLineRenderer()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.sortingLayerName = "Trajectory";
        lineRenderer.sortingOrder = 100;
    }

    private void CreateEndMarker()
    {
        endMarker = new GameObject("TrajectoryEndMarker");
        endMarker.transform.SetParent(transform);

        SpriteRenderer spriteRenderer = endMarker.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreatePlusSprite();
        spriteRenderer.color = lineColor;
        spriteRenderer.sortingLayerName = "Trajectory";
        spriteRenderer.sortingOrder = 101;

        endMarker.transform.localScale = Vector3.one * markerSize;
    }

    private Sprite CreatePlusSprite()
    {
        int size = 32;
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }

        int center = size / 2;
        int thickness = 4;

        for (int y = 0; y < size; y++)
        {
            for (int x = center - thickness / 2; x < center + thickness / 2; x++)
            {
                pixels[y * size + x] = Color.white;
            }
        }

        for (int x = 0; x < size; x++)
        {
            for (int y = center - thickness / 2; y < center + thickness / 2; y++)
            {
                pixels[y * size + x] = Color.white;
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
        texture.filterMode = FilterMode.Point;

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    public Vector2 GetShootDirection()
    {
        return shootDirection;
    }

    public Vector2 GetTrajectoryEndPoint()
    {
        return trajectoryEndPoint;
    }

    public void SetTrajectoryVisible(bool visible)
    {
        lineRenderer.enabled = visible;

        if (endMarker != null)
        {
            endMarker.SetActive(visible);
        }
    }
}
