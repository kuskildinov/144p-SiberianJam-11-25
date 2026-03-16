using System.Collections.Generic;
using UnityEngine;

public class SecureCam : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [Header("Movment Settings")]
    [SerializeField] private Transform[] _targets;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _holdDuration = 2f;
    [SerializeField] private Transform _eye;

    [Header("Spotlight Settings")]
    [SerializeField] private Light _eyeSpotlight;  
    [SerializeField] private float _spotlightAngle = 50f;

    [Header("Player Detection")]      
    [SerializeField] private float _detectionRange = 8f;

    [Header("Visualization Settings")]
    [SerializeField] private bool _showDetectionVisuals = true;
    [SerializeField] private Material _spotlightConeMaterial;
    [SerializeField] private float _visualsUpdateInterval = 0.1f;

    private int _currentIndex = 0;
    private bool _movingForward = true;
    private float _holdTimer = 0f;
    private bool _isLooking = true;

    private bool _playerInSight = false;
    private Transform _playerTransform;
    private Player _currentDetectedPlayer;

    private GameObject _visualizationContainer;
    private GameObject _rangeSphere;
    private GameObject _spotlightCone;
    private LineRenderer _targetLineRenderer;
    private List<LineRenderer> _pathLines = new List<LineRenderer>();
    private float _lastVisualsUpdate;

    private void Start()
    {
        if (_showDetectionVisuals)
        {
            CreateDetectionVisuals();
        }
    }

    private void Update()
    {
        if (_targets == null || _targets.Length == 0 || _isActive == false) return;

        if (_isLooking && _playerInSight == false)
        {
            HandleCamMovment();
        }
        else
        {
            // Немедленно переходим к следующей цели
            _isLooking = true;
        }

        if (_eyeSpotlight == null) return;

        CheckPlayerDetection();

        if (_showDetectionVisuals && Time.time > _lastVisualsUpdate + _visualsUpdateInterval)
        {
            UpdateDetectionVisuals();
            _lastVisualsUpdate = Time.time;
        }
    }

    #region Movment

    private void HandleCamMovment()
    {
        // Плавно поворачиваемся к цели
        Vector3 direction = (_targets[_currentIndex].position - _eye.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        _eye.rotation = Quaternion.Slerp(_eye.rotation, targetRot, _rotationSpeed * Time.deltaTime);

        // Если смотрим на цель, начинаем задержку
        if (Quaternion.Angle(_eye.rotation, targetRot) < 2f)
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= _holdDuration)
            {
                _holdTimer = 0f;
                _isLooking = false;
                GetNextTarget();
            }
        }
    }

    private void GetNextTarget()
    {
        if (_movingForward)
        {
            _currentIndex++;
            if (_currentIndex >= _targets.Length)
            {
                _currentIndex = _targets.Length - 2;
                _movingForward = false;
            }
        }
        else
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = 1;
                _movingForward = true;
            }
        }
    }

    #endregion

    #region Player Detection

    private void CheckPlayerDetection()
    {       
        Collider[] hitColliders = Physics.OverlapSphere(_eye.position, _detectionRange);
        bool detected = false;

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToPlayer = (hitCollider.transform.position - _eye.position).normalized;
            float angle = Vector3.Angle(_eye.forward, directionToPlayer);

            if (angle <= _spotlightAngle / 2f)
            {              
                RaycastHit hit;
                if (Physics.Raycast(_eye.position, directionToPlayer, out hit, _detectionRange))
                {
                    if (hit.transform.TryGetComponent<Player>(out Player player))
                    {
                        if (player.CheckCanBeDetected() == false)
                        {
                            Debug.Log("TEST");
                            break;
                        }

                        _currentDetectedPlayer = player;
                        detected = true;
                        _playerTransform = hitCollider.transform;
                        LookAtPlayer(_playerTransform);
                        if (!_playerInSight)
                        {
                            OnPlayerDetected();
                            _currentDetectedPlayer.DetectedBySecure(_eye);
                        }
                        break;
                    }                   
                }
            }
        }

        if (!detected && _playerInSight)
        {
            OnPlayerLost();
            _currentDetectedPlayer.LostDetectionBySecure();
        }
    }

    private void OnPlayerDetected()
    {
        _playerInSight = true;      
      
        if (_eyeSpotlight != null)
        {
            _eyeSpotlight.color = Color.red;
        }
    }

    private void OnPlayerLost()
    {
        _playerInSight = false;       
        if (_eyeSpotlight != null)
        {
            _eyeSpotlight.color = Color.white;
        }
    }

    private void LookAtPlayer(Transform player)
    {
        _eye.LookAt(player);
    }

    #endregion

    #region >>> GEOMETRY

    private void CreateDetectionVisuals()
    {
        // Создаем контейнер для всех визуальных элементов
        _visualizationContainer = new GameObject("DetectionVisuals");
        _visualizationContainer.transform.SetParent(transform);
        _visualizationContainer.transform.localPosition = Vector3.zero;
      
        // Создаем конус спотлайта
        CreateSpotlightCone();
    }

    private void CreateSpotlightCone()
    {
        _spotlightCone = new GameObject("SpotlightCone");
        _spotlightCone.transform.SetParent(_visualizationContainer.transform);
        _spotlightCone.transform.position = _eye.position;
        _spotlightCone.transform.rotation = _eye.rotation;

        // Создаем меш конуса
        Mesh coneMesh = CreateConeMesh(20, _detectionRange, _spotlightAngle);

        var meshFilter = _spotlightCone.AddComponent<MeshFilter>();
        meshFilter.mesh = coneMesh;

        var meshRenderer = _spotlightCone.AddComponent<MeshRenderer>();

        if (_spotlightConeMaterial != null)
        {
            meshRenderer.material = new Material(_spotlightConeMaterial);
        }
        else
        {
            // Создаем простой полупрозрачный материал
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = new Color(0f, 1f, 1f, 0.1f);
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
            meshRenderer.material = mat;
        }

        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;
    }

    private Mesh CreateConeMesh(int segments, float height, float angle)
    {
        Mesh mesh = new Mesh();

        float radius = Mathf.Tan(angle * 0.5f * Mathf.Deg2Rad) * height;

        // Вершины
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Вершина конуса (начало)
        vertices.Add(Vector3.zero);

        // Вершины основания
        for (int i = 0; i < segments; i++)
        {
            float rad = (float)i / segments * Mathf.PI * 2;
            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;
            vertices.Add(new Vector3(x, y, height));
        }

        // Боковые грани
        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;

            // Треугольники боковой поверхности
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(next + 1);
        }

        // Основание
        int baseStartIndex = vertices.Count;
        for (int i = 0; i < segments; i++)
        {
            float rad = (float)i / segments * Mathf.PI * 2;
            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;
            vertices.Add(new Vector3(x, y, height));
        }

        for (int i = 0; i < segments; i++)
        {
            int next = (i + 1) % segments;
            triangles.Add(baseStartIndex + i);
            triangles.Add(baseStartIndex + next);
            triangles.Add(baseStartIndex + segments);
        }

        // Добавляем центральную вершину для основания
        vertices.Add(new Vector3(0, 0, height));

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }
   
    private void UpdateDetectionVisuals()
    {
        if (!_showDetectionVisuals || _visualizationContainer == null) return;

        // Обновляем позицию сферы
        if (_rangeSphere != null)
        {
            _rangeSphere.transform.position = _eye.position;
        }

        // Обновляем позицию и поворот конуса
        if (_spotlightCone != null)
        {
            _spotlightCone.transform.position = _eye.position;
            _spotlightCone.transform.rotation = _eye.rotation;

            // Меняем цвет конуса в зависимости от обнаружения
            var renderer = _spotlightCone.GetComponent<MeshRenderer>();
            if (renderer != null && renderer.material != null)
            {
                Color targetColor = _playerInSight ? new Color(1f, 0f, 0f, 0.3f) : new Color(0f, 1f, 1f, 0.2f);
                renderer.material.color = Color.Lerp(renderer.material.color, targetColor, Time.deltaTime * 5f);
            }
        }

        // Обновляем линию к текущей цели
        if (_targetLineRenderer != null && _targets != null && _targets.Length > 0)
        {
            _targetLineRenderer.SetPosition(0, _eye.position);
            if (_currentIndex < _targets.Length && _targets[_currentIndex] != null)
            {
                _targetLineRenderer.SetPosition(1, _targets[_currentIndex].position);
            }
        }

        // Обновляем линии путей (если цели двигаются)
        for (int i = 0; i < _pathLines.Count; i++)
        {
            if (i < _targets.Length - 1 && _targets[i] != null && _targets[i + 1] != null)
            {
                _pathLines[i].SetPosition(0, _targets[i].position);
                _pathLines[i].SetPosition(1, _targets[i + 1].position);
            }
        }
    }

    // Метод для включения/выключения визуализации
    public void ToggleDetectionVisuals(bool show)
    {
        _showDetectionVisuals = show;
        if (_visualizationContainer != null)
        {
            _visualizationContainer.SetActive(show);
        }
    }

    private void OnDestroy()
    {
        if (_visualizationContainer != null)
        {
            Destroy(_visualizationContainer);
        }
    }


    #endregion
}
