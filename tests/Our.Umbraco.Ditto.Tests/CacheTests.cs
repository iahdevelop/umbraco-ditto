﻿using Umbraco.Core;
using Umbraco.Core.Cache;

namespace Our.Umbraco.Ditto.Tests
{
    using NUnit.Framework;
    using Our.Umbraco.Ditto.Tests.Mocks;

    [TestFixture]
    public class CacheTests
    {
        public class MyValueResolverModel1
        {
            [UmbracoProperty(CacheDuration = 5)]
            public string MyProperty1 { get; set; }

            [DittoCache(CacheDuration = 5)]
            public string MyProperty2 { get; set; }

            public string MyProperty3 { get; set; }
        }

        [DittoCache(CacheDuration = 5)]
        public class MyValueResolverModel2
        {
            public string MyProperty1 { get; set; }

            public string MyProperty2 { get; set; }

            public string MyProperty3 { get; set; }
        }

        [Test]
        public void Cache_Caches()
        {
            var cacheHelper = new CacheHelper(
                new ObjectCacheRuntimeCacheProvider(),
                new StaticCacheProvider(),
                new NullCacheProvider());

            var appCtx = new ApplicationContext(cacheHelper);

            ApplicationContext.EnsureContext(appCtx, true);

            var prop1 = new PublishedContentPropertyMock { Alias = "myProperty1", Value = "Test1" };
            var prop2 = new PublishedContentPropertyMock { Alias = "myProperty2", Value = "Test1" };
            var prop3 = new PublishedContentPropertyMock { Alias = "myProperty3", Value = "Test1" };

            var content = new PublishedContentMock
            {
                Id = 1,
                Properties = new[] { prop1, prop2, prop3 }
            };

            var model1 = content.As<MyValueResolverModel1>();
            var model2 = content.As<MyValueResolverModel2>();

            Assert.That(model1.MyProperty1, Is.EqualTo("Test1"));
            Assert.That(model1.MyProperty2, Is.EqualTo("Test1"));
            Assert.That(model1.MyProperty3, Is.EqualTo("Test1"));

            Assert.That(model2.MyProperty1, Is.EqualTo("Test1"));
            Assert.That(model2.MyProperty2, Is.EqualTo("Test1"));
            Assert.That(model2.MyProperty3, Is.EqualTo("Test1"));

            prop1.Value = "Test2";
            prop2.Value = "Test2";
            prop3.Value = "Test2";

            model1 = content.As<MyValueResolverModel1>();
            model2 = content.As<MyValueResolverModel2>();

            Assert.That(model1.MyProperty1, Is.EqualTo("Test1"));
            Assert.That(model1.MyProperty2, Is.EqualTo("Test1"));
            Assert.That(model1.MyProperty3, Is.EqualTo("Test2")); // This one doesn't cache

            Assert.That(model2.MyProperty1, Is.EqualTo("Test1"));
            Assert.That(model2.MyProperty2, Is.EqualTo("Test1"));
            Assert.That(model2.MyProperty3, Is.EqualTo("Test1"));
        }
    }
}