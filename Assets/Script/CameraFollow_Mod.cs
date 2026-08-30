using UnityEngine;

public class CameraFollow_Mod : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Vector3 cameraOffset = new Vector3(-8.79f, 7.34f, -13.23f);

    void LateUpdate()
    {
        if(!player) 
        { 
            Debug.LogError("Camera have no player reference to follow, WTF? Assign a player");
            return;
        }
        transform.position = player.position + cameraOffset;
    }

}
