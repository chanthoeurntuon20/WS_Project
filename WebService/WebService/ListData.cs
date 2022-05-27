using System.Collections.Generic;

namespace WebService
{
    public class SelectList
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public int Parent { get; set; }
    }
    public static class ListData
    {

        public static List<SelectList> OverdueType()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "អតិថិជនយឺត", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "អតិថិជនស្លាប់", Value = "2" });
                list.Add(new SelectList { Text = "អតិថិជនមានបញ្ហាប្រព័ន្ធ", Value = "3" });
            }
            catch
            {
                //
            }
            return list;
        }


        public static List<SelectList> MainReason()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "មានបញ្ហាមុខរបរ", Value = "1" });
                list.Add(new SelectList { Text = "ជំពាក់បំណុលវ័ណ្ឌក", Value = "2" });
                list.Add(new SelectList { Text = "ចែករំលែកឥណទាន", Value = "3" });
                list.Add(new SelectList { Text = "មានគ្រោះថ្នាក់ / បញ្ហាសុខភាព", Value = "4" });
                list.Add(new SelectList { Text = "មានបញ្ហាគ្រួសារ", Value = "5" });
                list.Add(new SelectList { Text = "ជាប់ពិរុទ្ធ", Value = "6" });
                list.Add(new SelectList { Text = "កំហុសរបស់បុគ្គលិកAMK", Value = "7" });
                list.Add(new SelectList { Text = "មិនមានឆន្ទៈក្នុងការសងត្រឡប់", Value = "8" });
                list.Add(new SelectList { Text = "បញ្ហាយឺតយ៉ាវមកពីដៃគូរអាជីវកម្មមិនទាន់សងតាមការធានា", Value = "9" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> BusinessProblem()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "មុខរបរដួលរលំ / បាត់បង់ការងារ / បាត់បង់អ្នករកចំណូល", Value = "1", Parent = 1 });
                list.Add(new SelectList { Text = "មុខរបរចុះខ្សោយ / ប្រាក់ខែថយចុះ ឬមិនសូវមានការងារធ្វើ", Value = "2", Parent = 1 });
                list.Add(new SelectList { Text = "មុខរបរមានបញ្ហាលំហូរសាច់ប្រាក់ (មួយគ្រាៗ) / បើប្រាក់ឈ្នួលមិនទាន់", Value = "3", Parent = 1 });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> Indebtedness()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "ជំពាក់បំណុល ១ ស្ថាប័ន - មុនពេលខ្ចីAMK ", Value = "4", Parent = 2 });
                list.Add(new SelectList { Text = "ជំពាក់បំណុល ២ ស្ថាប័ន ឬច្រើនជាង - មុនពេលខ្ចីAMK", Value = "5", Parent = 2 });
                list.Add(new SelectList { Text = "ជំពាក់បំណុល ១ ស្ថាប័ន - ក្រោយពេលខ្ចីAMK", Value = "6", Parent = 2 });
                list.Add(new SelectList { Text = "ជំពាក់បំណុល ២ ស្ថាប័ន ឬច្រើនជាង - ក្រោយពេលខ្ចីAMK", Value = "7", Parent = 2 });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> ShareCredit()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "ខ្ចីប្រាក់ឲ្យគេ", Value = "8" });
                list.Add(new SelectList { Text = "ខ្ចីប្រាក់ចែកគ្នា", Value = "9" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> DangerHealthProblems()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "ជួបគ្រោះថ្នាក់ការងារ", Value = "10" });
                list.Add(new SelectList { Text = "ជួបគ្រោះថ្នាក់ចៃដន្យ", Value = "11" });
                list.Add(new SelectList { Text = "ជួបគ្រោះធម្មជាតិ", Value = "12" });
                list.Add(new SelectList { Text = "រងគ្រោះដោយអំពើចោរកម្ម ឬបទល្មើសផ្សេងៗ", Value = "13" });
                list.Add(new SelectList { Text = "មានជំងឺ", Value = "14" });
            }
            catch
            {
                //
            }
            return list;
        }
        public static List<SelectList> FamilyProblems()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "បែកបាក់គ្រួសារ / លែងលះគ្នា", Value = "15" });
                list.Add(new SelectList { Text = "មានអំពើហឹង្សាក្នុងគ្រួសារ / មានទំនាស់ក្នុងគ្រួសារ", Value = "16" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> Convicted()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "ជាប់ពិរុទ្ធ", Value = "17" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> AMKStaffError()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "កំហុសបែបរដ្ឋបាល", Value = "18" });
                list.Add(new SelectList { Text = "កំហុសក្នុងការវាយតំលៃឥណទាន", Value = "19" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> NotWillingToPayBack()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "មិនមានឆន្ទៈក្នុងការសងត្រលប់", Value = "20" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> ShareLateIssuesFromBusinessPartnersNotYet()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "បញ្ហាយឺតយ៉ាវមកពីដៃគូអាជីវកម្មមិនទាន់សងតាមការធានា", Value = "21" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> Reason()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "Business Collapse / Job Loss / Loss of Earners", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "Weak Employment / Reduced Wages Or Low Employment", Value = "2" });
                list.Add(new SelectList { Text = "Occupation With Cash Flow Problems (one time) / If Wages Are Not yet", Value = "3" });

                list.Add(new SelectList { Text = "Indebted to an institution - before borrowing AMK", Value = "4" });
                list.Add(new SelectList { Text = "Debt of 2 or more institutions - before AMK loan", Value = "5" });
                list.Add(new SelectList { Text = "Debts to one institution - after borrowing AMK", Value = "6" });
                list.Add(new SelectList { Text = "Debt of 2 institutions or more - after AMK loan", Value = "7" });

                list.Add(new SelectList { Text = "Borrow Money", Value = "8" });
                list.Add(new SelectList { Text = "Borrow Money And Share", Value = "9" });

                list.Add(new SelectList { Text = "Work accident", Value = "10" });
                list.Add(new SelectList { Text = "Accident", Value = "11" });
                list.Add(new SelectList { Text = "Natural disasters", Value = "12" });
                list.Add(new SelectList { Text = "Victimized by theft or other crimes", Value = "13" });
                list.Add(new SelectList { Text = "Have a disease", Value = "14" });

                list.Add(new SelectList { Text = "Family breakup / divorce", Value = "15" });
                list.Add(new SelectList { Text = "Domestic Violence / Domestic Conflict", Value = "16" });

                list.Add(new SelectList { Text = "Convicted", Value = "17" });

                list.Add(new SelectList { Text = "Administrative error", Value = "18" });
                list.Add(new SelectList { Text = "Credit appraisal error", Value = "19" });

                list.Add(new SelectList { Text = "Not willing to repay", Value = "20" });

                list.Add(new SelectList { Text = "Late issues from business partners not yet paid under warranty", Value = "21" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> CustomerRating()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "មានលទ្ធភាពសង ហើយ មានឆន្ទៈសង", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "មានលទ្ធភាពសង ប៉ុន្ដែ គ្មានឆន្ទៈសង", Value = "2" });
                list.Add(new SelectList { Text = "គ្មានលទ្ធភាពសង ប៉ុន្ដែ មានឆន្ទៈសង", Value = "3" });
                list.Add(new SelectList { Text = "គ្មានលទ្ធភាពសង ហើយ គ្មានឆន្ទៈសង", Value = "4" });
            }
            catch
            {
                //
            }
            return list;
        }


        public static List<SelectList> ManagementAction()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "មិនទាន់បានទៅដោះស្រាយ", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "បានដោះស្រាយជាមួយអតិថិជន", Value = "2" });
                list.Add(new SelectList { Text = "បានដោះស្រាយជាមួយអ្នកធានា", Value = "3" });
                list.Add(new SelectList { Text = "បានដោះស្រាយដោយមានអាជ្ញាធរចូលរួម", Value = "4" });
                list.Add(new SelectList { Text = "បន្តសំណុំរឿងទៅតុលាការ", Value = "5" });
                list.Add(new SelectList { Text = "កំពុងដំណើរការលក់ទ្រព្យដើម្បីទូទាត់", Value = "6" });
            }
            catch
            {
                //
            }
            return list;
        }


        public static List<SelectList> AccuracyOfUseCredit()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "ប្រើប្រាស់ត្រឹមត្រូវតាមគោលបំណង", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "យកឥណទានទៅឲ្យគេទាំងស្រុង ឬទៅចែកគ្នា", Value = "2" });
                list.Add(new SelectList { Text = "យកទៅចំណាយលើទ្រព្យមិនបង្កើនចំណូល និងផ្គត់ផ្គង់ការចំណាយផ្សេងៗ", Value = "3" });
                list.Add(new SelectList { Text = "យកទៅទូទាត់បំណុលឯកជន ឬសា្ថប័នផ្សេង", Value = "4" });
                list.Add(new SelectList { Text = "ការប្រើខុសគោលបំណងផ្សេងៗ", Value = "5" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> StatusOfSolutions()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "បង់សងបានទៀងទាត់ ", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "បង់សងបានមួយចំនួន", Value = "2" });
                list.Add(new SelectList { Text = "បានធ្វើកិច្ចសន្យាបង់សង", Value = "3" });
                list.Add(new SelectList { Text = "មិនមានបង់សង", Value = "4" });
                list.Add(new SelectList { Text = "បានដាក់ពាក្យបណ្តឹងទប់ស្កាត់លក់អចលនទ្រព្យ", Value = "5" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> PromisedAmountCurrency()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "USD", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "KHR", Value = "2" });
                list.Add(new SelectList { Text = "THB", Value = "3" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> SourceOfMoneyPaid()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "សាច់ប្រាក់បានពីមុខរបរ", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "ខ្ចីគេ(ស្ថានប័នផ្សេងៗ និងពីឯកជន)", Value = "2" });
                list.Add(new SelectList { Text = "បានពីសាច់ញាតិ ឬពីអ្នកធានា", Value = "3" });
                list.Add(new SelectList { Text = "លក់ទ្រព្យ", Value = "4" });

            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> CustomerAttitude()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "1", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "2", Value = "2" });
                list.Add(new SelectList { Text = "3", Value = "3" });
                list.Add(new SelectList { Text = "4", Value = "4" });
                list.Add(new SelectList { Text = "5", Value = "5" });


            }
            catch
            {
                //
            }
            return list;
        }
        public static List<SelectList> SourceofIncome()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "1", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "2", Value = "2" });
                list.Add(new SelectList { Text = "3", Value = "3" });
                list.Add(new SelectList { Text = "4", Value = "4" });
                list.Add(new SelectList { Text = "5", Value = "5" });

            }
            catch
            {
                //
            }
            return list;
        }
        public static List<SelectList> GuarantorCollateral()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "1", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "2", Value = "2" });
                list.Add(new SelectList { Text = "3", Value = "3" });
                list.Add(new SelectList { Text = "4", Value = "4" });
                list.Add(new SelectList { Text = "5", Value = "5" });
            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> DebtStatus()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "1", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "2", Value = "2" });
                list.Add(new SelectList { Text = "3", Value = "3" });
                list.Add(new SelectList { Text = "4", Value = "4" });
                list.Add(new SelectList { Text = "5", Value = "5" });

            }
            catch
            {
                //
            }
            return list;
        }

        public static List<SelectList> FamilyStatus()
        {
            var list = new List<SelectList>();
            try
            {
                list.Add(new SelectList { Text = "1", Value = "1", Selected = true });
                list.Add(new SelectList { Text = "2", Value = "2" });
                list.Add(new SelectList { Text = "3", Value = "3" });
                list.Add(new SelectList { Text = "4", Value = "4" });
                list.Add(new SelectList { Text = "5", Value = "5" });

            }
            catch
            {
                //
            }
            return list;
        }
    }
}