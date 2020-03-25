namespace Covid19API.Web
{
    public class Covid19WebBuilder
    {
        public const string APIBase = "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/";

        public Covid19WebBuilder() { }

        public string GetRecoveredCases() => $"{APIBase}time_series_covid19_confirmed_global.csv";
        public string GetConfirmedCases() => $"{APIBase}time_series_covid19_confirmed_global.csv";
        public string GetDeathCases() => $"{APIBase}time_series_covid19_confirmed_global.csv";
    }
}