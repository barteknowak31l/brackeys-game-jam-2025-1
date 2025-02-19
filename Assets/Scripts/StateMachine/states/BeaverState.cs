using System.Collections;
using System.Collections.Generic;
using Lightning;
using Observers;
using Observers.dto;
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
        [SerializeField] private float _pigSpawnDistance = 10.0f;
        private Transform _playerTransform;
   
        public void EnterState(StateMachineManager ctx)
        {

            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;

            if (_variant == Variant.Second)
            {
                if (beaverPrefab != null && _playerTransform != null)
                {
                    Vector3 spawnPosition = new Vector3(_playerTransform.position.x + _beaverSpawnDistance, _playerTransform.position.y, _playerTransform.position.z);
                    var beaver = Instantiate(beaverPrefab, spawnPosition, Quaternion.identity).GetComponent<Beaver>();

                    beaver.Setup(this);

                }
            }
            else
            {
                if (portalPrefab != null && _playerTransform != null)
                {
                    Vector3 spawnPosition = new Vector3(_playerTransform.position.x + _pigSpawnDistance-1f, _playerTransform.position.y, _playerTransform.position.z);
                    var portal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);
                    portal.transform.Rotate(-90, 90, 0);



                }

                StartCoroutine(SpawnPigs());


            }



        }
        private IEnumerator SpawnPigs()
        {
            yield return new WaitForSeconds(2f);

            int pigCount = 0;
            while (pigCount < 3) 
            {
                if (pigPrefab != null && _playerTransform != null)
                {
                    Vector3 spawnPosition = new Vector3(_playerTransform.position.x + _pigSpawnDistance, _playerTransform.position.y, _playerTransform.position.z);
                    var pig = Instantiate(pigPrefab, spawnPosition, Quaternion.identity).GetComponent<Pig>();
                    pig.Setup(this);
                }

                pigCount++;
                yield return new WaitForSeconds(3f); 
            }
        }


        public void ExitState(StateMachineManager ctx)
        {

          

        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }

        public States GetStateType()
        {
            return States.Fruit;
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
            if (_variant == Variant.First)
            {
                BeaverDTO beaverDTO = new BeaverDTO().EndTime(true);
                NotifyObservers(beaverDTO);
            }
        }


    }
}
