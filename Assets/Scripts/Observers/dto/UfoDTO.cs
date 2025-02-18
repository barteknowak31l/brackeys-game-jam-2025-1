namespace Observers.dto
{
    public class UfoDTO : DataTransferObject
    {
        public bool _playerInBeam { get; private set; } = false;

        public UfoDTO PlayerInBeam(bool playerInBeam)
        {
            _playerInBeam = playerInBeam;
            return this;
        }
        
        
    }
}