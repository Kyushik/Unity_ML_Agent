using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class OneLegAgent : Agent {
    public GameObject foot;
    public GameObject leg1;
    public GameObject leg2;

    public Rigidbody footRB;
    public Rigidbody leg1RB;
    public Rigidbody leg2RB;

    private Vector3 foot_init_pos;
    private Vector3 leg1_init_pos;
    private Vector3 leg2_init_pos;

    private Quaternion foot_init_rot;
    private Quaternion leg1_init_rot;
    private Quaternion leg2_init_rot;

    public override void InitializeAgent()
    {
        foot_init_pos = foot.transform.position;
        leg1_init_pos = leg1.transform.position;
        leg2_init_pos = leg2.transform.position;

        foot_init_rot = foot.transform.rotation;
        leg1_init_rot = leg1.transform.rotation;
        leg2_init_rot = leg2.transform.rotation;
    }

    public override void CollectObservations()
    {
        AddVectorObs(foot.transform.localPosition.x);
        AddVectorObs(foot.transform.localPosition.y);
        AddVectorObs(foot.transform.localRotation.z);
        AddVectorObs(footRB.velocity.x);
        AddVectorObs(footRB.velocity.y);
        AddVectorObs(footRB.angularVelocity.z);

        AddVectorObs(leg1.transform.localPosition.x);
        AddVectorObs(leg1.transform.localPosition.y);
        AddVectorObs(leg1.transform.localRotation.z);
        AddVectorObs(leg1RB.velocity.x);
        AddVectorObs(leg1RB.velocity.y);
        AddVectorObs(leg1RB.angularVelocity.z);

        AddVectorObs(leg2.transform.localPosition.x);
        AddVectorObs(leg2.transform.localPosition.y);
        AddVectorObs(leg2.transform.localRotation.z);
        AddVectorObs(leg2RB.velocity.x);
        AddVectorObs(leg2RB.velocity.y);
        AddVectorObs(leg2RB.angularVelocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        for (int k=0; k < vectorAction.Length; k++)
        {
            vectorAction[k] = Mathf.Clamp(vectorAction[k], -1f, 1f);
        }

        float torque = 20f;

        footRB.AddTorque(transform.forward * torque * vectorAction[0]);
        leg1RB.AddTorque(transform.forward * torque * vectorAction[1]);

        if (leg2.transform.position.y < 0.5)
        {
            AddReward(-1f);
            Done();
        }

        if (foot.transform.position.x > 100f)
        {
            AddReward(1f);
            Done();
        }

        AddReward(footRB.velocity.x - 0.05f * leg1RB.velocity.y);


    }

    public override void AgentReset()
    {
        foot.transform.position = foot_init_pos;
        leg1.transform.position = leg1_init_pos;
        leg2.transform.position = leg2_init_pos;

        foot.transform.rotation = foot_init_rot;
        leg1.transform.rotation = leg1_init_rot;
        leg2.transform.rotation = leg2_init_rot;
    }

    public override void AgentOnDone()
    {

    }
}
