using System;
using StateMachine;
using StateMachine.states;
using UnityEngine;

namespace Log
{
    [RequireComponent(typeof(Rigidbody))]
    public class Log : MonoBehaviour, Observers.IObserver<BeaverDTO>, Observers.IObserver<StateDTO>
    {

        [SerializeField] private BeaverState _state;
        private Rigidbody _rb;
        private RotateObject _rotateObject;

        private Quaternion _startRotation;
        private Vector3 _startPosition;


        private void Awake()
        {
            _startRotation = transform.rotation;
            _startPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            _state.AddObserver(this);
            _rb = GetComponent<Rigidbody>();
            _rotateObject = GetComponent<RotateObject>();
            StateMachineManager.instance.AddObserver(this);
            _rb.isKinematic = true;
            _rb.useGravity = false;
            _rotateObject.startRotation = false;
            transform.rotation = _startRotation;   
            transform.localPosition = _startPosition;
            
            Debug.Log("OnEnable kloda");

        }

        private void OnDisable()
        {
            _state.RemoveObserver(this);
            StateMachineManager.instance.RemoveObserver(this);

        }


        public void OnNotify(BeaverDTO dto)
        {
            if (dto._endTime)
            {
                _rb.isKinematic = false;
                _rb.useGravity = true;
                _rotateObject.startRotation = true;
            }
        }

        public void OnNotify(StateDTO dto)
        {
            if (dto._state == States.StartState)
            {
                Debug.Log("StateDTO kloda");
                _rotateObject.startRotation = false;
                transform.rotation = _startRotation;   
                transform.localPosition = _startPosition;
            }
        }
    }
}
