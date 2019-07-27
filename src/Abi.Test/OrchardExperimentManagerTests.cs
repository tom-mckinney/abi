using Abi.Data;
using Abi.Models;
using Abi.OrchardCore;
using Abi.Services;
using Moq;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Abi.Test
{
    public class OrchardExperimentManagerTests
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

        public class GetOrCreateVisitorAsync : ExperimentManagerFixture
        {
            [Theory]
            [InlineData(14)]
            [InlineData(null)]
            public async Task Uses_visitor_cookie_from_request_if_valid(int? userId)
            {
                string expectedVisitorId = "visitorcookie123";
                var expectedVisitor = TestData.Create<Visitor>(v => v.VisitorId = expectedVisitorId);

                _cookieServiceMock.Setup(m => m.TryGetVisitorCookie(out expectedVisitorId))
                    .Returns(true)
                    .Verifiable();
                _visitorRepositoryMock.Setup(m => m.GetByPublicIdAsync(expectedVisitorId))
                    .ReturnsAsync(expectedVisitor)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.GetUserIdOrDefault())
                    .Returns(userId)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.AddVisitorCookie(expectedVisitorId))
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualVisitor = await manager.GetOrCreateVisitorAsync();

                Assert.Same(expectedVisitor, actualVisitor);

                VerifyMocks();
            }

            [Theory]
            [InlineData(14)]
            [InlineData(null)]
            public async Task Creates_new_visitor_if_request_cookie_is_invalid(int? userId)
            {
                string badVisitorId = "visitorcookie123";
                var expectedVisitor = TestData.Create<Visitor>();

                _cookieServiceMock.Setup(m => m.TryGetVisitorCookie(out badVisitorId))
                    .Returns(true)
                    .Verifiable();
                _visitorRepositoryMock.Setup(m => m.GetByPublicIdAsync(badVisitorId))
                    .ReturnsAsync((Visitor)null)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.GetUserIdOrDefault())
                    .Returns(userId)
                    .Verifiable();
                _visitorRepositoryMock.Setup(m => m.CreateAsync(userId))
                    .ReturnsAsync(expectedVisitor)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.AddVisitorCookie(expectedVisitor.VisitorId))
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualVisitor = await manager.GetOrCreateVisitorAsync();

                Assert.NotEqual(badVisitorId, actualVisitor.VisitorId);
                Assert.Same(expectedVisitor, actualVisitor);

                VerifyMocks();
            }

            [Theory]
            [InlineData(14)]
            [InlineData(null)]
            public async Task Creates_new_visitor_if_no_request_cookie(int? userId)
            {
                string noVisitorId;
                var newVisitor = TestData.Create<Visitor>();

                _cookieServiceMock.Setup(m => m.TryGetVisitorCookie(out noVisitorId))
                    .Returns(false)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.GetUserIdOrDefault())
                    .Returns(userId)
                    .Verifiable();
                _visitorRepositoryMock.Setup(m => m.CreateAsync(userId))
                    .ReturnsAsync(newVisitor)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.AddVisitorCookie(newVisitor.VisitorId))
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualVisitor = await manager.GetOrCreateVisitorAsync();

                Assert.Same(newVisitor, actualVisitor);

                VerifyMocks();
            }
        }

        public class GetOrCreateSessionAsync : ExperimentManagerFixture
        {
            [Fact]
            public async Task Use_session_id_from_request_cookie()
            {
                string sessionId = "sessionid123";
                Session expectedSession = TestData.Create<Session>(s => s.SessionId = sessionId);

                _cookieServiceMock.Setup(m => m.TryGetSessionCookie(out sessionId))
                    .Returns(true)
                    .Verifiable();
                _sessionRepositoryMock.Setup(m => m.GetByPublicIdAsync(sessionId))
                    .ReturnsAsync(expectedSession)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.AddSessionCookie(sessionId))
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualSession = await manager.GetOrCreateSessionAsync(It.IsAny<string>());

                Assert.Same(expectedSession, actualSession);

                VerifyMocks();
            }

            [Fact]
            public async Task Create_session_if_request_cookie_is_invalid()
            {
                string badSessionId = "sessionid123";
                Session expectedSession = TestData.Create<Session>(s => s.SessionId = "sessionid123");

                _cookieServiceMock.Setup(m => m.TryGetSessionCookie(out badSessionId))
                    .Returns(true)
                    .Verifiable();
                _sessionRepositoryMock.Setup(m => m.GetByPublicIdAsync(badSessionId))
                    .ReturnsAsync((Session)null)
                    .Verifiable();
                _sessionRepositoryMock.Setup(m => m.CreateAsync("visitorid123"))
                    .ReturnsAsync(expectedSession)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.AddSessionCookie(expectedSession.SessionId))
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualSession = await manager.GetOrCreateSessionAsync("visitorid123");

                Assert.Same(expectedSession, actualSession);

                VerifyMocks();
            }

            [Fact]
            public async Task Create_session_if_no_request_cookie()
            {
                string noSessionId;
                Session expectedSession = TestData.Create<Session>(s => s.SessionId = "sessionid123");

                _cookieServiceMock.Setup(m => m.TryGetSessionCookie(out noSessionId))
                    .Returns(false)
                    .Verifiable();
                _sessionRepositoryMock.Setup(m => m.CreateAsync("visitorid123"))
                    .ReturnsAsync(expectedSession)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.AddSessionCookie(expectedSession.SessionId))
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualSession = await manager.GetOrCreateSessionAsync("visitorid123");

                Assert.Same(expectedSession, actualSession);

                VerifyMocks();
            }
        }

        public class GetVariantAsync : ExperimentManagerFixture
        {
            [Fact]
            public async Task Return_variant_from_variant_id_in_request_cookie()
            {
                string variantId = "variantid123";
                var expectedVariant = TestData.Create<Variant>(v => v.VariantId = variantId);

                _cookieServiceMock.Setup(m => m.TryGetVariantCookie("content", "experimentid123", out variantId))
                    .Returns(true)
                    .Verifiable();
                _variantRepositoryMock.Setup(m => m.GetByPublicIdAsync(variantId))
                    .ReturnsAsync(expectedVariant)
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualVariant = await manager.GetVariantAsync("content", "experimentid123");

                Assert.Same(expectedVariant, actualVariant);

                VerifyMocks();
            }

            [Fact]
            public async Task Return_null_if_variant_id_in_request_cookie_is_invalid()
            {
                string badVariantId = "badvariantid456";

                _cookieServiceMock.Setup(m => m.TryGetVariantCookie("content", "experimentid123", out badVariantId))
                    .Returns(true)
                    .Verifiable();
                _variantRepositoryMock.Setup(m => m.GetByPublicIdAsync(badVariantId))
                    .ReturnsAsync((Variant)null)
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualVariant = await manager.GetVariantAsync("content", "experimentid123");

                Assert.Null(actualVariant);

                VerifyMocks();
            }

            [Fact]
            public async Task Return_null_if_no_variant_id_request_cookie()
            {
                string noVariantId;

                _cookieServiceMock.Setup(m => m.TryGetVariantCookie("content", "experimentid123", out noVariantId))
                    .Returns(false)
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualVariant = await manager.GetVariantAsync("content", "experimentid123");

                Assert.Null(actualVariant);

                VerifyMocks();
            }
        }

        public class SetVariantAsync : ExperimentManagerFixture
        {
            [Fact]
            public async Task Use_variant_id_if_it_is_present_in_widget_list()
            {
                Variant variant = TestData.Create<Variant>(v =>
                {
                    v.VariantId = "variant123";
                    v.ContentItemId = "widget1";
                });

                _cookieServiceMock.Setup(m => m.AddVariantCookie("content", "experimentflowpart123", "variant123"))
                    .Verifiable();

                var manager = CreateExperimentManager();

                string actualContentItemId = await manager.SetVariantAsync(variant, SampleFlowPart, "content", "experimentflowpart123");

                Assert.Equal(variant.ContentItemId, actualContentItemId);

                VerifyMocks();
            }

            [Fact]
            public async Task Create_variant_if_variant_is_null()
            {
                string expectedContentItemId = "widget2";
                var flowPart = SampleFlowPart;

                _contentBalancerMock.Setup(m => m.GetRandomIndex(flowPart.Widgets))
                    .Returns(1)
                    .Verifiable();
                _variantRepositoryMock.Setup(m => m.CreateAsync(expectedContentItemId))
                    .ReturnsAsync(TestData.Create<Variant>(v =>
                    {
                        v.VariantId = "variant123";
                        v.ContentItemId = expectedContentItemId;
                    }))
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.AddVariantCookie("content", "experimentflowpart123", "variant123"))
                    .Verifiable();

                var manager = CreateExperimentManager();

                string actualContentItemId = await manager.SetVariantAsync(null, flowPart, "content", "experimentflowpart123");

                Assert.Equal(expectedContentItemId, actualContentItemId);

                VerifyMocks();
            }

            [Fact]
            public async Task Update_existing_variant_if_variant_id_is_not_present_in_widget_list()
            {
                Variant variant = TestData.Create<Variant>(v =>
                {
                    v.VariantId = "variant123";
                    v.ContentItemId = "badwidget0";
                });
                string expectedContentItemId = "widget2";
                var flowPart = SampleFlowPart;

                _contentBalancerMock.Setup(m => m.GetRandomIndex(flowPart.Widgets))
                    .Returns(1)
                    .Verifiable();
                _variantRepositoryMock.Setup(m => m.UpdateAsync(variant))
                    .Returns(Task.CompletedTask)
                    .Verifiable();
                _cookieServiceMock.Setup(m => m.AddVariantCookie("content", "experimentflowpart123", "variant123"))
                    .Verifiable();

                var manager = CreateExperimentManager();

                string actualContentItemId = await manager.SetVariantAsync(variant, flowPart, "content", "experimentflowpart123");

                Assert.Equal(expectedContentItemId, actualContentItemId);

                VerifyMocks();
            }
        }

        public class CreateEncounterAsync : ExperimentManagerFixture
        {
            [Fact]
            public async Task Create_new_encounter()
            {
                var expectedEncounter = TestData.Create<Encounter>();

                _encounterRepositoryMock.Setup(m => m.CreateAsync("sessionid123", "variantid123"))
                    .ReturnsAsync(expectedEncounter)
                    .Verifiable();

                var manager = CreateExperimentManager();

                var actualEncounter = await manager.CreateEncounterAsync("sessionid123", "variantid123");

                Assert.Same(expectedEncounter, actualEncounter);

                VerifyMocks();
            }
        }
    }
}
