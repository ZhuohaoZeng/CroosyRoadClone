using UnityEngine;

public class CameraFollow_Mod : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private Vector3 cameraOffset = new Vector3(12.0f, 6.84f, -11f);

    void LateUpdate()
    {
        if(!player) 
        { 
            Debug.LogError("Camera have no player reference to follow, WTF? Assign a player");
            return;
        }
        if(player.GetComponent<PlayerController>().IsGameOver()) return;
        transform.position = player.position + cameraOffset;
    }

    

}
