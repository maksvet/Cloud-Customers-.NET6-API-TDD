using CloudCustomers.API.Models;
using CloudCustomers.API.Services;
using CloudCustomers.UnitTests.Fixtures;
using CloudCustomers.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq.Protected;
using Moq;
using System.Threading;
using FluentAssertions;
using Microsoft.Extensions.Options;
using CloudCustomers.API.Config;

namespace CloudCustomers.UnitTests.Systems.Services
{
    public class TestUsersService
    {
        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest()
        {
            //arrange
            var expectedResponse = UsersFixture.GetTestUsers();
            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourseList(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com";
            var config = Options.Create(new UsersApiOptions
            {
                Endpoint = endpoint
            });
            var sut = new UsersService(httpClient, config);

            //act

            await sut.GetAllUsers();

            //assert
            handlerMock
                .Protected()
                .Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
                );


        }

        [Fact]
        public async Task GetAllUsers_WhenHits404_ReturnsEmptyListOfUsers()
        {
            //arrange
            
            var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com";
            var config = Options.Create(new UsersApiOptions
            {
                Endpoint = endpoint
            });
            var sut = new UsersService(httpClient, config);
            
            //act

            var result = await sut.GetAllUsers();
            //assert
            result.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetAllUsers_WhenCalled_ReturnsListOfUsersOfExpectedSize()
        {
            //arrange
            var expectedResponse = UsersFixture.GetTestUsers();
            var handlerMock = MockHttpMessageHandler<User>
                .SetupBasicGetResourseList(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com";
            var config = Options.Create(new UsersApiOptions
            {
                Endpoint = endpoint
            });
            var sut = new UsersService(httpClient, config);

            //act

            var result = await sut.GetAllUsers();
            //assert
            result.Count.Should().Be(expectedResponse.Count);
        }
        
        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
        {
            //arrange
            var expectedResponse = UsersFixture.GetTestUsers();
            var endpoint = "https://example.com/users";
            var handlerMock = MockHttpMessageHandler<User>
                .SetupBasicGetResourseList(expectedResponse, endpoint);

            var httpClient = new HttpClient(handlerMock.Object);

            var config = Options.Create(new UsersApiOptions
            {
                Endpoint = endpoint
            });
            var sut = new UsersService(httpClient, config);

            //act

            var result = await sut.GetAllUsers();
            var uri = new Uri(endpoint);
            //assert
            handlerMock
                .Protected()
                .Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>
                    (
                    req => req.Method == HttpMethod.Get
                    && req.RequestUri == uri),
                    
                ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}
