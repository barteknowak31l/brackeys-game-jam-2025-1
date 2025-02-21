using System.Collections;
using System.Collections.Generic;
using Observers;
using UnityEngine;

namespace StateMachine.states
{
    public class BeaverState : Observable<BeaverDTO>, IBaseState
    {

        public string _playerTag = "Player";

        [Header("Beavers")]
        public GameObject beaverPrefab;
        [SerializeField] private float _beaverSpawnDistance = 10.0f;
        
        [Header("Pigs")]
        public GameObject pigPrefab;
        public GameObject portalPrefab;
        [SerializeField] private int pigAmount = 0;
        [SerializeField] private float _pigSpawnDistance = 15.0f;
        [SerializeField] private float _portalSpawnDistance = 15.0f;
        [SerializeField] private float _pigSpawnBaseTime = 3.0f;
        [SerializeField] private float _pigSpawnTimeRandomness = 2.0f;

        private Transform _playerTransform;
        private GameObject _portal;
        private Coroutine _spawnPigsCoroutine;
        private Beaver beaver;
        private Variant _variant;

        private List<Pig> _pigs;

        
        public void EnterState(StateMachineManager ctx)
        {
            Debug.Log("Entered BeaverState variant: " + _variant.ToString());

            _playerTransform = GameObject.FindGameObjectWithTag(_playerTag).transform;

            if (_variant == Variant.First)
            {
                if (beaverPrefab != null && _playerTransform != null)
                {
                    Vector3 spawnPosition = new Vector3(_playerTransform.position.x + _beaverSpawnDistance, 0.605f, _playerTransform.position.z);
                    beaver = Instantiate(beaverPrefab, spawnPosition, Quaternion.identity).GetComponent<Beaver>();

                    beaver.Setup(this);

                }
            }
            else if (_variant == Variant.Second)
            {
                _pigs = new List<Pig>();
                
                if (portalPrefab != null && _playerTransform != null)
                {
                    Vector3 spawnPosition = new Vector3(_playerTransform.position.x + _portalSpawnDistance, 0.405f, _playerTransform.position.z);
                    _portal = Instantiate(portalPrefab, spawnPosition, Quaternion.identity);


                }

                _spawnPigsCoroutine = StartCoroutine(SpawnPigs());


            }



        }
        private IEnumerator SpawnPigs()
        {
            yield return new WaitForSeconds(_pigSpawnBaseTime);

            int pigCount = 0;
            while (pigCount < pigAmount)
            {
                float pigSpawnDelay = _pigSpawnBaseTime * Random.Range(0.0f, _pigSpawnTimeRandomness);
                
                if (pigPrefab != null && _playerTransform != null)
                {
                    Vector3 portalPosition = _portal.transform.position;
                    Vector3 spawnPosition = new Vector3(portalPosition.x + 1.0f, portalPosition.y, portalPosition.z);
                    var pig = Instantiate(pigPrefab, spawnPosition, Quaternion.identity).GetComponent<Pig>();
                    _pigs.Add(pig);
                    pig.Setup(this);
                }
                pigCount++;
                yield return new WaitForSeconds(pigSpawnDelay);
            }
        }


        public void ExitState(StateMachineManager ctx)
        {   
            if (_variant == Variant.First)
            {
                if(beaver)
                    beaver.DestroyBeaver();
            }
            if (_variant == Variant.Second)
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

                Destroy(_portal, 1f);

                foreach (var pig in _pigs)
                {
                    Destroy(pig.gameObject);
                }
                
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

        public void RemovePigFromList(Pig pig)
        {
            _pigs.Remove(pig);
        }


    }
}
