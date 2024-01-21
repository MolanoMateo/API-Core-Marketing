namespace COREAPI.Models
{
    public class Campana
    {
        public int Id { get; set; }
        public string Objetivo { get; set; }
        public string Canal { get; set; }
        public float Presupuesto { get; set; }
        public string Frecuencia { get; set; }
        public int BuyerID { get; set; }
        public int UserID { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaVencimiento { get; set; }

    }
}
