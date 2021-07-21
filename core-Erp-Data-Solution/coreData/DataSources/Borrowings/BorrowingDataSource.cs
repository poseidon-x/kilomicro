using System.Collections.Generic;
using System.ComponentModel;
using coreLogic;
using coreLogic.HelperClasses;
using coreLogic.Models.Borrowing;

namespace coreData.DataSources.Borrowings
{
    [DataObject]
    public class BorrowingDataSource
    {
        private readonly coreLoansEntities le;

        //call a constructor to instialize a the  context 
        public BorrowingDataSource()
        {
            le = new coreLoansEntities();

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        public BorrowingAccountViewModel GetBorrowing(int borrowingId)
        {
            //declare BorrowingReportManager to retrieve all borrowing data
            BorrowingReportManager brm = new BorrowingReportManager(borrowingId,le);

            BorrowingAccountViewModel borrowing = new BorrowingAccountViewModel
            {
                clientName = brm.getclientName(),
                borrowingNumber = brm.getBorrowingNo(),
                amountDisbursed = brm.getAmountDisbursed(),
                disbursementDate = brm.getDisbursementDate(),
                expiryDate = brm.getExpiryDate(),
                borrowingInterest = brm.getBorrowingInterest(),
                totalAmountPaid = brm.getAmountDisbursed(),
                totalInterestPaid = brm.getTotalInterestPaid(),
                totalPrincipalPaid = brm.getTotalPrincipalPaid(),
                outstandingInterest = brm.getOutstandingInterest(),
                outstandingPrincipal = brm.getOustandingPrincipal(),
                outstandingTotal = brm.getOutstandingTotal(),
                JnlEntries = brm.getBorrowingTransactions()
            };

            return borrowing;
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        public ClientBorrowingsViewModel GetClientBorrowings(int clientId)
        {
            //declare BorrowingReportManager to retrieve all borrowing data
            BorrowingReportManager brm = new BorrowingReportManager(clientId,1, le);

            ClientBorrowingsViewModel client = new ClientBorrowingsViewModel
            {
                clientName = brm.clientName,
                brws = brm.getBorrowings(),
                clientBorrowings = new List<BorrowingAccountViewModel>()
            };

           

            foreach (var borrowing in client.brws)
            {
                BorrowingReportManager clientBorrow = new BorrowingReportManager(borrowing.borrowingId, le);

                BorrowingAccountViewModel brw = new BorrowingAccountViewModel
                {
                    clientName = clientBorrow.getclientName(),
                    borrowingNumber = clientBorrow.getBorrowingNo(),
                    amountDisbursed = clientBorrow.getAmountDisbursed(),
                    disbursementDate = clientBorrow.getDisbursementDate(),
                    expiryDate = clientBorrow.getExpiryDate(),
                    borrowingInterest = clientBorrow.getBorrowingInterest(),
                    totalAmountPaid = clientBorrow.getAmountDisbursed(),
                    totalInterestPaid = clientBorrow.getTotalInterestPaid(),
                    totalPrincipalPaid = clientBorrow.getTotalPrincipalPaid(),
                    outstandingInterest = clientBorrow.getOutstandingInterest(),
                    outstandingPrincipal = clientBorrow.getOustandingPrincipal(),
                    outstandingTotal = clientBorrow.getOutstandingTotal(),
                    JnlEntries = clientBorrow.getBorrowingTransactions()
                };
                client.clientBorrowings.Add(brw);
            }

            return client;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public ClientBorrowingsViewModel GetOutstandingBorrowings()
        {
            //declare BorrowingReportManager to retrieve all borrowing data
            BorrowingReportManager brm = new BorrowingReportManager(le);

            ClientBorrowingsViewModel client = new ClientBorrowingsViewModel
            {
                brws = brm.getBorrowings(),
                clientBorrowings = new List<BorrowingAccountViewModel>()
            };

            foreach (var borrowing in client.brws)
            {
                BorrowingReportManager clientBorrow = new BorrowingReportManager(borrowing.borrowingId, le);

                BorrowingAccountViewModel brw = new BorrowingAccountViewModel
                {
                    clientName = clientBorrow.getclientName(),
                    borrowingNumber = clientBorrow.getBorrowingNo(),
                    amountDisbursed = clientBorrow.getAmountDisbursed(),
                    disbursementDate = clientBorrow.getDisbursementDate(),
                    expiryDate = clientBorrow.getExpiryDate(),
                    borrowingInterest = clientBorrow.getBorrowingInterest(),
                    totalAmountPaid = clientBorrow.getAmountDisbursed(),
                    totalInterestPaid = clientBorrow.getTotalInterestPaid(),
                    totalPrincipalPaid = clientBorrow.getTotalPrincipalPaid(),
                    outstandingInterest = clientBorrow.getOutstandingInterest(),
                    outstandingPrincipal = clientBorrow.getOustandingPrincipal(),
                    outstandingTotal = clientBorrow.getOutstandingTotal(),
                    JnlEntries = clientBorrow.getBorrowingTransactions()
                };
                client.clientBorrowings.Add(brw);
            }

            return client;
        }


    }
}

