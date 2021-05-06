using NSubstitute;
using System;
using System.Collections.Generic;
using TrainApp.Core.Services;
using TrainApp.Data.Interfaces;
using TrainApp.Data.Repositories;
using Xunit;

namespace TrainApp.Tests.Core
{
    public class WagonServiceTests
    {

        [Fact]
        public async void CreateWagon_Throws_ArgumentNullException_When_WagonModel_IsNull()
        {
            // Arrange
            var wagonRepository = Substitute.For<IWagonRepository>();
            var wagonService = new WagonService(wagonRepository);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => wagonService.CreateWagonAsync(null));
        }

        [Fact]
        public async void GetWagon_Maps_Model_Correctly()
        {
            // Arrange
            var wagonRepository = Substitute.For<IWagonRepository>();
            var wagonService = new WagonService(wagonRepository);

            var chairs = new List<TrainApp.Data.Entities.Chair>
            {
                new TrainApp.Data.Entities.Chair { NearWindow = false, Number = 0, Reserved = false },
                new TrainApp.Data.Entities.Chair { NearWindow = false, Number = 1, Reserved = false }
            };
            var entity = new TrainApp.Data.Entities.Wagon
            {
                WagonId = 1,
                Chairs = chairs
            };

            wagonRepository.FindAsync(entity.WagonId).Returns(entity);

            // Act
            var wagonModel = await wagonService.GetWagonAsync(entity.WagonId);

            // Assert
            Assert.Equal(entity.WagonId, wagonModel.WagonId);
            Assert.Equal(entity.Chairs[1].Number, wagonModel.Chairs[1].Number);
            Assert.False(entity.Chairs[1].NearWindow);
        }
    }
}
