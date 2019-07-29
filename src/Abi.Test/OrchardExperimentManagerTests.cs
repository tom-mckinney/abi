using Abi.Data;
using Abi.Models;
using Abi.OrchardCore;
using Abi.Services;
using Abi.Test.Fixtures;
using Moq;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Abi.Test
{
    public class OrchardExperimentManagerTests
    {
        public class Integration : ExperimentManagerFixture
        {
            private class OrchardExperimentManagerSpy : OrchardExperimentManager
            {
                private readonly string _expectedContentItemId;

                public OrchardExperimentManagerSpy(string expectedContentItemId) : base(null, null, null, null, null, null)
                {
                    _expectedContentItemId = expectedContentItemId;
                }

                public Visitor Visitor => TestData.Create<Visitor>(v =>
                {
                    v.VisitorId = "visitorid123";
                    v.UserId = 14;
                });
                public Session Session => TestData.Create<Session>(s =>
                {
                    s.SessionId = "sessionid123";
                    s.VisitorId = Visitor.VisitorId;
                });
                public Variant Variant => TestData.Create<Variant>(v =>
                {
                    v.VariantId = "variantid123";
                    v.ContentItemId = _expectedContentItemId;
                });
                public Encounter Encounter => TestData.Create<Encounter>(e =>
                {
                    e.EncounterId = "encounterid123";
                    e.SessionId = Session.SessionId;
                });

                public bool CalledGetOrCreateVisitorAsync { get; set; }
                public override Task<Visitor> GetOrCreateVisitorAsync()
                {
                    CalledGetOrCreateVisitorAsync = true;
                    return Task.FromResult(Visitor);
                }

                public bool CalledGetOrCreateSessionAsync { get; set; }
                public override Task<Session> GetOrCreateSessionAsync(string visitorId)
                {
                    Assert.Equal(Visitor.VisitorId, visitorId);
                    CalledGetOrCreateSessionAsync = true;
                    return Task.FromResult(Session);
                }

                public bool CalledGetVariantAsync { get; set; }
                public override Task<Variant> GetVariantAsync(string zone, string experimentId)
                {
                    CalledGetVariantAsync = true;
                    return Task.FromResult(Variant);
                }

                public bool CalledSetVariantAsync { get; set; }
                public override Task<Variant> SetVariantAsync(Variant variant, FlowPart content, string zone, string experimentId)
                {
                    CalledSetVariantAsync = true;
                    return Task.FromResult(Variant);
                }

                public bool CalledCreateEncounterAsync { get; set; }
                public override Task<Encounter> CreateEncounterAsync(string sessionId, string variantId)
                {
                    Assert.Equal(Session.SessionId, sessionId);
                    Assert.Equal(Variant.VariantId, variantId);
                    CalledCreateEncounterAsync = true;
                    return Task.FromResult(Encounter);
                }
            }

            [Fact]
            public async Task All_methods_called()
            {
                var flowPart = SampleFlowPart;
                string expectedContentItemId = flowPart.Widgets.Last().ContentItemId;

                var managerSpy = new OrchardExperimentManagerSpy(expectedContentItemId);

                var filteredFlowPart = await managerSpy.GetOrSetVariantAsync(SampleFlowPart);

                Assert.True(managerSpy.CalledGetOrCreateVisitorAsync);
                Assert.True(managerSpy.CalledGetOrCreateSessionAsync);
                Assert.True(managerSpy.CalledGetVariantAsync);
                Assert.True(managerSpy.CalledSetVariantAsync);
                Assert.True(managerSpy.CalledCreateEncounterAsync);

                var variantContentItem = Assert.Single(filteredFlowPart.Widgets);
                Assert.Equal(expectedContentItemId, variantContentItem.ContentItemId);
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

                Variant actualVariant = await manager.SetVariantAsync(variant, SampleFlowPart, "content", "experimentflowpart123");

                Assert.Equal(variant.ContentItemId, actualVariant.ContentItemId);

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

                Variant actualVariant = await manager.SetVariantAsync(null, flowPart, "content", "experimentflowpart123");

                Assert.Equal(expectedContentItemId, actualVariant.ContentItemId);

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

                Variant actualVariant = await manager.SetVariantAsync(variant, flowPart, "content", "experimentflowpart123");

                Assert.Equal(expectedContentItemId, actualVariant.ContentItemId);

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
