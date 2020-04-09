using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex = 0;


    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count-1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, 
                                             targetPosition, 
                                             movementThisFrame);
            LookAtTargetPoint(targetPosition);
            
            if(transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }
        else 
        {
            Destroy(gameObject);
        }   
    }
    
    private void LookAtTargetPoint( Vector3 targetPosition)
    {
        Vector3 direction = CalculateRotationDirection(targetPosition);
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private Vector3 CalculateRotationDirection(Vector3 targetPosition)
    {
        Vector3 direction = new Vector3();
        if (gameObject.tag == "ySym")
        {
            direction = CheckDirection(targetPosition);
        }
        else if (gameObject.tag == "xSym")
        {
            direction = targetPosition - transform.position;
        } 
        return direction;
    }

    private Vector3 CheckDirection(Vector3 targetPosition)
    {
        if(targetPosition.x > transform.position.x)
        {
            return (targetPosition - transform.position);
        }
        else
        {
            return (transform.position - targetPosition);
        }   
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

}
