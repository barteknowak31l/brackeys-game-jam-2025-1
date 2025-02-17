
using Observers.dto;
using StateMachine;
using StateMachine.states;

public class StateDTO : DataTransferObject
{
    public States _state {get; private set;}
    public Variant _variant {get; private set;}
    
    public bool _isDefault {get; private set;}

    public StateDTO State(States state)
    {
        _state = state;
        return this;
    }

    public StateDTO Variant(Variant variant)
    {
        _variant = variant;
        return this;
    }

    public StateDTO IsDefault(bool isDefault)
    {
        _isDefault = isDefault;
        return this;
    }
    

}
