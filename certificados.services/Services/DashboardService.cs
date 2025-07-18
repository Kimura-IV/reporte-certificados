using certificados.dal.DataAccess;
using certificados.models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace certificados.services.Services
{
    public class DashboardService
    {
        private readonly DashboardDA dashboardDA;
        public DashboardService(DashboardDA dashboardDA)
        {
            this.dashboardDA = dashboardDA;
        }

        public ResponseApp getDataDashboard() { 
        
            return dashboardDA.getDataDashboard();
        }
    }
}
