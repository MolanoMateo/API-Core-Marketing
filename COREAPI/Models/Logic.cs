using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COREAPI.Models
{
    public class Logic
    {
        public int Id { get; set; }
        public Campana campana { get; set; }
        public Buyer buyer { get; set; }
        public List<ResultsCampana> ResultsCampana { get; set; }
    }
    
}
