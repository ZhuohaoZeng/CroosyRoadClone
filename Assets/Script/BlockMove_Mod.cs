using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MoveModule : MonoBehaviour
{
    public float speed = 3;
    public Vector3 direction = new Vector3(0, 0, 0);
    [SerializeField] private float lifeCycle = 3;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lifeCycle <= 0) Destroy(this.gameObject);
        lifeCycle -= Time.deltaTime;
        Move();
    }

    void Move()
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }
}
