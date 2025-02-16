using Observers.dto;

namespace Observers
{
    public interface IObserver
    {
        void OnNotify<T>(T dto) where T : DataTransferObject;
    }
}
