namespace COREAPI.Models
{
    public class ResultadoApi
    {
        public string httpResponseCode { get; set; }

        public List<Usuario> LUsuario { get; set; }

        public Usuario NUsuario { get; set; }
        public List<Buyer> LBuyer { get; set; }

        public Buyer NBuyer { get; set; }
        public List<CreCampana> LCreCampana { get; set; }

        public CreCampana NCreCampana { get; set; }
        public List<KPI> LKPI { get; set; }

        public KPI NKPI { get; set; }
        public List<Logic> LLogic { get; set; }

        public Logic NLogic { get; set; }
    }
}
