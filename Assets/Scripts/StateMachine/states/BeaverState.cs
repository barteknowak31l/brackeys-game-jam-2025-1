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

        [SerializeField] string _playerTag = "Player";
        public GameObject beaverPrefab;
        public GameObject pigPrefab;
        [SerializeField] private float _beaverSpawnDistance = 10.0f;
        private Transform _playerTransform;
   
        public void EnterState(StateMachineManager ctx)
        {

            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;

            if (_variant == Variant.First)
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
            BeaverDTO beaverDTO = new BeaverDTO().EndTime(true);
            NotifyObservers(beaverDTO);
        }


    }
}
