using System.Collections.Generic;

namespace MessagingService
{
    public interface IRepository<T>
    {
        List<T> Find(T filter);
        T FindOne(T filter);
        T Save(T item);
    }
}