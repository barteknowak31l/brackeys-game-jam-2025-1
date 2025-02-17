using System.Collections;
using Observers;
using Observers.dto;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StateMachine.states
{
    public class WindState : Observable<WindDTO>, IBaseState
    {
        [SerializeField] private Variant _variant;
    
        private float _windSpeed = 0.0f;
        [SerializeField] float _windBase = 5f;
        [SerializeField] float _windBaseSecond = 8f;
        [SerializeField] float _windOffset = 2.5f;
        [Space]
        [SerializeField] int _windDirection = 1; // 0 - left, 1 - right
        [SerializeField] private float _windChangeTimer = 7.0f;
        [SerializeField] private float _windChangeTimerVariant2 = 4.0f;

        public void EnterState(StateMachineManager ctx)
        {
            StartCoroutine(UpdateWindCoroutine());
        
            WindDTO dto = new WindDTO()
                .Enabled(true)
                .Direction(_windDirection)
                .Speed(_windSpeed);
            NotifyObservers(dto);
        
        
            Debug.Log("Enter Wind State variant:" + _variant.ToString());
        
        }

        public void UpdateState(StateMachineManager ctx)
        {
        }

        public void ExitState(StateMachineManager ctx)
        {
            StopAllCoroutines();
            WindDTO dto = new WindDTO()
                .Enabled(true)
                .Direction(_windDirection)
                .Speed(_windSpeed);
            NotifyObservers(dto);
        
            Debug.Log("Exit Wind State");
        
        }

        public IBaseState SetVariant(Variant variant)
        {
            _variant = variant;
            return this;
        }


        private void UpdateWind()
        {
            float windBase = _variant == Variant.First ? _windBase : _windBaseSecond; ;
            _windSpeed = windBase + Random.Range(-_windOffset, _windOffset);
            if (_variant == Variant.Second)
            {
                _windDirection = Random.Range(0, 1);
            }
        
            WindDTO dto = new WindDTO()
                .Enabled(true)
                .Direction(_windDirection)
                .Speed(_windSpeed);
            NotifyObservers(dto);
        
            Debug.Log("Updated wind: " + _windSpeed + " " + _windDirection);
        
        }

        private IEnumerator UpdateWindCoroutine()
        {
            while (true)
            {
                UpdateWind();
                yield return new WaitForSeconds(_variant == Variant.First ? _windChangeTimer : _windChangeTimerVariant2);   
            }
        }
    }
}
