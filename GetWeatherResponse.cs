namespace WeatherPlugin
{
    public class GetWeatherResponse
    {
        public double temperature { get; set; }
        public double windspeed { get; set; }
        public double winddirection { get; set; }
        public GetWeatherResponse() { }
        public GetWeatherResponse(
            double temperature,
            double windspeed,
            double winddirection)
        {
            this.temperature = temperature;
            this.windspeed = windspeed;
            this.winddirection = winddirection;
        }
    }
}
