using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private GameObject RedCube, GreenCube;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-2f, +2f), 1f, Random.Range(-4.5f, 4.5f));
        targetTransform.localPosition = new Vector3(Random.Range(-2f, +2f), 1f, Random.Range(-4.5f, 4.5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float distance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(distance);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }


    private void OnTriggerEnter(Collider other)
    {


        if (other.TryGetComponent<Target>(out Target target))
        {
            GreenCube.SetActive(true);
            RedCube.SetActive(false);
            SetReward(+50f);
            EndEpisode();
        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            GreenCube.SetActive(false);
            RedCube.SetActive(true);
            float distance = Vector3.Distance(transform.localPosition, targetTransform.localPosition);
            SetReward(-1 * distance);

            EndEpisode();
        }


    }


}
 