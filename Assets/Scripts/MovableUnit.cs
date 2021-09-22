using UnityEngine;

public class MovableUnit : MonoBehaviour
{
    protected const float SpeedUnattacked = 3f;
    protected const float SpeedAttacked = 1f;

    protected Vector3 destination;
    protected bool movingEnabled;
    protected float speed;
    
    protected virtual void Start()
    {
        speed = SpeedUnattacked;
        destination = transform.position;
    }
    
    protected void FixedUpdate()
    {
        if (movingEnabled) Move();
    }

    private void Move()
    {
        var toEnemyDirection = destination - transform.position;
        if (toEnemyDirection.magnitude < 0.05f)
        {
            transform.position = destination;
            movingEnabled = false;
        }
        
        var movement = toEnemyDirection.normalized * speed * Time.deltaTime;
        transform.position += movement;
    }

    protected virtual void OnMoveWarriors(Vector3 destination)
    {
        this.destination = destination;
        movingEnabled = true;
    }
}
