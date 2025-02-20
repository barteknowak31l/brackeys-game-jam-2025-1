using System;
using System.Collections;
using StateMachine.states;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sharknado
{
    public class Sharknado : MonoBehaviour
    {
        private SharkState _ctx;
        private float _sideOffset;
        private float _speed;
        [SerializeField] private Vector3 _leftSide;
        [SerializeField] private Vector3 _rightSide;
        [SerializeField] private bool _moveLeft;

        public GameObject lightning;

        void Start()
        {
            lightning.SetActive(false);
            transform.localScale = Vector3.zero;
            StartCoroutine(ScaleOverTime(2.0f, Vector3.zero, Vector3.one));
        }
        
        
        public void Setup(SharkState ctx, float sideOffset, float speed)
        {
            _ctx = ctx;
            _sideOffset = sideOffset;
            _speed = speed;
            
            _leftSide =  transform.position - transform.up * _sideOffset;
            _rightSide =  transform.position + transform.up * _sideOffset;


            _moveLeft = Random.Range(0, 2) == 0;
            StartCoroutine(MoveToPosition(NextDirection(), _speed));
        }

        public void PrepareToDestroy()
        {
            lightning.SetActive(false);
            StartCoroutine(ScaleOverTime(2.0f, Vector3.one, Vector3.zero,true));
        }
        
        
        
        IEnumerator MoveToPosition(Vector3 targetPosition, float speed)
        {
            Vector3 startPosition = transform.position;
            float distance = Vector3.Distance(startPosition, targetPosition); 
            float duration = distance / speed; 

            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            transform.position = targetPosition;
            FinishMovement();
        }
        
        private void FinishMovement()
        {
            StartCoroutine(MoveToPosition(NextDirection(), _speed));
        }

        private Vector3 NextDirection()
        {
            _moveLeft = !_moveLeft;
             return _moveLeft ? _leftSide : _rightSide;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_ctx.PlayerTag))
            {
                _ctx.OnSharknadoHitPlayer();
            }
            
        }
        
        
        IEnumerator ScaleOverTime(float time, Vector3 startScale, Vector3 endScale, bool destroy = false)
        {
            float elapsedTime = 0f;
            while (elapsedTime < time)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = endScale;
            lightning.SetActive(true);

            if(destroy)
                Destroy(gameObject);
            
        }
        
        
        
    }
}
