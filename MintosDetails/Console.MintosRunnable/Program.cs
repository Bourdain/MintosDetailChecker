using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace MintosRunnable
{
    class Program
    {
        static void Main(string[] args)
        {
            Business.PrimaryListings primaryListings = new Business.PrimaryListings();
            var primaryLoan = primaryListings.GetFirstNonDetailedLoan();

            ProcessLoan(primaryLoan);
        }

        private static void ProcessLoan(Data.EF.PrimaryLoan primaryLoan)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            var htmlDocumentsPage = htmlWeb.Load("https://www.mintos.com/en/" + primaryLoan.ID);
            var getAllTables = htmlDocumentsPage.DocumentNode.Descendants("table");
            HtmlNode extraInformationTable = getAllTables.ToList()[1];
            var tableRowDescriptions = extraInformationTable.SelectNodes(extraInformationTable.XPath + "//td[@class='field-description']");
            var tableRowValues = extraInformationTable.SelectNodes(extraInformationTable.XPath + "//td[@class='value']");

            Business.LoanDetails businessLoanDetails = new Business.LoanDetails();
            var loanDetails = businessLoanDetails.GetDetails(tableRowDescriptions, tableRowValues);
            var loanID = businessLoanDetails.AddDetails(loanDetails);

            Business.PrimaryListings primaryListings = new Business.PrimaryListings();
            primaryListings.UpdateLoanDetailsID(loanID, primaryLoan);


        }
    }
}
