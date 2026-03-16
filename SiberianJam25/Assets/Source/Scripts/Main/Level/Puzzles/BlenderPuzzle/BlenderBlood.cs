using UnityEngine;

public class BlenderBlood : MonoBehaviour
{
    [Header("Настройки прокрутки")]
    [SerializeField] private float scrollSpeed = 0.5f;
    [SerializeField] private string texturePropertyName = "_MainTex";

    private Material material;
    private float offset;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        material.SetTextureOffset(texturePropertyName, new Vector2(offset, 0));
    }
}
