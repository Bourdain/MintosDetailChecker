﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using HtmlAgilityPack;
using Data.EF;
using System.Globalization;

namespace MintosRunnable
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessCSVFile();

            Console.WriteLine("1 - Read CSV file and insert Listings in Database.\n2 - Process Loans with no details");
            string userInput = Console.ReadLine();

            if (userInput == "1")
            {

                ProcessCSVFile();

            }

            if (userInput == "2")
            {
                int i = 0;
                while (i < int.Parse(ConfigurationManager.AppSettings["NumberOfThreads"]))
                {
                    Thread t1 = new Thread(ProcessLoanDetails);
                    t1.Start();
                    i++;

                }
                while (true)
                {
                    Business.PrimaryListings primaryListings = new Business.PrimaryListings();

                    var primaryLoan = primaryListings.GetRandomNonDetailedLoan();
                    if (primaryLoan == null)
                        break;
                    Thread.Sleep(5000);
                }
            }


        }

        private static void ProcessLoanDetails()
        {
            GC.Collect();
            while (true)
            {
                Business.PrimaryListings primaryListings = new Business.PrimaryListings();

                var primaryLoan = primaryListings.GetRandomNonDetailedLoan();
                if (primaryLoan == null)
                {
                    Console.WriteLine("Ran out of rows in thread #" + Thread.CurrentThread.ManagedThreadId);
                    Thread.CurrentThread.Abort();
                    break;
                }

                Console.WriteLine("Thead " + Thread.CurrentThread.ManagedThreadId + " " + primaryLoan.ID);

                HtmlWeb htmlWeb = new HtmlWeb();
                var htmlDocumentsPage = htmlWeb.Load("https://www.mintos.com/en/" + primaryLoan.ID);
                var getAllTables = htmlDocumentsPage.DocumentNode.Descendants("table");
                HtmlNode extraInformationTable = getAllTables.ToList()[1];
                var tableRowDescriptions = extraInformationTable.SelectNodes(extraInformationTable.XPath + "//td[@class='field-description']");
                var tableRowValues = extraInformationTable.SelectNodes(extraInformationTable.XPath + "//td[@class='value']");

                Business.LoanDetails businessLoanDetails = new Business.LoanDetails();
                var loanDetails = businessLoanDetails.GetDetails(tableRowDescriptions, tableRowValues);
                var loanID = businessLoanDetails.AddDetails(loanDetails);

                primaryListings.UpdateLoanDetailsID(loanID, primaryLoan);

            }
        }

        private static void ProcessCSVFile()
        {
            string csvPath = ConfigurationManager.AppSettings["CSVPath"];
            var files = Directory.GetFiles(csvPath, "*.csv");
            foreach (var item in files)
            {
                List<PrimaryLoan> listOfLoans = new List<PrimaryLoan>();
                using (var reader = new StreamReader(item))
                using (var csv = new CsvReader(reader))
                {
                    bool isHeader = true;
                    while (csv.Read())
                    {
                        if (isHeader)
                        {
                            isHeader = false;
                            continue;
                        }
                        PrimaryLoan primaryLoanToAdd = new PrimaryLoan();

                        primaryLoanToAdd.Country = csv.GetField(0);
                        primaryLoanToAdd.ID = csv.GetField(1);
                        primaryLoanToAdd.Issue_Date = DateTime.ParseExact(csv.GetField(2),"dd.MM.yyyy",CultureInfo.InvariantCulture);
                        primaryLoanToAdd.Closing_Date = DateTime.ParseExact(csv.GetField(3), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                        primaryLoanToAdd.Loan_Type = csv.GetField(4);
                        primaryLoanToAdd.Amortization_Method = csv.GetField(5);
                        primaryLoanToAdd.Loan_Originator = csv.GetField(6);
                        primaryLoanToAdd.Loan_Amount = decimal.Parse(csv.GetField(7));
                        primaryLoanToAdd.Remaining_Principal = decimal.Parse(csv.GetField(8));
                        primaryLoanToAdd.LTV = decimal.Parse(csv.GetField(9));
                        primaryLoanToAdd.Interest_Rate = decimal.Parse(csv.GetField(10));
                        primaryLoanToAdd.Term = csv.GetField(11);
                        primaryLoanToAdd.Payments_Received = int.Parse(csv.GetField(12));
                        primaryLoanToAdd.Status = csv.GetField(13);
                        primaryLoanToAdd.Amount_Available_for_Investment = decimal.Parse(csv.GetField(14));
                        primaryLoanToAdd.Buyback_Guarantee = csv.GetField(15);
                        primaryLoanToAdd.My_Investments = decimal.Parse(csv.GetField(16));
                        primaryLoanToAdd.Currency = csv.GetField(17);
                        primaryLoanToAdd.Borrower_APR = decimal.Parse(csv.GetField(18));

                        listOfLoans.Add(primaryLoanToAdd);
                    }
                }
            }
        }
    }
}
