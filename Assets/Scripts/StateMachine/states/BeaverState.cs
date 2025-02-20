using System.Collections;
using System.Collections.Generic;
using Lightning;
using Observers;
using Observers.dto;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace StateMachine.states
{
    public class BeaverState : Observable<BeaverDTO>, IBaseState
    {
        private Variant _variant;

        public string _playerTag = "Player";
        public GameObject beaverPrefab;
        public GameObject pigPrefab;
        public GameObject portalPrefab;
        [SerializeField] private float _beaverSpawnDistance = 10.0f;
        [SerializeField] private float _portalSpawnDistance = 15.0f;
        private Transform _playerTransform;
        private GameObject _portal;
        public int pigAmount = 0;
        private Coroutine _spawnPigsCoroutine;

        public void EnterState(StateMachineManager ctx)
        {

            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;

            if (_variant == Variant.First)
            {
                if (beaverPrefab != null && _playerTransform != null)
                {
                    Vector3 spawnPosition = new Vector3(_playerTransform.position.x + _beaverSpawnDistance, 0.605f, _playerTransform.position.z);
                    var beaver = Instantiate(beaverPrefab, spawnPosition, Quaternion.identity).GetComponent<Beaver>();

                    beaver.Setup(this);

                }
            }
            else
            {
                if (portalPrefab != null && _playerTransform != null)
                {
                    Vector3 spawnPosition = new Vector3(_playerTransform.position.x + _portalSpawnDistance , 0.405f, _playerTransform.position.z);
                    _portal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);


                }

                _spawnPigsCoroutine = StartCoroutine(SpawnPigs());


            }



        }
        private IEnumerator SpawnPigs()
        {
            yield return new WaitForSeconds(2f);

            int pigCount = 0;
            while (pigCount < pigAmount) 
            {
                if (pigPrefab != null && _playerTransform != null)
                {
                    Vector3 portalPosition = _portal.transform.position;

                    Vector3 spawnPosition = new Vector3(portalPosition.x + 1.0f, portalPosition.y, portalPosition.z);
                    var pig = Instantiate(pigPrefab, spawnPosition, Quaternion.identity).GetComponent<Pig>();
                    pig.Setup(this);
                }

                pigCount++;
                yield return new WaitForSeconds(3f); 
            }
        }


        public void ExitState(StateMachineManager ctx)
        {
          if(_variant== Variant.Second)
            {
                if (_spawnPigsCoroutine != null)
                {
                    StopCoroutine(_spawnPigsCoroutine);
                    _spawnPigsCoroutine = null; 
                }
                Animator portalAnimator = _portal.GetComponentInChildren<Animator>();
                if (portalAnimator != null)
                {
                    portalAnimator.SetTrigger("Destroy");
                }

                 Destroy(_portal,1f);

            }

        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        public States GetStateType()
        {
            return States.Beaver;
        }

        public Variant GetVariant()
        {
            return _variant;
        }
        public void UpdateState(StateMachineManager ctx)
        {


        }


        public void EndTime()
        {
           
                BeaverDTO beaverDTO = new BeaverDTO().EndTime(true);
                NotifyObservers(beaverDTO);
            
            
        }

        public void OnPlayerCollisionChange(bool playerInCollider)
        {
            BeaverDTO beaverDto = new BeaverDTO().PlayerInCollider(playerInCollider);
            NotifyObservers(beaverDto);
        }
        
        


    }
}
