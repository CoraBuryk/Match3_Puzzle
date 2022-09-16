using UnityEngine;

namespace Assets.Match.Scripts.States
{
    public class StateMachine : MonoBehaviour
    {
		protected State _currentState;

		public virtual State CurrentState
		{
			get
			{
				return _currentState;
			}
			set
			{
				if (_currentState == value)
					return;

				if (_currentState != null)
					_currentState.Exit();

				_currentState = value;

				if (_currentState != null)
					_currentState.Enter();
			}
		}

		public virtual T GetState<T>() where T : State
		{
			T target = GetComponent<T>();
			if (target == null)
				target = gameObject.AddComponent<T>();
			return target;
		}

		public virtual void SwitchState<T>() where T : State
		{
			CurrentState = GetState<T>();
		}
	}
}
