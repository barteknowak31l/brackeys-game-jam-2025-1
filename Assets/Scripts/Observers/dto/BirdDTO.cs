using Observers.dto;
using StateMachine;

public class BirdDTO : DataTransferObject
{
    public float _damage { get; private set;}
    
    public Variant _variant { get; private set;}

    public BirdDTO Damage(float damage)
    {
        _damage = damage;
        return this;
    }

    public BirdDTO Variant(Variant variant)
    {
        _variant = variant;
        return this;
    }
}
