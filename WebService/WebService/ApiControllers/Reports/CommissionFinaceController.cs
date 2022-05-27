using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models.Req.Reports;
using WebService.Models.Res.Reports;
using WebService.Repositories;

namespace WebService.ApiControllers.Reports
{
    [BasicAuthentication]
    public class CommissionFinaceController : ApiController
    {
        private readonly CommissionRepo commissionRepo = new CommissionRepo();
        [Route("api/v1/commissions/finance-by-office")]
        [HttpPost()]
        public CommissionReportByOfficeIDGetV21Res GetCommissionFinanceByOfficeID([FromBody] CommissionReportByOfficeIDGetV21Req req)//{"Type":"TaskType"}
        {
            try
            {
                var commission = commissionRepo.GetCommissionFinanceByOfficeID(req);
                return commission;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
                return new CommissionReportByOfficeIDGetV21Res();
            }

        }
    }
}