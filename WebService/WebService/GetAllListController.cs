using System;
using System.Collections.Generic;
using System.Web.Http;

namespace WebService
{
    [BasicAuthentication]
    public class GetAllListController : ApiController
    {
        // GET api/<controller>
        public JsonResponse<ListModel> Get(string api_name, string api_key)
        {
            var response = new JsonResponse<ListModel>();
            var list = new ListModel();
            Class1 c = new Class1();
            string ERR = "Succeed", SMS = "";
            List<ListModel> RSData = new List<ListModel>();
            try
            {
                #region api
                string[] CheckApi = c.CheckApi(api_name, api_key);
                ERR = CheckApi[0];
                SMS = CheckApi[1];
                #endregion api

                #region data
                if (ERR != "Error")
                {
                    ListModel ListHeader = new ListModel
                    {
                        ERR = ERR,
                        SMS = SMS,
                        OverdueType = new List<OverdueType>(),
                        MainReason = new List<MainReason>(),
                        CustomerRating = new List<CustomerRating>(),
                        ManagementAction = new List<ManagementAction>(),
                        AccuracyOfUseCredit = new List<AccuracyOfUseCredit>(),
                        StatusOfSolutions = new List<StatusOfSolutions>(),
                        PromisedAmountCurrency = new List<PromisedAmountCurrency>(),
                        SourceOfMoneyPaid = new List<SourceOfMoneyPaid>(),
                        CustomerAttitude = new List<CustomerAttitude>(),
                        SourceofIncome = new List<SourceofIncome>(),
                        GuarantorCollateral = new List<GuarantorCollateral>(),
                        DebtStatus = new List<DebtStatus>(),
                        FamilyStatus = new List<FamilyStatus>()
                    };


                    var overdueType = ListData.OverdueType();
                    for (int i = 0; i < overdueType.Count; i++)
                    {
                        OverdueType o = new OverdueType();
                        o.text = overdueType[i].Text;
                        o.value = Convert.ToInt32(overdueType[i].Value);
                        ListHeader.OverdueType.Add(o);
                    }

                    var mainReason = ListData.MainReason();
                    for (int i = 0; i < mainReason.Count; i++)
                    {
                        MainReason m = new MainReason();
                        m.text = mainReason[i].Text;
                        m.value = Convert.ToInt32(mainReason[i].Value);
                        if (i == 0)
                        {
                            var businessProblem = ListData.BusinessProblem();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < businessProblem.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = businessProblem[j].Text;
                                r.value = Convert.ToInt32(businessProblem[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }
                        if (i == 1)
                        {
                            var indebtedness = ListData.Indebtedness();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < indebtedness.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = indebtedness[j].Text;
                                r.value = Convert.ToInt32(indebtedness[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }
                        if (i == 2)
                        {
                            var shareCredit = ListData.ShareCredit();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < shareCredit.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = shareCredit[j].Text;
                                r.value = Convert.ToInt32(shareCredit[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }
                        if (i == 3)
                        {
                            var dangerHealthProblems = ListData.DangerHealthProblems();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < dangerHealthProblems.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = dangerHealthProblems[j].Text;
                                r.value = Convert.ToInt32(dangerHealthProblems[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }
                        if (i == 4)
                        {
                            var familyProblems = ListData.FamilyProblems();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < familyProblems.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = familyProblems[j].Text;
                                r.value = Convert.ToInt32(familyProblems[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }
                        if (i == 5)
                        {
                            var convicted = ListData.Convicted();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < convicted.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = convicted[j].Text;
                                r.value = Convert.ToInt32(convicted[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }
                        if (i == 6)
                        {
                            var aMKStaffError = ListData.AMKStaffError();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < aMKStaffError.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = aMKStaffError[j].Text;
                                r.value = Convert.ToInt32(aMKStaffError[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }
                        if (i == 7)
                        {
                            var notWillingToPayBack = ListData.NotWillingToPayBack();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < notWillingToPayBack.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = notWillingToPayBack[j].Text;
                                r.value = Convert.ToInt32(notWillingToPayBack[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }
                        if (i == 8)
                        {
                            var shareLateIssuesFromBusinessPartnersNotYet = ListData.ShareLateIssuesFromBusinessPartnersNotYet();
                            List<Reason> Reason = new List<Reason>();
                            for (int j = 0; j < shareLateIssuesFromBusinessPartnersNotYet.Count; j++)
                            {
                                Reason r = new Reason();
                                r.text = shareLateIssuesFromBusinessPartnersNotYet[j].Text;
                                r.value = Convert.ToInt32(shareLateIssuesFromBusinessPartnersNotYet[j].Value);
                                Reason.Add(r);
                            }
                            m.reason = Reason;
                        }


                        ListHeader.MainReason.Add(m);
                    }

                    var customerRating = ListData.CustomerRating();
                    for (int i = 0; i < customerRating.Count; i++)
                    {
                        CustomerRating cr = new CustomerRating();
                        cr.text = customerRating[i].Text;
                        cr.value = Convert.ToInt32(customerRating[i].Value);
                        ListHeader.CustomerRating.Add(cr);
                    }

                    var managementAction = ListData.ManagementAction();
                    for (int i = 0; i < managementAction.Count; i++)
                    {
                        ManagementAction m = new ManagementAction();
                        m.text = managementAction[i].Text;
                        m.value = Convert.ToInt32(managementAction[i].Value);
                        ListHeader.ManagementAction.Add(m);
                    }

                    var accuracyOfUseCredit = ListData.AccuracyOfUseCredit();
                    for (int i = 0; i < accuracyOfUseCredit.Count; i++)
                    {
                        AccuracyOfUseCredit a = new AccuracyOfUseCredit();
                        a.text = accuracyOfUseCredit[i].Text;
                        a.value = Convert.ToInt32(accuracyOfUseCredit[i].Value);
                        ListHeader.AccuracyOfUseCredit.Add(a);
                    }

                    var statusOfSolutions = ListData.StatusOfSolutions();
                    for (int i = 0; i < statusOfSolutions.Count; i++)
                    {
                        StatusOfSolutions s = new StatusOfSolutions();
                        s.text = statusOfSolutions[i].Text;
                        s.value = Convert.ToInt32(statusOfSolutions[i].Value);
                        ListHeader.StatusOfSolutions.Add(s);
                    }

                    var promisedAmountCurrency = ListData.PromisedAmountCurrency();
                    for (int i = 0; i < promisedAmountCurrency.Count; i++)
                    {
                        PromisedAmountCurrency p = new PromisedAmountCurrency();
                        p.text = promisedAmountCurrency[i].Text;
                        p.value = Convert.ToInt32(promisedAmountCurrency[i].Value);
                        ListHeader.PromisedAmountCurrency.Add(p);
                    }

                    var sourceOfMoneyPaid = ListData.SourceOfMoneyPaid();
                    for (int i = 0; i < sourceOfMoneyPaid.Count; i++)
                    {
                        SourceOfMoneyPaid s = new SourceOfMoneyPaid();
                        s.text = sourceOfMoneyPaid[i].Text;
                        s.value = Convert.ToInt32(sourceOfMoneyPaid[i].Value);
                        ListHeader.SourceOfMoneyPaid.Add(s);
                    }

                    var customerAttitude = ListData.CustomerAttitude();
                    for (int i = 0; i < customerAttitude.Count; i++)
                    {
                        CustomerAttitude ca = new CustomerAttitude();
                        ca.text = customerAttitude[i].Text;
                        ca.value = Convert.ToInt32(customerAttitude[i].Value);
                        ListHeader.CustomerAttitude.Add(ca);
                    }

                    var sourceofIncome = ListData.SourceofIncome();
                    for (int i = 0; i < sourceofIncome.Count; i++)
                    {
                        SourceofIncome s = new SourceofIncome();
                        s.text = sourceofIncome[i].Text;
                        s.value = Convert.ToInt32(sourceofIncome[i].Value);
                        ListHeader.SourceofIncome.Add(s);
                    }

                    var guarantorCollateral = ListData.GuarantorCollateral();
                    for (int i = 0; i < guarantorCollateral.Count; i++)
                    {
                        GuarantorCollateral g = new GuarantorCollateral();
                        g.text = guarantorCollateral[i].Text;
                        g.value = Convert.ToInt32(guarantorCollateral[i].Value);
                        ListHeader.GuarantorCollateral.Add(g);
                    }

                    var debtStatus = ListData.DebtStatus();
                    for (int i = 0; i < debtStatus.Count; i++)
                    {
                        DebtStatus d = new DebtStatus();
                        d.text = debtStatus[i].Text;
                        d.value = Convert.ToInt32(debtStatus[i].Value);
                        ListHeader.DebtStatus.Add(d);
                    }

                    var familyStatus = ListData.FamilyStatus();
                    for (int i = 0; i < familyStatus.Count; i++)
                    {
                        FamilyStatus f = new FamilyStatus();
                        f.text = familyStatus[i].Text;
                        f.value = Convert.ToInt32(familyStatus[i].Value);
                        ListHeader.FamilyStatus.Add(f);
                    }


                    response.ERR = ERR;
                    response.SMS = SMS;
                    response.Data = ListHeader;
                }
                #endregion data
            }
            catch (Exception ex)
            {
                ERR = "Error";
                SMS = "Something was wrong";
            }
            #region if Error
            if (ERR == "Error")
            {
                response.ERR = ERR;
                response.SMS = SMS;
                response.Data = null;
            }
            #endregion if Error

            return response;
        }

    }
}