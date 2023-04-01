using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModernProxyModule.Tests
{

        [TestClass]
        public class CustomProxyTests
        {
            [TestMethod]
            public void Credentials_ReturnsNull_WhenProxyConfigIsNull()
            {
                // Arrange
                var customProxy = new CustomProxy();
                Environment.SetEnvironmentVariable("HTTP_PROXY", null);

                // Act
                var result = customProxy.Credentials;

                // Assert
                Assert.IsNull(result);
            }

            [TestMethod]
            public void Credentials_ReturnsNetworkCredential_WhenProxyConfigIsNotNull()
            {
                // Arrange
                var customProxy = new CustomProxy();
                Environment.SetEnvironmentVariable("HTTP_PROXY", "http://user:password@proxy:8080");

                // Act
                var result = customProxy.Credentials;

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(NetworkCredential));
            }

            [TestMethod]
            public void GetProxy_ReturnsNull_WhenProxyConfigIsNull()
            {
                // Arrange
                var customProxy = new CustomProxy();
                Environment.SetEnvironmentVariable("HTTP_PROXY", null);

                // Act
                var result = customProxy.GetProxy(new Uri("http://www.example.com"));

                // Assert
                Assert.IsNull(result);
            }

            [TestMethod]
            public void GetProxy_ReturnsUri_WhenProxyConfigIsNotNull()
            {
                // Arrange
                var customProxy = new CustomProxy();
                Environment.SetEnvironmentVariable("HTTP_PROXY", "http://user:password@proxy:8080");

                // Act
                var result = customProxy.GetProxy(new Uri("http://www.example.com"));

                // Assert
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Uri));
            }

            [TestMethod]
            public void IsBypassed_ReturnsFalse_WhenHostIsInNoProxyList()
            {
                // Arrange
                var customProxy = new CustomProxy();
                Environment.SetEnvironmentVariable("NO_PROXY", "example.com");

                // Act
                var result = customProxy.IsBypassed(new Uri("http://www.example.com"));

                // Assert
                Assert.IsFalse(result);
            }

            [TestMethod]
            public void IsBypassed_ReturnsFalse_WhenHostIsNotInNoProxyList()
            {
                // Arrange
                var customProxy = new CustomProxy();
                Environment.SetEnvironmentVariable("NO_PROXY", "example.com");

                // Act
                var result = customProxy.IsBypassed(new Uri("http://www.google.com"));

                // Assert
                Assert.IsFalse(result);
            }
        }
    
}
