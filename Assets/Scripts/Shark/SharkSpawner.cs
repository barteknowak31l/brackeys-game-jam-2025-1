using System.Collections;
using StateMachine.states;
using UnityEngine;

namespace Shark
{
    public class SharkSpawner : MonoBehaviour
    {
        private SharkState _ctx;
        private float _spawnDelay;
        private Vector3 _leftSide;
        private Vector3 _rightSide;
        private GameObject _shark;
        private bool _spawnLeft;
        private float _height;
        private float _duration;

        public void Setup(SharkState ctx, float spawnDelay, float sideOffset, GameObject shark, float height, float duration)
        {
            _ctx = ctx;
            _spawnDelay = spawnDelay;
            _leftSide =  transform.position - transform.right * sideOffset;
            _rightSide =  transform.position + transform.right * sideOffset;

            _shark = shark;
            _duration = duration;
            _height = height;
            
            _spawnLeft = Random.Range(0, 2) == 0;
            StartCoroutine(SpawnSharkCoroutine());
        }
        
        IEnumerator SpawnSharkCoroutine()
        {
            while (true)
            {
                CreateShark();
                yield return new WaitForSeconds(_spawnDelay);
                _spawnLeft = !_spawnLeft;                
            }
        }

        private void CreateShark()
        {
            var pos = _spawnLeft ? _leftSide : _rightSide;
            var dest = _spawnLeft ? _rightSide : _leftSide;
            Shark shark = Instantiate(_shark, pos, Quaternion.identity).GetComponent<Shark>();
            shark.gameObject.SetActive(true);
            shark.Setup(_ctx,pos, dest, _height, _duration, _spawnLeft);
        }
    }
}
