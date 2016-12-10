using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using Rhino.Mocks;

namespace Tests
{
    [TestClass]
    public class Loading
    {
        public BitRippleRepository Repository { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Repository = Container.GetRepository();
        }

        [TestMethod]
        public void ShouldFailLoading()
        {
            Repository.Context.Data.Stub(x => x.LoadDownloads()).Throw(new Exception());
            Repository.Context.Data.Stub(x => x.LoadFilters()).Throw(new Exception());

            Assert.AreEqual(false, Repository.Data.Load().Success);
            Assert.AreEqual(2, Repository.Data.Load().Errors.Count);
        }

        [TestMethod]
        public void ShouldNotFail()
        {
            Assert.AreEqual(true, Repository.Data.Load().Success);
            Assert.AreEqual(0, Repository.Data.Load().Errors.Count);
        }
    }
}