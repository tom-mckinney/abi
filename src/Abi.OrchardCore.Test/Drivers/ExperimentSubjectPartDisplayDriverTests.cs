using Abi.Models;
using Abi.OrchardCore.Data;
using Abi.OrchardCore.Drivers;
using Abi.OrchardCore.Models;
using Abi.OrchardCore.ViewModels;
using Moq;
using OrchardCore.DisplayManagement.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Abi.OrchardCore.Test.Drivers
{
    public class ExperimentSubjectPartDisplayDriverTests
    {
        private readonly MockRepository _mocks = new MockRepository(MockBehavior.Strict);
        private readonly Mock<IExperimentRepository> _experimentRepositoryMock;
        private readonly Mock<IUpdateModel> _updateModelMock;

        public ExperimentSubjectPartDisplayDriverTests()
        {
            _experimentRepositoryMock = _mocks.Create<IExperimentRepository>();
            _updateModelMock = _mocks.Create<IUpdateModel>();
        }

        public ExperimentSubjectPartDisplayDriver CreateTestClass()
        {
            return new ExperimentSubjectPartDisplayDriver(_experimentRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_creates_a_new_experiment_if_specified()
        {
            _experimentRepositoryMock.Setup(m => m.CreateAsync("Foo"))
                .ReturnsAsync(new Experiment { Id = 123, ExperimentId = "abc-456", Name = "Foo" });

            _updateModelMock.Setup(m => m.TryUpdateModelAsync(It.IsAny<ExperimentSubjectPartEditViewModel>(), ""))
                .Callback((ExperimentSubjectPartEditViewModel viewModel, string prefix) =>
                {
                    viewModel.CreateNewExperiment = true;
                    viewModel.ExperimentName = "Foo";
                })
                .ReturnsAsync(true);

            var part = new ExperimentSubjectPart();

            var driver = CreateTestClass();

            await driver.UpdateAsync(part, _updateModelMock.Object);

            Assert.Equal("abc-456", part.ExperimentId);
            Assert.Equal("Foo", part.Name);

            _mocks.VerifyAll();
        }
    }
}
