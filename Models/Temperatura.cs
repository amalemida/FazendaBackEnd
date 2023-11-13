namespace FazendaBackEnd.Models

{
    public class Temperatura
    {
        public int id { get; set; }
        public int sensorid { get; set; }
        public string? Tmax { get; set; }
        public string? Tmin { get; set; }
        public DateTime data { get; set; } = DateTime.Now;

    }
}