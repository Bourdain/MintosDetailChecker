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
    }
}
