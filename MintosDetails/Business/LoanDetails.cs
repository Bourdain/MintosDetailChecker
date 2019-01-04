using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Data.EF;
namespace Business
{
    public class LoanDetails
    {
        public LoanDetail GetDetails(HtmlNodeCollection tableRowDescriptions, HtmlNodeCollection tableRowValues)
        {
            LoanDetail loanDetail = new LoanDetail();

            for (int i = 0; i < tableRowDescriptions.Count; i++)
            {
                switch (tableRowDescriptions[i].InnerText.Trim())
                {
                    case "Previous Loans":
                        loanDetail.PreviousLoans = int.Parse(tableRowValues[i].InnerText.Trim());
                        break;

                    case "Loan Purpose":
                        loanDetail.Loan_Purpose = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Borrower":
                        loanDetail.Borrower = tableRowValues[i].InnerText.Trim();

                        if (tableRowValues[i].InnerText.Trim().Contains("Male"))
                        {
                            loanDetail.Gender = "Male";
                        }

                        else if (tableRowValues[i].InnerText.Trim().Contains("Female"))
                        {
                            loanDetail.Gender = "Female";
                        }

                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\d+");

                        if (regex.IsMatch(tableRowValues[i].InnerText.Trim()))
                        {
                            loanDetail.Age = int.Parse(regex.Match(tableRowValues[i].InnerText.Trim()).ToString());
                        }

                        break;

                    case "Business Sector":
                        loanDetail.Business_Sector = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Description":
                        loanDetail.Description = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Country":
                        loanDetail.Country = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Risk category":
                        loanDetail.Risk_category = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Estimated annual default rate":
                        loanDetail.Estimated_annual_default_rate = float.Parse(tableRowValues[i].InnerText.Trim());
                        break;

                    case "Collateral":
                        loanDetail.Collateral = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Valuation":
                        loanDetail.Valuation = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Personal guarantee":
                        loanDetail.Personal_guarantee = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Registered":
                        loanDetail.Registered = DateTime.ParseExact(tableRowValues[i].InnerText.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        break;

                    case "Invoice Transaction":
                        loanDetail.Invoice_Transaction = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Invoice Recourse":
                        loanDetail.Invoice_Recourse = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Real Estate Type":
                        loanDetail.Real_Estate_Type = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Size":
                        loanDetail.Size = tableRowValues[i].InnerText.Trim();
                        break;

                    case "VAT Country":
                        loanDetail.VAT_Country = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Liability":
                        loanDetail.Liability = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Pledge Type":
                        loanDetail.Pledge_Type = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Occupation":
                        loanDetail.Occupation = tableRowValues[i].InnerText.Trim();
                        break;

                    case "Dependents":
                        loanDetail.Dependents = tableRowValues[i].InnerText.Trim();
                        break;

                    default:
                        break;
                }
            }

            return loanDetail;
        }

        public int AddDetails (Data.EF.LoanDetail loanDetail)
        {
            using (MintosEntities mintosEntities = new MintosEntities())
            {
                loanDetail = mintosEntities.LoanDetails.Add(loanDetail);
                mintosEntities.SaveChanges();
                return loanDetail.DetailsID;
            }
        }
    }
}
