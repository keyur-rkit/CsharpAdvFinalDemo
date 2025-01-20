using API.Models.Enum;
using API.Models;

namespace API.BL.Interface
{
    public interface IDataHandler<T> where T : class
    {
        EnmType Type { get; set; }

        void PreSave(T objDTO);

        Response Validation();

        Response Save();
    }
}