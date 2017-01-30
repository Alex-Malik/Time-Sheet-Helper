﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4.Data;

namespace TimeSheet.Services.Models
{
    using Interfaces;

    internal class GoogleCell<T> : ICell<T>
    {
        public GoogleCell(CellData cell, T value)
        {
            Source = cell;
            Value  = value;

            // TODO: Adapte cells data.
        }

        public CellData Source { get; }

        public T Value { get; }

        object ICell.Value => Value;
    }
}
