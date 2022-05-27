using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebService.Models.Res.CustomerAmls;
using WebService.Models.Req.CustomerAmls;
using WebService.Services;

namespace WebService.ApiControllers.CustomerAmls
{
    [BasicAuthentication]
    public class CustomerAMLController : ApiController
    {
        [Route("api/v1/watchlist-aml-screening")]
        [HttpPost()]
        public WatchlistAmlRes WatchlistAMLScreening([FromUri] string p, [FromUri] string msgid, [FromBody] WatchlistAmlReq watchlist)
        {
            var wtchlistRes = new WatchlistAmlRes();
            #region get watchlist of each customer
            try
            {
                var watchlistAml = WatchlistAmlScreeningService.GetWatchlistAML(watchlist.CID);
                if(watchlistAml != null)
                {
                    var customerAML = new CustomerAmlReq();
                    customerAML.LoanAppID = watchlist.LoanAppID;
                    customerAML.LoanAppPersonID = watchlist.LoanAppPersonID;
                    customerAML.CreateBy = "";

                    customerAML.BlockStatus = watchlistAml.BlockStatus;
                    customerAML.WatchListScreeningStatus = watchlistAml.WatchListScreeningStatus;
                    customerAML.WatchListCaseUrl = watchlistAml.WatchListCaseUrl;
                    customerAML.RiskProfiling = watchlistAml.WatchListCaseUrl;
                    customerAML.AMLApprovalStatus = watchlistAml.AMLApprovalStatus;
                    customerAML.WatchListExposition = watchlistAml.WatchListExposition;
                    customerAML.ProductAndService = watchlistAml.ProductAndService;

                    AddLoanAppPersonAMLService.AddWatchlistAMLCreation(customerAML);

                    wtchlistRes.Status = 200;
                    wtchlistRes.Message = "Get watch list is successfully";
                    wtchlistRes.WatchlistAml = watchlistAml;
                }
            }
            catch (Exception ex)
            {
                wtchlistRes.Status = 500;
                wtchlistRes.WatchlistAml = null;
                wtchlistRes.Message = $"Internal Server error: {ex.Message}";
            }
            #endregion get watchlist of each customer

            return wtchlistRes;
        }
    }
}