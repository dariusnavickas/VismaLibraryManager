using System.Collections.Generic;

namespace BusinessLayer
{
    public interface IData
    {
        List<Book> Load();

        void Save(List<Book> books);
    }
}
