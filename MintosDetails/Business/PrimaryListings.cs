using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.EF;
namespace Business
{
    public class PrimaryListings
    {

        public PrimaryLoan GetFirstNonDetailedLoan()
        {
            using (MintosEntities mintosEntities = new MintosEntities())
            {
                return mintosEntities.PrimaryLoans.Where(x => x.Detail_ID == null).FirstOrDefault();
            }
        }

        public PrimaryLoan GetRandomNonDetailedLoan()
        {
            using (MintosEntities mintosEntities = new MintosEntities())
            {
                return mintosEntities.PrimaryLoans.Where(x => x.Detail_ID == null).OrderBy(r=>Guid.NewGuid()).FirstOrDefault();
            }
        }

        public void UpdateLoanDetailsID(int detailsID, Data.EF.PrimaryLoan primaryLoan)
        {
            using (MintosEntities mintosEntities = new MintosEntities())
            {
                mintosEntities.PrimaryLoans.Where(x => x.ID == primaryLoan.ID).SingleOrDefault().Detail_ID = detailsID;
                mintosEntities.SaveChanges();
            }
        }

        public void UpdateOrAddPrimaryLoans(List<PrimaryLoan> primaryLoansList)
        {
            using (MintosEntities mintosEntities = new MintosEntities())
            {
                foreach (var loanItem in primaryLoansList)
                {
                    if(mintosEntities.PrimaryLoans.Any(x=>x.ID == loanItem.ID))
                    {
                        Console.WriteLine("Updating " + loanItem.ID);
                        var loanInDB = mintosEntities.PrimaryLoans.Where(x => x.ID == loanItem.ID).Single();
                        loanInDB.Country = loanItem.Country;
                        loanInDB.Issue_Date = loanItem.Issue_Date;
                        loanInDB.Closing_Date = loanItem.Closing_Date;
                        loanInDB.Loan_Type = loanItem.Loan_Type;
                        loanInDB.Amortization_Method = loanItem.Amortization_Method;
                        loanInDB.Loan_Originator = loanItem.Loan_Originator;
                        loanInDB.Loan_Amount = loanItem.Loan_Amount;
                        loanInDB.Remaining_Principal = loanItem.Remaining_Principal;
                        loanInDB.LTV = loanItem.LTV;
                        loanInDB.Interest_Rate = loanItem.Interest_Rate;
                        loanInDB.Term = loanItem.Term;
                        loanInDB.Payments_Received = loanItem.Payments_Received;
                        loanInDB.Status = loanItem.Status;
                        loanInDB.Amount_Available_for_Investment = loanItem.Amount_Available_for_Investment;
                        loanInDB.Buyback_Guarantee = loanItem.Buyback_Guarantee;
                        loanInDB.My_Investments = loanItem.My_Investments;
                        loanInDB.Currency = loanItem.Currency;
                        loanInDB.Borrower_APR = loanItem.Borrower_APR;

                    }
                    else
                    {
                        //add
                        Console.WriteLine("Adding " + loanItem.ID);

                        mintosEntities.PrimaryLoans.Add(loanItem);
                    }
                }
                Console.WriteLine("Inserting/Updating Database");
                mintosEntities.SaveChanges();
            }
        }
    }
}
