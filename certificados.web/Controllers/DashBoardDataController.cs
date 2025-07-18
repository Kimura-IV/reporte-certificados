using certificados.models.Entitys;
using certificados.services.Services;
using Microsoft.AspNetCore.Mvc;

namespace certificados.web.Controllers
{
    [ApiController]
    [Route("api/dashboard/")]
    public class DashBoardDataController : Controller
    {
        private readonly DashboardService dashboardService;

        public DashBoardDataController(DashboardService dashboardService) { 
        
            this.dashboardService = dashboardService;
        }

        [HttpGet("all")]
        public ResponseApp getData() { 
        
            return dashboardService.getDataDashboard();
        }

    }
}
