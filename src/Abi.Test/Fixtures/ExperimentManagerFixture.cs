using Abi.Data;
using Abi.OrchardCore;
using Abi.Services;
using Moq;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abi.Test.Fixtures
{
    public class ExperimentManagerFixture
    {
        protected readonly Mock<IVisitorRepository> _visitorRepositoryMock = new Mock<IVisitorRepository>(MockBehavior.Strict);
        protected readonly Mock<ISessionRepository> _sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
        protected readonly Mock<IVariantRepository> _variantRepositoryMock = new Mock<IVariantRepository>(MockBehavior.Strict);
        protected readonly Mock<IEncounterRepository> _encounterRepositoryMock = new Mock<IEncounterRepository>(MockBehavior.Strict);
        protected readonly Mock<ICookieService> _cookieServiceMock = new Mock<ICookieService>(MockBehavior.Strict);
        protected readonly Mock<IContentBalancer> _contentBalancerMock = new Mock<IContentBalancer>();

        protected OrchardExperimentManager CreateExperimentManager()
        {
            return new OrchardExperimentManager(_visitorRepositoryMock.Object,
                _sessionRepositoryMock.Object,
                _variantRepositoryMock.Object,
                _encounterRepositoryMock.Object,
                _cookieServiceMock.Object,
                _contentBalancerMock.Object);
        }

        protected void VerifyMocks()
        {
            _visitorRepositoryMock.Verify();
            _sessionRepositoryMock.Verify();
            _variantRepositoryMock.Verify();
            _encounterRepositoryMock.Verify();
            _cookieServiceMock.Verify();
            _contentBalancerMock.Verify();
        }

        protected FlowPart SampleFlowPart
        {
            get
            {
                var flowPart = new FlowPart { ContentItem = new ContentItem { ContentItemId = "experimentflowpart123" } };
                flowPart.Widgets.Add(new ContentItem { ContentItemId = "widget1" });
                flowPart.Widgets.Add(new ContentItem { ContentItemId = "widget2" });
                return flowPart;
            }
        }
    }
}
