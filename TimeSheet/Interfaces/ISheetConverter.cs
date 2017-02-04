using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimeSheet.Services.Interfaces
{
    public interface ISheetConverter
    {
        IEnumerable<IData> Convert(ISheetData sheet);

        Task<IEnumerable<IData>> ConvertAsync(ISheetData sheet);
    }
}