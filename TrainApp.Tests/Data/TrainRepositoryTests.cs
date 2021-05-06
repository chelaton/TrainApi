using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TrainApp.Core.Models;
using TrainApp.Data;
using TrainApp.Data.Entities;
using TrainApp.Data.Repositories;
using Xunit;

namespace TrainApp.Tests.Data
{
    public class TrainRepositoryTests
    {
        private readonly DbContextOptions<TrainContext> _contextOptions;

        public TrainRepositoryTests()
        {
            _contextOptions = new DbContextOptionsBuilder<TrainContext>().UseInMemoryDatabase(databaseName: "TrainDb").Options;
        }

        [Fact]
        public async void Can_Add_Wagon()
        {
            // Arrange
            var chairs = new List<TrainApp.Data.Entities.Chair>();
            for (int i = 0; i < 2; i++)
            {
                chairs.Add(new TrainApp.Data.Entities.Chair { NearWindow = false, Number = i, Reserved = false });
            }
            var testWagon = new TrainApp.Data.Entities.Wagon
            {
                Chairs=chairs
            };

            // Act
            using (var context = new TrainContext(_contextOptions))
            {
                var repository = new WagonRepository(context);
                var wagon = await repository.AddAsync(testWagon);
            }

            // Assert
            using (var context = new TrainContext(_contextOptions))
            {
                var wagon = await context.Wagons.Include(c => c.Chairs).FirstOrDefaultAsync(x => x.WagonId == testWagon.WagonId);
                Assert.Equal(testWagon.Chairs.Count, wagon.Chairs.Count);
                Assert.Equal(testWagon.Chairs[0].Number, wagon.Chairs[0].Number);
            }
        }

        [Fact]
        public async void Can_Update_Wagon()
        {
            // Arrange
            var chairs = new List<TrainApp.Data.Entities.Chair>
            {
                new TrainApp.Data.Entities.Chair { NearWindow = false, Number = 0, Reserved = false },
                new TrainApp.Data.Entities.Chair { NearWindow = false, Number = 1, Reserved = false }
            };

            var testWagon = new TrainApp.Data.Entities.Wagon
            {
                Chairs = chairs
            };

            using (var context = new TrainContext(_contextOptions))
            {
                await context.Wagons.AddAsync(testWagon);
                await context.SaveChangesAsync();
            }

            var updatedWagon = testWagon;
            updatedWagon.Chairs[1].NearWindow = true;
            
            // Act
            using (var context = new TrainContext(_contextOptions))
            {
                var repository = new WagonRepository(context);
                var wagon = await repository.UpdateAsync(updatedWagon);
            }

            // Assert
            using (var context = new TrainContext(_contextOptions))
            {
                var wagon = await context.Wagons.Include(c => c.Chairs).FirstOrDefaultAsync(x => x.WagonId == testWagon.WagonId);

                Assert.Equal(testWagon.Chairs.Count, wagon.Chairs.Count);
                Assert.Equal(testWagon.Chairs[1].Number, wagon.Chairs[1].Number);
                Assert.True(wagon.Chairs[1].NearWindow);
            }

        }
    }
}
