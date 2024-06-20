using UnityEngine;

public class MinimapFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            Vector3 newPosition = playerTransform.position;
            newPosition.z = transform.position.z; // Maintain the minimap camera height
            transform.position = newPosition;
        }
    }
}
