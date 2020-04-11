using UnityEngine;

public class Human : MonoBehaviour
{
	public enum State
	{
		Sick,
		Recovered,
		Healthy
	}

	private State _state = State.Healthy;

	private bool IsStealth;

	[SerializeField] private float magnitude = 100f;
	[SerializeField] private float recoverTime = 5f;

	[SerializeField] private SpriteRenderer renderer;
	[SerializeField] private new Rigidbody2D rigidbody2D;

	[SerializeField] private float sickTimer;

	public State CurrentState
	{
		get => _state;
		set
		{
			_state = value;
			switch (_state)
			{
				case State.Sick:
					if (!IsStealth)
					{
						renderer.color = Color.red;
					}
					break;
				case State.Recovered:
					if (!IsStealth)
					{
						renderer.color = Color.green;
					}
					break;
				case State.Healthy:
					renderer.color = Color.white;
					break;
			}
		}
	}

	private void Start()
	{
		rigidbody2D.velocity = Random.insideUnitCircle * magnitude;
		IsStealth = Random.Range(0f, 1f) < 0.8f;
	}

	private void Update()
	{
		rigidbody2D.velocity = rigidbody2D.velocity.normalized * magnitude;

		if (CurrentState == State.Sick)
		{
			sickTimer += Time.deltaTime;
			if (sickTimer >= recoverTime) CurrentState = State.Recovered;
		}
	}

	private void Reset()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		renderer = GetComponent<SpriteRenderer>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (CurrentState == State.Sick)
		{
			var human = other.gameObject.GetComponent<Human>();
			if (human != null && human.CurrentState == State.Healthy) human.CurrentState = State.Sick;
		}
	}
}