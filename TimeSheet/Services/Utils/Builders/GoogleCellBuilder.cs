using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Services.Interfaces;

namespace TimeSheet.Services.Builders
{
    internal class GoogleCellBuilder : ICellBuilder<CellData>
    {
        private const string NumberFormatText = "TEXT";
        private const string NumberFormatNumber = "NUMBER";
        private const string NumberFormatPercent = "PERCENT";
        private const string NumberFormatCurrency = "CURRENCY";
        private const string NumberFormatDate = "DATE";
        private const string NumberFormatTime = "TIME";
        private const string NumberFormatDateTime = "DATE_TIME";
        private const string NumberFormatScientific = "SCIENTIFIC";

        // TODO: Think how to implement this throgh interface.
        public static GoogleCellBuilder New => new GoogleCellBuilder();

        public ICellBuilder<CellData> Cell => new GoogleCellBuilder();

        private readonly CellData _cell;
        private readonly ExtendedValue _value;
        private readonly CellFormat _format;

        public GoogleCellBuilder()
        {
            _cell = new CellData();
            _value = new ExtendedValue();
            _format = new CellFormat();

            _cell.UserEnteredValue = _value;
            _cell.UserEnteredFormat = _format;
        }

        public ICellBuilder<CellData> FromDate(DateTime date, String pattern = "dd.mm.yyyy")
        {
            _value.NumberValue = date.Date.ToOADate();
            _format.NumberFormat = new NumberFormat
            {
                Type = NumberFormatDate,
                Pattern = pattern,
            };

            return this;
        }

        public ICellBuilder<CellData> FromTime(DateTime time, String pattern = "hh:mm")
        {
            // First convert datetime value to the time only value.
            time = DateTime.Parse(time.ToShortTimeString());

            _value.NumberValue = time.ToOADate();
            _format.NumberFormat = new NumberFormat
            {
                Type = NumberFormatTime,
                Pattern = pattern,
            };

            return this;
        }

        public ICellBuilder<CellData> FromDateTime(DateTime datetime, String pattern = "")
        {
            _value.NumberValue = datetime.ToOADate();
            _format.NumberFormat = new NumberFormat
            {
                Type = NumberFormatDateTime,
                Pattern = pattern,
            };

            return this;
        }

        public ICellBuilder<CellData> FromString(String value)
        {
            _value.StringValue = value;
            return this;
        }

        public ICellBuilder<CellData> FromDouble(Double value)
        {
            _value.NumberValue = value;
            return this;
        }

        public ICellBuilder<CellData> FromInt32(int value)
        {
            throw new NotImplementedException();
        }

        public CellData Build()
        {
            return _cell;
        }

        public CellData Empty()
        {
            _cell.UserEnteredValue = null;
            _cell.UserEnteredFormat = null;
            return _cell;
        }

        // TODO: Implement visual formatting methods 
        // (which will add borders, paddings etc).
    }
}
