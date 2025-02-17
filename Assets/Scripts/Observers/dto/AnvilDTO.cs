using Observers.dto;

public class AnvilDTO : DataTransferObject
{
    public float _damage {get; private set;}


    public AnvilDTO Damage(float damage)
    {
        _damage = damage;
        return this;
    }
    
}
