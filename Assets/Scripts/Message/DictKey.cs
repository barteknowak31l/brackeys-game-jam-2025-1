using StateMachine;
using StateMachine.states;
using System;
using UnityEngine;

public class DictKey
{
    public States State;
    public Variant Variant;

    public DictKey(States state, Variant variant)
    {
        this.State = state;
        this.Variant = variant;
    }
    public override bool Equals(object obj)
    {
        if (obj is DictKey other)
        {
            return State == other.State && Variant == other.Variant;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(State, Variant);
    }
}
