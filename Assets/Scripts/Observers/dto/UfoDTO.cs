namespace Observers.dto
{
    public class UfoDTO : DataTransferObject
    {
        public bool _playerInBeam { get; private set; } = false;
        public bool _cowHit { get; private set; } = false;

        public UfoDTO PlayerInBeam(bool playerInBeam)
        {
            _playerInBeam = playerInBeam;
            return this;
        }

        public UfoDTO CowHit(bool cowHit)
        {
            _cowHit = cowHit;
            return this;
        }
        
        
    }
}