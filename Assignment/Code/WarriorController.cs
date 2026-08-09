using System.Collections;
using UnityEngine;

namespace ARFightingRobot
{
	public class WarriorController:SuperStateMachine
	{
		[Header("Components")]
		public Warrior warrior;
		public GameObject target;
		public GameObject weapon;

		private Rigidbody rb;
		private FixedJoystick fixedJoystick;
		[SerializeField] private float speed = 0.01f;


		[HideInInspector] public WarriorTiming warriorTiming;
		[HideInInspector] public Animator animator;
		[HideInInspector] public IKHands ikHands;


		private bool useInputSystem;

		public bool allowedInput { get { return _allowedInput; } }
		private bool _allowedInput = true;

		// Actions.
		[HideInInspector] public bool isMoving;
		[HideInInspector] public bool useRootMotion = false;

		public bool canAction { get { return _canAction; } }
		private bool _canAction = true;

		public bool canMove { get { return _canMove; } }
		private bool _canMove = true;

		public bool canJump { get { return _canJump; } }
		private bool _canJump = true;

		public float animationSpeed = 50f;

		#region Initialization

		private void Awake()
		{

			warriorTiming = gameObject.AddComponent<WarriorTiming>();
			warriorTiming.warriorController = this;

			fixedJoystick = FindObjectOfType<FixedJoystick>();

			// Add IKHands.
			ikHands = gameObject.AddComponent<IKHands>();
			if (ikHands != null) {
				if (warrior == Warrior.TwoHanded
					|| warrior == Warrior.Hammer
					|| warrior == Warrior.Crossbow
					|| warrior == Warrior.Spearman) {
					ikHands.canBeUsed = true;
					ikHands.BlendIK(true, 0, 0.25f);
				}
			}

			animator = GetComponentInChildren<Animator>();
			if (animator == null) {
				Debug.LogError("ERROR: There is no Animator component for character.");
				Debug.Break();
			} else {
				animator.gameObject.AddComponent<WarriorCharacterAnimatorEvents>();
				animator.GetComponent<WarriorCharacterAnimatorEvents>().warriorController = this;
				animator.gameObject.AddComponent<AnimatorParentMove>();
				animator.GetComponent<AnimatorParentMove>().animator = animator;
				animator.GetComponent<AnimatorParentMove>().warriorController = this;
				animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
				animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
			}


			rb = GetComponent<Rigidbody>();
			if (rb != null) { rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; }

			currentState = WarriorState.Idle;
		}

		private void FixedUpdate()
		{
			MoveCharacter();
		}

		#endregion

		#region Input

	
		public void AllowInput(bool b)
		{
			_allowedInput = b;
		}

		#endregion

		#region Updates

		private void Update()
		{


			UpdateAnimationSpeed();
		}

		private void UpdateAnimationSpeed()
		{
			SetAnimatorFloat("Animation Speed", animationSpeed);
		}

		#endregion

		#region Combat

		public void Jump()
		{
			Debug.Log("Warrior type: " + warrior); // Kiểm tra loại warrior
			if (warrior == Warrior.TwoHanded
				|| warrior == Warrior.Hammer
				|| warrior == Warrior.Crossbow
				|| warrior == Warrior.Spearman)
			{
				if (ikHands != null)
				{
  					SetAnimatorInt("Jumping", 1);
					SetAnimatorTrigger(AnimatorTrigger.JumpTrigger);
					// Thay đổi thời gian này cho phù hợp với hoạt ảnh nhảy
					currentState = WarriorState.Idle;
					SetAnimatorInt("Jumping", 0);


				}
				else
				{
					Debug.LogError("IKHands is not initialized!");
				}
			}

		}

		public void Land()
		{
			if (warrior == Warrior.TwoHanded
					|| warrior == Warrior.Hammer
					|| warrior == Warrior.Crossbow
					|| warrior == Warrior.Spearman) {
					SetAnimatorInt("Jumping", 0);

					ikHands.BlendIK(true, 0.5f, 0.25f);
				}

		}

		/// <summary>
		/// The different attack types.
		/// </summary>
		public void Attacking()
		{
			// Tìm đối tượng 'Mesh-Hammer-Weapon'
			Transform weapon1 = transform.Find("Hammer").Find("Mesh-Hammer-Weapon").Find("Point-Attack-1");
			weapon1.GetComponent<BoxCollider>().enabled = true;
			Transform weapon2 = transform.Find("Hammer").Find("Mesh-Hammer-Weapon").Find("Point-Attack-2");
			weapon2.GetComponent<BoxCollider>().enabled = true;

			// Gọi các hàm của animator
			SetAnimatorInt("Action", 1);
			SetAnimatorTrigger(AnimatorTrigger.AttackTrigger);

			// Gọi Coroutine để vô hiệu hóa collider sau 0.5 giây
			StartCoroutine(DisableColliderAfterDelay(0.5f));
		}

