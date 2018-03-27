using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdAgent : Agent
{
	// Variables for birds
	public GameObject bird;
	public GameObject[] columns;
	public float scoreReward = 0f;
	public float scoreReward_old = 0f;
	public bool isDead = false;

	private float y_agent = 0f;
	private float upForce = 100f;
	private Rigidbody2D rb2d;
	private Animator anim;

	public override void InitializeAgent()
	{
		rb2d = this.GetComponent<Rigidbody2D> ();
		anim = this.GetComponent<Animator> ();
	}

	public override List<float> CollectState()
	{
		int position = 0;
		List<float> state = new List<float>();
		state.Add(position);
		return state;
	}

	public override void AgentStep(float[] act)
	{
		float movement = act[0];

		y_agent = rb2d.position.y;

		if (movement == 0) {
			rb2d.position = new Vector2 (0, y_agent - 0.1f);
			anim.SetTrigger ("Flap");

//			rb2d.AddForce (new Vector2 (0, -75f));
		} else if (movement == 1) {
			rb2d.position = new Vector2 (0, y_agent + 0.1f);
//				rb2d.AddForce (new Vector2 (0, upForce));
			anim.SetTrigger ("Flap");

		} else if (movement == 2) {
			rb2d.position = new Vector2 (0, y_agent);
			anim.SetTrigger ("Flap");
		}

		if (isDead == true || rb2d.position.y > 5) {
			isDead = true;
			reward = -1f;
			done = true;
		} else {
			reward = scoreReward;
			done = false;
		}
	}


	public override void AgentReset()
	{
		rb2d.transform.position = new Vector2 (0f, 0f);
		rb2d.transform.rotation = Quaternion.identity;
		isDead = false;
		GameControl.instance.BirdScored(true);
		y_agent = 0f;

	}

	public override void AgentOnDone()
	{
		
	}

	void OnCollisionEnter2D () {
		isDead = true;
	}

	void RewardScore(float score_column){
		scoreReward = score_column;
	}

}
