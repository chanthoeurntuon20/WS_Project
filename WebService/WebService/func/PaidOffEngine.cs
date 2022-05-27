using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.func
{
    public class PaidOffEngine
    {
        public double PaidOffCalculator(string ProCode, double AmountToClose, string paidOffDate, string maturityDate
            , string disbDate, string firstRapayDate, string filingDate, double approvedAmount, Boolean isMigration
            , int loanTerm, double outstandingBalance, double originalPriDueAmount)
        {
            double PaidOffAmt = 0;
            string fixDate = "2020-02-10";
            //check is easy loan
            if (isEasyLoan(ProCode))
            {
                PaidOffAmt = AmountToClose;
            }
            else if (isPaidOffBiggerThanMaturity(paidOffDate, maturityDate))
            {
                PaidOffAmt = AmountToClose;
            }
            else if (compareCalendarFromFormat(disbDate, fixDate) >= 0 && !isProductExcluded(ProCode) && firstRapayDate != "")
            {
                PaidOffAmt = penaltyPercetageAmt(filingDate, approvedAmount, paidOffDate, firstRapayDate) + AmountToClose;
            }
            else
            {
                if (isMigration)
                {
                    if (loanTerm <= 12 && compareCalendarFromFormatBoolean(DateTime.Now.ToString(), maturityDate) == true || (CalculateDayBetweenTwoDates(DateTime.Now.ToString(), disbDate) / 30) <= 12)
                    {
                        PaidOffAmt = ((outstandingBalance - originalPriDueAmount) * 0.03) + AmountToClose;
                    }
                    else
                    {
                        PaidOffAmt = AmountToClose;
                    }
                }
                else
                {
                    if (loanTerm <= 12 && (CalculateDayBetweenTwoDates(disbDate, paidOffDate) / 30) <= 6)
                    {
                        PaidOffAmt = ((outstandingBalance - originalPriDueAmount) * 0.03) + AmountToClose;
                    }
                    else if (loanTerm > 12 && (CalculateDayBetweenTwoDates(disbDate, paidOffDate) / 30) <= 12)
                    {
                        PaidOffAmt = ((outstandingBalance - originalPriDueAmount) * 0.03) + AmountToClose;
                    }
                    else
                    {
                        PaidOffAmt = AmountToClose;
                    }

                    if (isPaidOffBiggerThanMaturity(paidOffDate, maturityDate))
                    {
                        PaidOffAmt = AmountToClose;
                    }

                }
            }

            return PaidOffAmt;
        }

        private Boolean isEasyLoan(string productCode)
        {
            Boolean rs = false;
            if (productCode == "INDEML")
                rs = true;

            return rs;
        }
        private Boolean isPaidOffBiggerThanMaturity(string paidOffDate, string maturityDate)
        {
            Boolean rs = false;

            try
            {
                DateTime d1 = Convert.ToDateTime(paidOffDate);
                DateTime d2 = Convert.ToDateTime(maturityDate);

                DateTime date1 = new DateTime(d1.Year, d1.Month, d1.Day, 0, 0, 0);
                DateTime date2 = new DateTime(d2.Year, d2.Month, d2.Day, 12, 0, 0);

                if (DateTime.Compare(date1, date2) >= 0)
                {
                    rs = true;
                }


            }
            catch { }

            return rs;
        }
        public int compareCalendarFromFormat(string dateTime1, string dateTime2)
        {
            int rs = 0;
            try
            {
                if (dateTime1 == "")
                {
                    rs = 0;
                }
                else
                {
                    DateTime d1 = Convert.ToDateTime(dateTime1);
                    DateTime d2 = Convert.ToDateTime(dateTime2);

                    DateTime date1 = new DateTime(d1.Year, d1.Month, d1.Day, 0, 0, 0);
                    DateTime date2 = new DateTime(d2.Year, d2.Month, d2.Day, 12, 0, 0);
                    rs = DateTime.Compare(date1, date2);
                }
            }
            catch
            {
                rs = 0;
            }
            return rs;
        }
        private Boolean isProductExcluded(string productCode)
        {
            Boolean rs = false;

            if (productCode == "LSGMTR" || productCode == "INDMTL" || productCode == "LSGEQL" || productCode == "INDFEL")
                rs = true;

            return rs;
        }
        private double penaltyPercetageAmt(string filingDate, double approvedAmount, string paidOffDate, string firstRapayDate)
        {
            double rs = 0;

            if (compareCalendarFromFormat(paidOffDate, firstRapayDate) > 0)
            {
                if (filingDate == "")
                {
                    rs = approvedAmount * 0.05;
                }
                else if (CalculateDayBetweenTwoDates(paidOffDate, filingDate) >= 60)
                {
                    rs = 0;
                }
                else
                {
                    rs = approvedAmount * 0.05;
                }
            }
            else
            {
                if (filingDate == "")
                {
                    rs = approvedAmount * 0.05;
                }
                else if (CalculateDayBetweenTwoDates(paidOffDate, filingDate) >= 60)
                {
                    rs = 0.03;
                }
                else
                {
                    rs = approvedAmount * 0.05;
                }
            }



            return rs;
        }
        public int CalculateDayBetweenTwoDates(string date1, string date2)
        {
            int rs = 0;

            try
            {
                rs = (Convert.ToDateTime(date1) - Convert.ToDateTime(date2)).Days;
            }
            catch { }

            return rs;
        }
        public Boolean compareCalendarFromFormatBoolean(string dateTime1, string dateTime2)
        {
            Boolean rs = false;
            try
            {
                DateTime d1 = Convert.ToDateTime(dateTime1);
                DateTime d2 = Convert.ToDateTime(dateTime2);

                DateTime date1 = new DateTime(d1.Year, d1.Month, d1.Day, 0, 0, 0);
                DateTime date2 = new DateTime(d2.Year, d2.Month, d2.Day, 12, 0, 0);
                if (DateTime.Compare(date1, date2) < 0)
                {
                    rs = true;
                }
            }
            catch
            {
                rs = false;
            }
            return rs;
        }
    }
}