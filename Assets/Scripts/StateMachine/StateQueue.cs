using System.Collections.Generic;
using System.Linq;
using StateMachine.states;
using UnityEngine;

namespace StateMachine
{
    public class StateQueue
    {
        private Queue<IBaseState> _queue;
        private List<IBaseState> _allStates;
        private List<int> _unvisitedStates;
        
        private int _stateCounter = 0;
        private int _statesCount;

        private Variant _variant;

        public StateQueue(List<IBaseState> allStates)
        {
          
           _allStates = allStates;
           _statesCount = allStates.Count;
            
          _queue = new Queue<IBaseState>();
          _variant = Variant.First; 
          CreateUnvisitedStates();

          _stateCounter = 0;

          // get 3 states ahead
          AddRandomState();
          AddRandomState();
          AddRandomState();
        }

        private void CreateUnvisitedStates()
        {
            _unvisitedStates = new List<int>();
            for (int i = 0; i < _allStates.Count; i++) _unvisitedStates.Add(i);
        }
        

        // find state that hasn't been selected yet and add new one to the queue
        private IBaseState GetRandomUnvisitedState()
        {
            int rnd = Random.Range(0, _unvisitedStates.Count);
            IBaseState state = _allStates[_unvisitedStates[rnd]];
            _unvisitedStates.RemoveAt(rnd);

            if (_unvisitedStates.Count == 0)
            {
                CreateUnvisitedStates();
            }
            
            return state;
        }

        private void AddRandomState()
        {
            _queue.Enqueue(GetRandomUnvisitedState());
        }

        public IBaseState NextState()
        {
            AddRandomState();
            _stateCounter += 1;
            if (_stateCounter > _statesCount)
            {
                _variant = Variant.Second;
            }
            return _queue.Dequeue().SetVariant(_variant);
        }

        public IBaseState Peek()
        {
            return _queue.Peek().SetVariant(_variant);
        }
        
    }
}
