using UnityEngine;

public class Bush : MonoBehaviour
{
    private Vector3 _startScale;
    private Quaternion _startRotation;

    private void Start()
    {
        _startScale = transform.localScale;
        _startRotation = transform.localRotation;

        var scaleRand = Random.Range(0,2);

        transform.localScale = new Vector3(_startScale.x + scaleRand, _startScale.y + scaleRand, _startScale.z + scaleRand);

        var rotateRand = Random.Range(0,360);

        transform.localRotation = Quaternion.Euler(_startRotation.x + rotateRand, _startRotation.y + rotateRand, _startRotation.z + rotateRand);

        gameObject.isStatic = true;
    }
}
