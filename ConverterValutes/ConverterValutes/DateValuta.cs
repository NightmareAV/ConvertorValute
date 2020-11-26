using System;
using System.Collections.Generic;
using System.Text;

namespace ConverterValutes
{
    public class DateValuta
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public Valute Valute { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
