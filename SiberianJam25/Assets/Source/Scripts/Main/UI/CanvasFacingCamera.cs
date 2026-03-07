using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFacingCamera : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private bool faceCamera = true; // Смотреть на камеру или от камеры
    [SerializeField] private bool maintainWorldUp = false; // Сохранять вертикаль мира

    private Transform canvasTransform;
    private RectTransform rectTransform;

    void Start()
    {
        canvasTransform = transform;
        rectTransform = GetComponent<RectTransform>();

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        // Убедимся, что Canvas в режиме World Space
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null && canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogWarning("Canvas должен быть в режиме World Space для этого скрипта!");
        }
    }

    void LateUpdate()
    {
        if (targetCamera == null || canvasTransform == null) return;

        Vector3 direction;

        if (faceCamera)
        {
            direction = canvasTransform.position - targetCamera.transform.position;
        }
        else
            direction = targetCamera.transform.position - canvasTransform.position;
    
        
        if (direction != Vector3.zero)
        {
            if (maintainWorldUp)
            {
                canvasTransform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
            else
{
    canvasTransform.rotation = Quaternion.LookRotation(direction);
}
        }
    }
}

