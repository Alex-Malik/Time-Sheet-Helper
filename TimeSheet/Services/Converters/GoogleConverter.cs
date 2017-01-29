using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Converters
{
    using Interfaces;

    internal class GoogleConverter : ISheetConverter
    {
        public IEnumerable<IData> Convert(ISheetData sheet)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IData>> ConvertAsync(ISheetData sheet)
        {
            throw new NotImplementedException();
        }
    }
}
