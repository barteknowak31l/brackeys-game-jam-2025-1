using Observers.dto;

namespace Observers
{
    public interface IObserver<T> where T : DataTransferObject
    {
        void OnNotify(T dto);
    }
}
