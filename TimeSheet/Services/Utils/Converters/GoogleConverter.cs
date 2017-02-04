using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Converters
{
    using Interfaces;
    using Models;

    internal class GoogleConverter : ISheetConverter
    {
        public IEnumerable<IData> Convert(ISheetData sheet)
        {
            List<IData> retval = new List<IData>();
            foreach (IRow row in sheet.Rows)
            {
                foreach (ICell cell in row.Cells)
                {
                    // TODO: data models should no have nullable fields.
                    Data dataModel = new Data
                    {
                        CreatedAt = cell?.Value as DateTime?,
                        Content   = cell?.Value as String,
                        Hours     = cell?.Value as Double?,
                        Project   = cell?.Value as String,
                        StartedAt = cell?.Value as DateTime?,
                        EndedAt   = cell?.Value as DateTime?
                    };

                    if (dataModel.CreatedAt.HasValue
                        && String.IsNullOrEmpty(dataModel.Project))
                        retval.Add(dataModel);
                }
                
            }
            return retval.OrderBy(x => x.CreatedAt);
        }

        public Task<IEnumerable<IData>> ConvertAsync(ISheetData sheet)
        {
            throw new NotImplementedException();
        }
    }
}
