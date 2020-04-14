using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class BreakoutAgent_multi : Agent {

    public GameObject agent;
    public GameObject ball;

    private Rigidbody RbAgent;
    private Rigidbody RbBall;

    private const int Stay = 0;
    private const int Right = 2;
    private const int Left = 1;

    private Vector3 ResetPos;
    private Vector3 velocity;

    private float ball_vel_z = 0f;
    private float ball_vel_z_old = 0f;

    private GameObject[] blocks_init;
    private GameObject[] blocks; 
    private int count_blocks = 0;
    private int count_blocks_old = 0;

    private float max_ball_speed = 10f;
    private float min_ball_speed = 7f;
    private float min_x_ball_speed = 0.5f;

    private Vector3 ResetPosBall;
    private Vector3 ResetPosAgent;
    private Vector3 ResetPosEnemy;

    public GameObject block1;
    public GameObject block2;
    public GameObject block3;
    public GameObject block4;
    public GameObject block5;

    private string blockTag;

    public override void InitializeAgent()
    {
        ResetPosBall = ball.transform.position;
        ResetPosAgent = agent.transform.position;

        RbAgent = agent.GetComponent<Rigidbody>();
        RbBall = ball.GetComponent<Rigidbody>();

        blockTag = block1.tag;
        blocks_init = GameObject.FindGameObjectsWithTag("block");
        blocks = GameObject.FindGameObjectsWithTag("block");
        count_blocks = CountBlocks();
        count_blocks_old = CountBlocks();
    }

    public override void CollectObservations()
    {
        AddVectorObs(agent.transform.position.x);
        AddVectorObs(ball.transform.position.x);
        AddVectorObs(RbAgent.velocity.x);
        AddVectorObs(RbBall.velocity.x);
        AddVectorObs(RbBall.velocity.z);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        RbAgent.velocity = Vector3.zero;

        int action = Mathf.FloorToInt(vectorAction[0]);
        switch (action)
        {
            case Stay:
                agent.transform.position = agent.transform.position + 0f * Vector3.right;
                break;
            case Left:
                agent.transform.position = agent.transform.position + 0.3f * Vector3.left;
                break;
            case Right:
                agent.transform.position = agent.transform.position + 0.3f * Vector3.right;
                break;
        }

        if (RbBall.velocity.x > max_ball_speed)
        {
            RbBall.velocity = new Vector3(max_ball_speed, 0, RbBall.velocity.z);
        }

        if (RbBall.velocity.x < -max_ball_speed)
        {
            RbBall.velocity = new Vector3(-max_ball_speed, 0, RbBall.velocity.z);
        }

        if (RbBall.velocity.z > 0 && RbBall.velocity.z < min_ball_speed)
        {
            RbBall.velocity = new Vector3(RbBall.velocity.x, 0, min_ball_speed);
        }

        if (RbBall.velocity.z < 0 && RbBall.velocity.z > -min_ball_speed)
        {
            RbBall.velocity = new Vector3(RbBall.velocity.x, 0, -min_ball_speed);
        }

        if (ball.transform.position.x > ResetPosAgent.x + 4.75f && Mathf.Abs(RbBall.velocity.x) < 1f)
        {
            RbBall.velocity = new Vector3(-min_ball_speed, 0, RbBall.velocity.z);
        }

        if (ball.transform.position.x < ResetPosAgent.x -4.75f && Mathf.Abs(RbBall.velocity.x) < 1f)
        {
            RbBall.velocity = new Vector3(min_ball_speed, 0, RbBall.velocity.z);
        }

        blocks = GameObject.FindGameObjectsWithTag("block");
        count_blocks = CountBlocks();

        float reward = 0f;

        if (count_blocks == 0)
        {
            DestroyBlocks();
            Done();
        }

        if (count_blocks_old > count_blocks)
        {
            AddReward(1.0f);
            reward = 1f;
        }

        if (ball.transform.position.z < ResetPosAgent.z)
        {
            DestroyBlocks();
            AddReward(-1.0f);
            reward = -1f;
            Done();
        }

        ball_vel_z = RbBall.velocity.z;

        /*
        //If agent hit the ball, reward = 0.1
        if (ball_vel_z > 0 && ball_vel_z_old < 0 && ball.transform.position.z < ResetPosBall.z - 1)
        {
            AddReward(0.1f);
        }
        */
        ball_vel_z_old = ball_vel_z;
        count_blocks_old = count_blocks;

        AddReward(0f);
    }

    public override void AgentReset()
    {
        ball.transform.position = ResetPosBall;
        agent.transform.position = ResetPosAgent;
        RbBall.velocity = Vector3.zero;
        ball.transform.rotation = Quaternion.identity;
        RbAgent.velocity = Vector3.zero;
        RbAgent.angularVelocity = Vector3.zero;

        RbBall.velocity = new Vector3(Random.Range(-0.5f*max_ball_speed, 0.5f*max_ball_speed), 0, Random.Range(-max_ball_speed, -min_ball_speed));

        DestroyBlocks();
        InitBlocks();

        blocks = GameObject.FindGameObjectsWithTag("block");
        count_blocks = CountBlocks();
        count_blocks_old = CountBlocks();
    }

    public override void AgentOnDone()
    {

    }

    public void InitBlocks()
    {
        for (float i = ResetPosAgent.x - 4.5f; i <= ResetPosAgent.x + 4.5f; i+=1.0f)
        {
            Instantiate(block1, new Vector3(i, 0.5f, ResetPosAgent.z + 5.5f), Quaternion.identity);
        }
        for (float i = ResetPosAgent.x - 4.5f; i <= ResetPosAgent.x + 4.5f; i += 1.0f)
        {
            Instantiate(block2, new Vector3(i, 0.5f, ResetPosAgent.z + 6.0f), Quaternion.identity);
        }
        for (float i = ResetPosAgent.x - 4.5f; i <= ResetPosAgent.x + 4.5f; i += 1.0f)
        {
            Instantiate(block3, new Vector3(i, 0.5f, ResetPosAgent.z + 6.5f), Quaternion.identity);
        }
        for (float i = ResetPosAgent.x - 4.5f; i <= ResetPosAgent.x + 4.5f; i += 1.0f)
        {
            Instantiate(block4, new Vector3(i, 0.5f, ResetPosAgent.z + 7.0f), Quaternion.identity);
        }
        for (float i = ResetPosAgent.x - 4.5f; i <= ResetPosAgent.x + 4.5f; i += 1.0f)
        {
            Instantiate(block5, new Vector3(i, 0.5f, ResetPosAgent.z + 7.5f), Quaternion.identity);
        }
    }

    public void DestroyBlocks()
    {
        float left_lim = ResetPosAgent.x - 5f;
        float right_lim = ResetPosAgent.x + 5f;
        float upper_lim = ResetPosAgent.z + 8f;
        float lower_lim = ResetPosAgent.z;

        blocks = GameObject.FindGameObjectsWithTag("block");
        foreach (GameObject block in blocks)
        {
            if (block.transform.position.z < upper_lim && block.transform.position.x < right_lim && block.transform.position.x > left_lim && block.transform.position.z > lower_lim)
            {
                Destroy(block.gameObject);
            }
        }
    }

    public int CountBlocks()
    {
        int count = 0;

        float left_lim = ResetPosAgent.x - 5f;
        float right_lim = ResetPosAgent.x + 5f;
        float upper_lim = ResetPosAgent.z + 8f;
        float lower_lim = ResetPosAgent.z;

        blocks = GameObject.FindGameObjectsWithTag("block");
        foreach (GameObject block in blocks)
        {
            if (block.transform.position.z < upper_lim && block.transform.position.x < right_lim && block.transform.position.x > left_lim && block.transform.position.z > lower_lim)
            {
                count += 1;
            }
        }
        return count; 
    }
}
