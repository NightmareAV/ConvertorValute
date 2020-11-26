using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace ConverterValutes
{
    class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<NameValuta> NameValutas { get; set; }
        public MainViewModel()
        {
            NameValutas = new ObservableCollection<NameValuta>();
            _ChoiceDate = DateTime.Now;
            LoadData();
            _MaxDate = DateTime.Now;
            _input = string.Empty;         
        }        

        private string _input;
        private string _result;
        private NameValuta _sourceSelectedValuta;
        private NameValuta _resultSelectedValuta;
        private string _ID;
        private string _NumCode;
        private string _CharCode;
        private int _Nominal;
        private string _Name;
        private float _Value;
        private float _Previous;
        private DateValuta _DateValuta;
        private DateTime _Date;
        private DateTime _ChoiceDate;
        private DateTime _MaxDate;
        private DateTime _PreviousDate;
        private string _PreviousURL;
        private Valute _Valute;
        public string ID 
        { 
            get { return _ID; } 
            set { _ID = value; OnPropertyChanged(nameof(ID)); } 
        }
        public string NumCode 
        {
            get { return _NumCode; } 
            set { _NumCode = value; OnPropertyChanged(nameof(NumCode)); } 
        }
        public string CharCode 
        { 
            get { return _CharCode; }
            set { _CharCode = value; OnPropertyChanged(nameof(CharCode)); } 
        }
        public int Nominal 
        { 
            get { return _Nominal; } 
            set { _Nominal = value; OnPropertyChanged(nameof(Nominal)); } 
        }
        public string Name 
        { 
            get { return _Name; } 
            set { _Name = value; OnPropertyChanged(nameof(Name)); } 
        }
        public float Value 
        { 
            get { return _Value; }
            set { _Value = value; OnPropertyChanged(nameof(Value)); } 
        }
        public float Previous 
        { 
            get { return _Previous; } 
            set { _Previous = value; OnPropertyChanged(nameof(Previous)); } 
        }
        public DateValuta DateValuta 
        { 
            get { return _DateValuta; } 
            set { _DateValuta = value; OnPropertyChanged(nameof(DateValuta)); }
        }
        public DateTime Date 
        { 
            get { return _Date; } 
            set { _Date = value; OnPropertyChanged(nameof(Date)); } 
        }
        public DateTime PreviousDate 
        { 
            get { return _PreviousDate; }
            set { _PreviousDate = value; OnPropertyChanged(nameof(PreviousDate)); } 
        }
        public string PreviousURL 
        {
            get { return _PreviousURL; } 
            set { _PreviousURL = value; OnPropertyChanged(nameof(PreviousURL)); } 
        }
        public Valute Valute 
        { 
            get { return _Valute; } 
            set { _Valute = value; OnPropertyChanged(nameof(Valute)); } 
        }
        public string Input
        {
            get { return _input; }
            set 
            {
                if (SourceSelectedValuta != null && ResultSelectedValuta != null && !string.IsNullOrEmpty(value))
                {
                    Result = Math.Round(((ResultSelectedValuta.Value * Convert.ToDouble(value)) / SourceSelectedValuta.Value), 4).ToString();
                    _input = value;
                }
                else
                {
                    _input = "";
                    Result = "";
                }
                OnPropertyChanged(nameof(Input)); 
            }
        }
        public DateTime ChoiceDate
        {
            get { return _ChoiceDate; }
            set { _ChoiceDate = value; LoadData(); OnPropertyChanged(nameof(ChoiceDate)); }
        }
        public DateTime MaxDate
        {
            get { return _MaxDate; }
            set { _MaxDate = value; OnPropertyChanged(nameof(MaxDate)); }
        }

        public string Result
        {
            get { return _result; }
            set 
            {
                if (SourceSelectedValuta != null && ResultSelectedValuta != null && !string.IsNullOrEmpty(Input))
                    _result = Math.Round(((ResultSelectedValuta.Value * Convert.ToDouble(Input)) / SourceSelectedValuta.Value), 4).ToString();
                else
                    _result = string.Empty;
                OnPropertyChanged(nameof(Result)); 
            }
        }
        public NameValuta SourceSelectedValuta
        {
            get {  return _sourceSelectedValuta; }
            set { _sourceSelectedValuta = value; OnPropertyChanged(nameof(SourceSelectedValuta)); }
        }
        public NameValuta ResultSelectedValuta
        {
            get { return _resultSelectedValuta; }
            set { _resultSelectedValuta = value; OnPropertyChanged(nameof(ResultSelectedValuta)); }
        }

        private async void LoadData()
        {
            string url = "";
            if (ChoiceDate.Date == DateTime.Now.Date)
                url = "https://www.cbr-xml-daily.ru/daily_json.js";
            else
                url = $"https://www.cbr-xml-daily.ru/archive/{ChoiceDate.Year}/{ChoiceDate.Month}/{ChoiceDate.Day}/daily_json.js";
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                var response = await client.GetAsync(client.BaseAddress);

                int day = ChoiceDate.Day;
                int month = ChoiceDate.Month;
                int year = ChoiceDate.Year;

                #region
                //if (response.StatusCode != System.Net.HttpStatusCode.OK)
                //{
                //    url = "https://www.cbr-xml-daily.ru/daily_json.js";
                //    HttpClient client2 = new HttpClient();
                //    client2.BaseAddress = new Uri(url);
                //    response = await client2.GetAsync(client2.BaseAddress);
                //    var prev_content = await response.Content.ReadAsStringAsync();
                //    var prev_info = JsonConvert.DeserializeObject<DateValuta>(prev_content);

                //    while (prev_info.Date > ChoiceDate.Date)
                //    {
                //        url = prev_info.PreviousURL;

                //        client.BaseAddress = new Uri(url);
                //        response = await client.GetAsync(client.BaseAddress);
                //        prev_content = await response.Content.ReadAsStringAsync();
                //        prev_info = JsonConvert.DeserializeObject<DateValuta>(prev_content);

                //        content = prev_content;
                //        info = prev_info;
                //        ChoiceDate = prev_info.Date;
                //    }
                //}
                //else
                //{
                //    content = await response.Content.ReadAsStringAsync();
                //    info = JsonConvert.DeserializeObject<DateValuta>(content);
                //}
                #endregion
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    while (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        if (day != 1)
                            day--;

                        if (month == 1 && day == 1)
                        {
                            day = 31;
                            month = 12;
                            year--;
                        }

                        if (day == 1)
                        {
                            day = 31;
                            month--;
                        }

                        url = $"https://www.cbr-xml-daily.ru/archive/{year}/{month}/{day}/daily_json.js";

                        HttpClient client2 = new HttpClient();
                        client2.BaseAddress = new Uri(url);
                        response = await client2.GetAsync(client2.BaseAddress);
                    }
                }
                var content = await response.Content.ReadAsStringAsync();
                var info = JsonConvert.DeserializeObject<DateValuta>(content);

                Date = info.Date;
                PreviousDate = info.PreviousDate;
                PreviousURL = info.PreviousURL;
                Valute = info.Valute;

                NameValutas.Clear();

                NameValuta nameValuta = new NameValuta
                {
                    ID = "R00001",
                    NumCode = "001",
                    CharCode = "RUB",
                    Nominal = 1,
                    Name = "Российский рубль",
                    Value = (float)1,
                    Previous = (float)1
                };

                NameValutas.Add(nameValuta);
                NameValutas.Add(info.Valute.AUD);
                NameValutas.Add(info.Valute.AZN);
                NameValutas.Add(info.Valute.GBP);
                NameValutas.Add(info.Valute.AMD);
                NameValutas.Add(info.Valute.BYN);
                NameValutas.Add(info.Valute.BGN);
                NameValutas.Add(info.Valute.BRL);
                NameValutas.Add(info.Valute.HUF);
                NameValutas.Add(info.Valute.HKD);
                NameValutas.Add(info.Valute.DKK);
                NameValutas.Add(info.Valute.USD);
                NameValutas.Add(info.Valute.EUR);
                NameValutas.Add(info.Valute.INR);
                NameValutas.Add(info.Valute.KZT);
                NameValutas.Add(info.Valute.CAD);
                NameValutas.Add(info.Valute.KGS);
                NameValutas.Add(info.Valute.CNY);
                NameValutas.Add(info.Valute.MDL);
                NameValutas.Add(info.Valute.NOK);
                NameValutas.Add(info.Valute.PLN);
                NameValutas.Add(info.Valute.RON);
                NameValutas.Add(info.Valute.XDR);
                NameValutas.Add(info.Valute.SGD);
                NameValutas.Add(info.Valute.TJS);
                NameValutas.Add(info.Valute.TRY);
                NameValutas.Add(info.Valute.TMT);
                NameValutas.Add(info.Valute.UZS);
                NameValutas.Add(info.Valute.UAH);
                NameValutas.Add(info.Valute.CZK);
                NameValutas.Add(info.Valute.SEK);
                NameValutas.Add(info.Valute.CHF);
                NameValutas.Add(info.Valute.ZAR);
                NameValutas.Add(info.Valute.KRW);
                NameValutas.Add(info.Valute.JPY);
            }
            catch (Exception ex)
            { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
