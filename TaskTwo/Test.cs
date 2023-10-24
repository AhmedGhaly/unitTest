using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Payment;
using CarAPI.Repositories_DAL;
using CarAPI.Services_BLL;
using CarFactoryAPITests.Fake;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace TaskTwo
{
    public class Test
    {
        private readonly ITestOutputHelper testOutput;
        private Mock<IOwnersRepository> ownerRepoMock;
        private Mock<ICarsRepository> carRepoMock;
        private OwnersService ownersService;


        public Test(ITestOutputHelper testOutput)
        {
            this.testOutput = testOutput;

            testOutput.WriteLine("Start ...");

            carRepoMock = new Mock<ICarsRepository>();
            ownerRepoMock = new Mock<IOwnersRepository>();

            ownersService = new OwnersService(
      carRepoMock.Object,
      ownerRepoMock.Object,
      new CashService()
      );
        }
        public void Dispose()
        {
            // Test clean up
            testOutput.WriteLine("end....");
        }


        [Fact]
        public void BuyCar_CarNotExist_DoesNotExist()
        {
            InMemoryContext inMemoryContext = new InMemoryContext();

            CarsRepository carsRepository = new CarsRepository(inMemoryContext);
            OwnersRepository ownersRepository = new OwnersRepository(inMemoryContext);

            OwnersService ownersService = new OwnersService(
                carsRepository,
                ownersRepository,
                new CashService()
                );

            BuyCarInput buyCarInput = new()
            {
                CarId = 4,
                OwnerId = 1,
                Amount = 100
            };

            var result = ownersService.BuyCar(buyCarInput);

            Assert.Equal("Car doesn't exist", result);
            testOutput.WriteLine("test car not exist");
        }

        [Fact]
        public void BuyCar_CarHasOwner_AlreadySold()
        {
            OwnersService ownersService = new OwnersService(
                new StupCarsWithOwnerRepo(),
                new StupOwnerWithoutCarRepo(),
                new CashService()
                );
            BuyCarInput buyCarInput = new()
            {
                CarId = 1,
                OwnerId = 1,
                Amount = 100
            };

            var result = ownersService.BuyCar(buyCarInput);

            Assert.Equal("Already sold", result);
        }



    }
}
