using System.Collections;
using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private float startOffset = 0;
    [SerializeField, Min(1)] private int repeats = 1;
    //[SerializeField, Min(0.01f)] private float repeatInterval = 0.3f;
    [SerializeField, Min(0.01f)] private float blockLength = 1f;
    [SerializeField, Min(0f)] private float blockGap = 2f;
    [SerializeField, Min(0.01f)] private float interval = 2f;
    [SerializeField, Min(2f)] private float blockSpeed = 2f;

    private bool shouldOverrideDirection;
    private Vector3 instanceDirection;

    public void SetSpawnDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude <= 0f)
        {
            Debug.LogWarning("Spawn direction cannot be zero.", this);
            return;
        }

        instanceDirection = direction.normalized;
        shouldOverrideDirection = true;
    }

    public void SetStartOffset(float offset)
    {
        startOffset = Mathf.Max(0f, offset);
    }

    public void SetBlockSpeed(float newSpeed)
    {
        blockSpeed = newSpeed;
    }

    public void SetRepeat(int newRepeats)
    {
        repeats = Mathf.Max(newRepeats, 2);
    }

    private IEnumerator Start()
    {
        //timer = interval + startOffset;
        if(!blockPrefab)
        {
            Debug.LogError("Hi, you forgot to set blockPrefab.", this);
            yield break;
        }
        if(startOffset > 0f) yield return new WaitForSeconds(startOffset);
        while(true)
        {
            for(int i = 0; i < repeats; ++i)
            {
                MoveModule moveModule = Generate();
                if (i < repeats - 1 && moveModule != null)
                {
                    float moveSpeed = moveModule.Velocity.magnitude;

                    float repeatInterval = (blockLength + blockGap) / Mathf.Max(moveSpeed, 0.01f);
                    yield return new WaitForSeconds(repeatInterval);
                }
            }

            // 一组 repeats 全部生成完后，再等待下一组。
            // 这个 yield 也能防止 repeats = 1 时在同一帧无限循环。
            yield return new WaitForSeconds(interval);
        }
    }

    private MoveModule Generate()
    {
        GameObject instance = Instantiate(
            blockPrefab,
            transform.position,
            Quaternion.identity
        );

        MoveModule moveModule = instance.GetComponent<MoveModule>();

        if (moveModule == null)
        {
            Debug.LogWarning(
                $"{blockPrefab.name} does not have a MoveModule component.",
                instance
            );

            return null;
        }

        if (shouldOverrideDirection)
        {
            moveModule.SetDirection(instanceDirection);
        }

        moveModule.SetSpeed(blockSpeed);

        return moveModule;
    }
}
