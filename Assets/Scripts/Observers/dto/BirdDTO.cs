using Observers.dto;

public class BirdDTO : DataTransferObject
{
    public float _damage { get; private set;}

    public BirdDTO Damage(float damage)
    {
        _damage = damage;
        return this;
    }
}
