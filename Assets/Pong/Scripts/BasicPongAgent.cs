using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPongAgent : Agent
{

	public float position;
	public bool is_inference;

	public Vector3 position_agent = new Vector3(0,0,0);
	public Vector3 position_enemy = new Vector3(0,0,0);

	public GameObject ball;
	public GameObject enemy;

	public Vector3 ballPosition = new Vector3(0,0,0);

	private float enemy_delay = 0f;

	private float bar_speed = 2f;
	private float ball_x_speed = 1.5f;
	private float ball_y_speed = 0f;

	private float ball_y_speed_range_min = 0.5f;
	private float ball_y_speed_range_max = 2.5f;

	private float ball_direction_x = 1f;
	private float ball_direction_y = 1f;

	private bool is_hit_ball = false;
	private bool is_lose = false;
	private bool is_win = false;

	// If agent hits the ball over the max_hit, agent wins!!
	private int count_hit = 0;
	private int max_hit = 7;

	private float bar_range = 50f;

	public override List<float> CollectState()
	{
		List<float> state = new List<float>();
		state.Add(position);
		return state;
	}

	public override void AgentStep(float[] act)
	{
		float movement = act[0];

		// Actions: 
		//         0 (s) -> Stop
		//         1 (a) -> left
		//         2 (d) -> right

		if ((movement == 1) && (this.transform.position.z < bar_range)) {
			this.transform.Translate (Vector3.forward * bar_speed);
		} else if ((movement == 2) && (this.transform.position.z > -bar_range)) {
			this.transform.Translate (Vector3.forward * -bar_speed);
		} else if (movement == 0) {
			this.transform.Translate (Vector3.forward * 0);
		}

		is_hit_ball = ball_control ();

		enemy_control ();

		is_lose = check_lose ();
		is_win = check_win ();

		// Reward
		// if bar hits the ball, get reward +1
		// if agent lose, get penalty -1

		if (is_hit_ball)
		{
			reward = 1f;
			is_hit_ball = false;
		}

		if (is_lose)
		{
			done = true;
			reward = -1f;
			is_lose = false;
		}

		if (is_win)
		{
			done = true;
			reward = 1f;
			is_win = false;
		}

	}

	public bool ball_control() 
	{
		ball.transform.Translate (Vector3.left * ball_x_speed);
		ball.transform.Translate (Vector3.forward * ball_y_speed);

		float agent_length = this.transform.localScale.z;

		// If ball hits the wall
		if ((ball.transform.position.z < -62.5) || (ball.transform.position.z > 62.5)) {
			ball_y_speed = -ball_y_speed;
		}

		// If agent hits the ball

		if ((ball.transform.position.x - 2 > this.transform.position.x) && 
			(ball.transform.position.x - 2 < this.transform.position.x + 2.5) &&
			(ball.transform.position.z + 2 > this.transform.position.z - agent_length/2) && 
			(ball.transform.position.z - 2 < this.transform.position.z + agent_length/2)) {
			ball_x_speed = -ball_x_speed;
			count_hit += 1;
			return true;
		}

		// If enemy hits the ball
		float enemy_length = enemy.transform.localScale.z;
		if ((ball.transform.position.x + 2 < enemy.transform.position.x) && 
			(ball.transform.position.x + 2 > enemy.transform.position.x - 2.5) &&
			(ball.transform.position.z + 2 > enemy.transform.position.z - (enemy_length/2)) && 
			(ball.transform.position.z - 2 < enemy.transform.position.z + (enemy_length/2))) {
			ball_x_speed = -ball_x_speed;
		}

		return false;
	}
		
	public void enemy_control() {

		if (is_inference == false) { 
			if (enemy.transform.position.z < ball.transform.position.z &&
			    enemy.transform.position.z < bar_range) {
				enemy.transform.Translate (Vector3.forward * (Mathf.Abs (ball_y_speed) - enemy_delay));
			} else if (enemy.transform.position.z > ball.transform.position.z &&
			           enemy.transform.position.z > -bar_range) {
				enemy.transform.Translate (Vector3.forward * -(Mathf.Abs (ball_y_speed) - enemy_delay));
			}
		} else {
			if (Input.GetKey (KeyCode.UpArrow) && enemy.transform.position.z < bar_range) {
				enemy.transform.Translate (Vector3.forward * bar_speed);
			} else if (Input.GetKey (KeyCode.DownArrow) && enemy.transform.position.z > -bar_range) {
				enemy.transform.Translate (Vector3.forward * -bar_speed);
			} else {
				enemy.transform.Translate (Vector3.forward * 0);
			}
		}
				
			
	}

	public bool check_lose() {
		if (ball.transform.position.x - 2.5 < -95) {
			return true;
		}

		return false;
	}

	public bool check_win() {
		if (ball.transform.position.x + 2.5 > 95 || count_hit == 5) {
			return true;
		}

		return false;
	}

	public override void AgentReset()
	{
		// Initialize position
		ball.transform.position = new Vector3(0f, 0f, 0f);
		this.transform.position = new Vector3(-85f, 5f, 0f);
		enemy.transform.position = new Vector3(85f, 5f, 0f);

		// Initialize ball speed
		ball_y_speed = Random.Range (ball_y_speed_range_min, ball_y_speed_range_max);

		// Set random direction of the ball
		float random_x = Random.Range(-1, 1);
		float random_y = Random.Range(-1, 1);

		if (random_x < 0) {
			ball_x_speed = -ball_x_speed;
		}

		if (random_y < 0) {
			ball_y_speed = -ball_y_speed;
		}

		// Initialize count_hit
		count_hit = 0;

	}

	public override void AgentOnDone()
	{

	}
}
