using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Цель, за которой будет следовать камера (игрок)
    public Vector3 offset = new Vector3(0, 0, -10); // Смещение камеры

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset; // Камера следует за игроком
        }
    }
}
