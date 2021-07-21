using System;
using System.Linq;
using coreERP.Controllers.Clients;
using coreLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace coreErpApi.Tests
{
    [TestClass]
    public class ClientLookupControllerTests
    {
        private MockcoreLoansEntities _context;
        [TestInitialize]
        public void TestInit()
        {
            _context = new MockcoreLoansEntities();
            var client = new client
            {
                clientID = 1,
                surName = "Tingare",
                otherNames = @"Naa"
            };
            _context.susuAccounts.Add(new susuAccount
            {
                clientID = 1,
                susuAccountID = 1,
                regularSusCommissionAmount = 10,
                contributionAmount = 10
            });
            _context.clients.Add(client);
        }
        
        [TestMethod]
        public void Test_GetSusuClients_Returns_Not_Null()
        {
            ClientLookUpController controller = new ClientLookUpController(_context);
            var susuClients = controller.GetSusuClients();
            Assert.IsNotNull(susuClients);
        }

        [TestMethod]
        public void Test_GetSusuClients_Returns_Non_Empty_List()
        {
            ClientLookUpController controller = new ClientLookUpController(_context);
            var susuClients = controller.GetSusuClients();
            Assert.IsTrue(susuClients.LongCount()>0);
        }

    }
}
