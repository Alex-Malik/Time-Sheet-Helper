using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services.Interfaces
{
    // TODO: Consider public declaration.
    internal interface ICellBuilder<T>
    {
        ICellBuilder<T> Cell { get; }

        T Build();
        T Empty();

        ICellBuilder<T> FromDate(DateTime date, String pattern);
        ICellBuilder<T> FromTime(DateTime time, String pattern);
        ICellBuilder<T> FromDateTime(DateTime datetime, String pattern);
        ICellBuilder<T> FromDouble(Double value);
        ICellBuilder<T> FromInt32(Int32 value);
        ICellBuilder<T> FromString(String value);

        // TODO: Implement visual formatting methods 
        // (which will add borders, paddings etc).
    }
}