		// Coroutine để vô hiệu hóa collider sau một khoảng thời gian
		private IEnumerator DisableColliderAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);

			Transform weapon1 = transform.Find("Hammer").Find("Mesh-Hammer-Weapon").Find("Point-Attack-1");
			weapon1.GetComponent<BoxCollider>().enabled = false;		
			Transform weapon2 = transform.Find("Hammer").Find("Mesh-Hammer-Weapon").Find("Point-Attack-2");
			weapon2.GetComponent<BoxCollider>().enabled = false;	
		}


		private void MoveCharacter()
		{
			// Lấy giá trị từ joystick
			float xVal = fixedJoystick.Horizontal;
			float yVal = fixedJoystick.Vertical;

			// Tạo vector di chuyển từ giá trị joystick
			Vector3 movement = (new Vector3(xVal, 0, yVal).normalized * speed)/750f;

			// Xoay và di chuyển nhân vật
			if ((xVal != 0 || yVal != 0) && _canMove)
			{
				float targetAngle = Mathf.Atan2(xVal, yVal) * Mathf.Rad2Deg;

				// Xoay nhân vật
				rb.MoveRotation(Quaternion.Euler(0, targetAngle, 0));

				// Di chuyển nhân vật
				rb.MovePosition(rb.position + movement);

				// Cập nhật Animator
				isMoving = true;
				SetAnimatorBool("Moving", true);
				SetAnimatorFloat("Velocity", movement.magnitude*1000f);
			}
			else
			{
				// Dừng di chuyển
				isMoving = false;
				SetAnimatorBool("Moving", false);
				SetAnimatorFloat("Velocity", 0);
			}
		}



		#endregion

		#region Locks

		/// <summary>
		/// Lock character movement and/or action, on a delay for a set time.
		/// </summary>
		/// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
		/// <param name="lockAction">If set to <c>true</c> lock action.</param>
		/// <param name="timed">If set to <c>true</c> timed.</param>
		/// <param name="delayTime">Delay time.</param>
		/// <param name="lockTime">Lock time.</param>
		public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
		{
			StopCoroutine("_Lock");
			StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
		}

		//Timed -1 = infinite, 0 = no, 1 = yes.
		public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
		{
			if (delayTime > 0) { yield return new WaitForSeconds(delayTime); }
			if (lockMovement) { LockMove(true); }
			if (lockAction) { LockAction(true); }
			if (timed) {
				if (lockTime > 0) {
					yield return new WaitForSeconds(lockTime);
					UnLock(lockMovement, lockAction);
				}
			}
		}

		/// <summary>
		/// Keep character from moving and use or diable Rootmotion.
		/// </summary>
		public void LockMove(bool b)
		{
			if (b) {
				SetAnimatorBool("Moving", false);
				SetAnimatorRootMotion(true);
				_canMove = false;
				// moveInput = Vector3.zero;
			} else {
				_canMove = true;
				SetAnimatorRootMotion(false);
			}
		}

		/// <summary>
		/// Keep character from doing actions.
		/// </summary>
		public void LockAction(bool b)
		{
			_canAction = !b;
		}

		/// <summary>
		/// Keep character from jumping.
		/// </summary>
		public void LockJump(bool b)
		{
			_canJump = !b;
		}

		/// <summary>
		/// Let character move and act again.
		/// </summary>
		private void UnLock(bool movement, bool actions)
		{
			if (movement) { LockMove(false); }
			if (actions) { _canAction = true; }
		}

		#endregion

		#region Misc


		public void SetAnimatorTrigger(AnimatorTrigger trigger)
		{
			//Debug.Log("SetAnimatorTrigger: " + trigger + " - " + ( int )trigger);
			animator.SetInteger("Trigger Number", ( int )trigger);
			animator.SetTrigger("Trigger");
		}

		/// <summary>
		/// Set Animator Bool.
		/// </summary>
		public void SetAnimatorBool(string name, bool b)
		{
			//Debug.Log("SetAnimatorBool: " + name + b);
			animator.SetBool(name, b);
		}

		/// <summary>
		/// Set Animator float.
		/// </summary>
		public void SetAnimatorFloat(string name, float f)
		{
			//Debug.Log("SetAnimatorFloat: " + name + f);
			animator.SetFloat(name, f);
		}

		/// <summary>
		/// Set Animator ingeter.
		/// </summary>
		public void SetAnimatorInt(string name, int i)
		{
			//Debug.Log("SetAnimatorInt: " + name + i);
			animator.SetInteger(name, i);
		}

		/// <summary>
		/// Set Animator to use root motion or not.
		/// </summary>
		public void SetAnimatorRootMotion(bool b)
		{
			useRootMotion = b;
		}



		#endregion
	}
}