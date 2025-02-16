namespace Observers.dto
{
    public class WindDTO : DataTransferObject
    {
        public bool _enabled {get; private set;}
        public int _direction {get; private set;}
        public float _speed {get; private set;}


        public WindDTO Enabled(bool enabled)
        {
            _enabled = enabled;
            return this;
        }

        public WindDTO Direction(int direction)
        {
            _direction = direction;
            return this;
        }

        public WindDTO Speed(float speed)
        {
            _speed = speed;
            return this;
        }
    
    }
}
