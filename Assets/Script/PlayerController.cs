using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [FormerlySerializedAs("speed")]
    [SerializeField, Min(1)] private int gridStep = 1;
    [SerializeField] private LayerMask hazardLayer;

    [Header("Map Detection")]
    [SerializeField] private GridMap gridMap;
    [SerializeField, Min(0.01f)] private float platformProbeRadius = 0.2f;
    [SerializeField, Min(0.1f)] private float platformProbeHeight = 1f;
    [SerializeField, Min(0.1f)] private float platformProbeDistance = 3f;

    private Rigidbody playerRigidbody;
    private MovingPlatform currentPlatform;
    private bool isGameOver;

    void Start()
    {
        playerTrans = transform;
        playerRigidbody = GetComponent<Rigidbody>();

        if (gridMap == null)
        {
            gridMap = FindAnyObjectByType<GridMap>();
        }

        if (gridMap == null)
        {
            Debug.LogError("PlayerController could not find GridMap.", this);
            enabled = false;
        }
    }

    void Update()
    {
        if (!isGameOver)
        {
            ReadMovementInput();
        }
    }

    private void FixedUpdate()
    {
        if (isGameOver || gridMap == null || !gridMap.IsReady) return;

        bool wasOnPlatform = currentPlatform != null;

        if (TryGetPlatformAt(playerTrans.position, out MovingPlatform platform))
        {
            currentPlatform = platform;
            MoveWithCurrentPlatform();
            return;
        }

        currentPlatform = null;
        CheckUnsupportedPosition(wasOnPlatform);
    }

    private void ReadMovementInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector2Int movement = Vector2Int.zero;

        if (keyboard.wKey.wasPressedThisFrame) movement = Vector2Int.up;
        else if (keyboard.sKey.wasPressedThisFrame) movement = Vector2Int.down;
        else if (keyboard.aKey.wasPressedThisFrame) movement = Vector2Int.left;
        else if (keyboard.dKey.wasPressedThisFrame) movement = Vector2Int.right;

        if (movement != Vector2Int.zero)
        {
            TryMove(movement);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        if (gridMap == null || !gridMap.IsReady) return;

        Vector2Int currentGrid = gridMap.WorldToGrid(playerTrans.position);
        Vector2Int targetGrid = currentGrid + direction * gridStep;

        if (!gridMap.TryGetTileType(targetGrid, out TileType tileType))
        {
            return;
        }

        Vector3 targetWorld = gridMap.GridToWorld(
            targetGrid,
            playerTrans.position.y
        );

        switch (tileType)
        {
            case TileType.Walkable:
                currentPlatform = null;
                MovePlayerTo(targetWorld);
                break;

            case TileType.Blocked:
                break;

            case TileType.Hazard:
                MoveToHazardTile(targetWorld);
                break;
        }
    }

    private void MoveToHazardTile(Vector3 targetWorld)
    {
        MovePlayerTo(targetWorld);

        if (TryGetPlatformAt(targetWorld, out MovingPlatform platform))
        {
            currentPlatform = platform;
            return;
        }

        GameOver("Player fell into the water.");
    }

    private void MovePlayerTo(Vector3 targetWorld)
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.position = targetWorld;
        }
        else
        {
            playerTrans.position = targetWorld;
        }
    }

    private void MoveWithCurrentPlatform()
    {
        Vector3 movement = currentPlatform.Velocity * Time.fixedDeltaTime;

        if (playerRigidbody != null)
        {
            playerRigidbody.MovePosition(playerRigidbody.position + movement);
        }
        else
        {
            playerTrans.position += movement;
        }
    }

    private bool TryGetPlatformAt(
        Vector3 worldPosition,
        out MovingPlatform platform)
    {
        Vector3 origin = worldPosition + Vector3.up * platformProbeHeight;
        RaycastHit[] hits = Physics.SphereCastAll(
            origin,
            platformProbeRadius,
            Vector3.down,
            platformProbeDistance,
            Physics.AllLayers,
            QueryTriggerInteraction.Ignore
        );

        foreach (RaycastHit hit in hits)
        {
            platform = hit.collider.GetComponentInParent<MovingPlatform>();

            if (platform != null)
            {
                return true;
            }
        }

        platform = null;
        return false;
    }

    private void CheckUnsupportedPosition(bool wasOnPlatform)
    {
        Vector2Int currentGrid = gridMap.WorldToGrid(playerTrans.position);

        if (gridMap.TryGetTileType(currentGrid, out TileType tileType))
        {
            if (tileType == TileType.Hazard)
            {
                GameOver("Player is no longer standing on a moving platform.");
            }

            return;
        }

        if (wasOnPlatform)
        {
            GameOver("Player was carried outside the map.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((hazardLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            GameOver("Player got hit by a hazard.");
        }
    }

    private void GameOver(string reason)
    {
        if (isGameOver) return;

        isGameOver = true;
        Destroy(this);
        Debug.Log($"{reason} Game over.");
    }
}
