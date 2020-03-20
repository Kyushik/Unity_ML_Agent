using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PongAgent : Agent {

    public GameObject agent;
    public GameObject enemy;
    public GameObject ball;

    private Rigidbody RbAgent;
    private Rigidbody RbEnemy;
    private Rigidbody RbBall;

    private const int Stay = 0;
    private const int Up = 1;
    private const int Down = 2;

    private Vector3 ResetPos;
    private Vector3 velocity;

    private float ball_vel_z = 0f;
    private float ball_vel_z_old = 0f;

    private float max_ball_speed = 10f;
    private float min_ball_speed = 7f;

    private Vector3 ResetPosBall;
    private Vector3 ResetPosAgent;
    private Vector3 ResetPosEnemy;

    public override void InitializeAgent()
    {
        ResetPosBall = ball.transform.position;
        ResetPosAgent = agent.transform.position;
        ResetPosEnemy = enemy.transform.position;

        RbAgent = agent.GetComponent<Rigidbody>();
        RbEnemy = enemy.GetComponent<Rigidbody>();
        RbBall = ball.GetComponent<Rigidbody>();
    }

    public override void CollectObservations()
    {
        AddVectorObs(agent.transform.position.x);
        AddVectorObs(enemy.transform.position.x);
        AddVectorObs(ball.transform.position.x);
        AddVectorObs(ball.transform.position.z);
        AddVectorObs(RbAgent.velocity.x);
        AddVectorObs(RbEnemy.velocity.x);
        AddVectorObs(RbBall.velocity.x);
        AddVectorObs(RbBall.velocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
	{
        int action = Mathf.FloorToInt(vectorAction[0]);
        switch (action)
        {
            case Stay:
                agent.transform.position = agent.transform.position + 0f * Vector3.right;
                break;
            case Up:
                agent.transform.position = agent.transform.position + 0.3f * Vector3.left;
                break;
            case Down:
                agent.transform.position = agent.transform.position + 0.3f * Vector3.right;
                break;
        }

        enemy.transform.position = new Vector3(ball.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);

        if (ball.transform.position.z < -10)
        {
            AddReward(-1.0f);
            Done();
        }

        if (ball.transform.position.z > 10)
        {
            AddReward(1.0f);
            Done();
        }

        ball_vel_z = RbBall.velocity.z;

        if (ball_vel_z > 0 && ball_vel_z_old < 0)
        {
            AddReward(0.5f);
        }

        ball_vel_z_old = ball_vel_z;

        AddReward(0f);
    }

    public override void AgentReset()
    {
        ball.transform.position = ResetPosBall;
        agent.transform.position = ResetPosAgent;
        enemy.transform.position = ResetPosEnemy;
        RbBall.velocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
        RbAgent.velocity = Vector3.zero;
        RbAgent.angularVelocity = Vector3.zero;
        RbEnemy.velocity = Vector3.zero;
        RbEnemy.angularVelocity = Vector3.zero;

        float rand_num = Random.Range(-1.0f, 1.0f);

        if (rand_num < -0.5f)
        {
            velocity = new Vector3(Random.Range(min_ball_speed, max_ball_speed), 0, Random.Range(min_ball_speed, max_ball_speed));
        }
        else if (rand_num < 0f)
        {
            velocity = new Vector3(Random.Range(min_ball_speed, max_ball_speed), 0, Random.Range(-max_ball_speed, -min_ball_speed));
        }
        else if (rand_num < 0f)
        {
            velocity = new Vector3(Random.Range(-max_ball_speed, -min_ball_speed), 0, Random.Range(min_ball_speed, max_ball_speed));
        }
        else 
        {
            velocity = new Vector3(Random.Range(-max_ball_speed, -min_ball_speed), 0, Random.Range(-max_ball_speed, -min_ball_speed));
        }
        RbBall.AddForce(velocity);
    }

    public override void AgentOnDone()
    {

    }
}
