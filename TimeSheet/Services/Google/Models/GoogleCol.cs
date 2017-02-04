using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;

namespace TimeSheet.Services.Models
{
    using Interfaces;

    internal class GoogleCol : ICol
    {
        public GoogleCol()
        {

        }

        public IEnumerable<ICell> Cells
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
