using Observers.dto;

public class BeaverDTO : DataTransferObject
{

    public bool _endTime { get; private set; }


  
    public BeaverDTO EndTime(bool endTime)
    {
        _endTime = endTime;
        return this;
    }

  
}
