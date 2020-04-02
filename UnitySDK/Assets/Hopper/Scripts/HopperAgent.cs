using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class HopperAgent : Agent {

    public GameObject foot;
    public GameObject leg1;
    public GameObject leg2;

    private Rigidbody foot_rBody;
    private Rigidbody leg1_rBody;
    private Rigidbody leg2_rBody;

    private Vector3 foot_init_pos;
    private Quaternion foot_init_rot;
    private Vector3 leg1_init_pos;
    private Quaternion leg1_init_rot;
    private Vector3 leg2_init_pos;
    private Quaternion leg2_init_rot;

    public override void InitializeAgent()
    {
        foot_init_pos = foot.transform.position;
        foot_init_rot = foot.transform.rotation;
        leg1_init_pos = leg1.transform.position;
        leg1_init_rot = leg1.transform.rotation;
        leg2_init_pos = leg2.transform.position;
        leg2_init_rot = leg2.transform.rotation;

        foot_rBody = foot.GetComponent<Rigidbody>();
        leg1_rBody = leg1.GetComponent<Rigidbody>();
        leg2_rBody = leg2.GetComponent<Rigidbody>();
    }

    public override void CollectObservations()
    {
        AddVectorObs(foot.transform.position.x / 50f);
        AddVectorObs(foot.transform.localPosition.x);
        AddVectorObs(foot.transform.localPosition.y);
        AddVectorObs(foot.transform.localRotation.z);
        AddVectorObs(foot_rBody.velocity.x);
        AddVectorObs(foot_rBody.velocity.y);
        AddVectorObs(foot_rBody.angularVelocity.z);

        AddVectorObs(leg1.transform.localPosition.x);
        AddVectorObs(leg1.transform.localPosition.y);
        AddVectorObs(leg1.transform.localRotation.z);
        AddVectorObs(leg1_rBody.velocity.x);
        AddVectorObs(leg1_rBody.velocity.y);
        AddVectorObs(leg1_rBody.angularVelocity.z);

        AddVectorObs(leg2.transform.localPosition.x);
        AddVectorObs(leg2.transform.localPosition.y);
        AddVectorObs(leg2.transform.localRotation.z);
        AddVectorObs(leg2_rBody.velocity.x);
        AddVectorObs(leg2_rBody.velocity.y);
        AddVectorObs(leg2_rBody.angularVelocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
	{
        for (int k = 0; k < vectorAction.Length; k++)
        {
            vectorAction[k] = Mathf.Clamp(vectorAction[k], -1f, 1f);
        }

        float torque = 80f;

        foot_rBody.AddTorque(transform.forward * torque * vectorAction[0]);
        leg1_rBody.AddTorque(transform.forward * torque * vectorAction[1]);
        leg2_rBody.AddTorque(transform.forward * torque * vectorAction[2]);

        if (leg2.transform.position.y < 0.8f)
        {
            AddReward(-1f);
            Done();
        }

        if (foot.transform.position.x > 50f)
        {
            AddReward(1f);
            Done();
        }

        AddReward(0.01f*(foot_rBody.velocity.x) + 0.00001f*(foot.transform.position.x / 50f));
    }

    public override void AgentReset()
    {
        foot.transform.position = foot_init_pos;
        foot.transform.rotation = foot_init_rot;
        leg1.transform.position = leg1_init_pos;
        leg1.transform.rotation = leg1_init_rot;
        leg2.transform.position = leg2_init_pos;
        leg2.transform.rotation = leg2_init_rot;
    }

    public override void AgentOnDone()
    {

    }
}
