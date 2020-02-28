using Abi.OrchardCore.Data;
using Moq;
using OrchardCore;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Abi.OrchardCore.Test.Data
{
    public class ExperimentRepositoryTests
    {
        private readonly Mock<IContentManager> _contentManagerMock = new Mock<IContentManager>(MockBehavior.Strict);
        private readonly Mock<IOrchardHelper> _orchardHelperMock = new Mock<IOrchardHelper>(MockBehavior.Strict);

        private IExperimentRepository CreateTestClass()
        {
            return new ExperimentRepository(_contentManagerMock.Object, _orchardHelperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_creates_a_new_ContentItem()
        {
            _contentManagerMock.Setup(m => m.NewAsync(Constants.Types.Experiment))
                .ReturnsAsync(new ContentItem { ContentItemId = "content-123" });

            var repository = CreateTestClass();

            var experiment = await repository.CreateAsync("Foo");

            Assert.Equal("Foo", experiment.Name);
            Assert.Equal("content-123", experiment.ContentItemId);

            // verify mocks
        }
    }
}
