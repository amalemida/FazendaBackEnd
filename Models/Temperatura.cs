namespace FazendaBackEnd.Models

{
    public class Temperatura
    {
        public int id { get; set; }
        public int culturaId { get; set; }
        public double? Tmax { get; set; }
        public double? Tmin { get; set; }
        public DateTime data { get; set; }

    }
}