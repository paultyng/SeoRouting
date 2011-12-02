using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SeoRouting.Tests
{
    [Binding]
    public class RedirectionSteps
    {
        [Given(@"a route url of (.*)")]
        public void GivenARouteUrlOf(string url)
        {
            ScenarioContext.Current["RouteUrl"] = url;
        }

        [Given(@"ForceLowerCase set to (true|false)")]
        public void GivenForceLowerCaseSetTo(bool flc)
        {
            ScenarioContext.Current["FLC"] = flc;
        }

        [Given(@"ForceTrailingSlash is set to (true|false)")]
        public void GivenForceTrailingSlashIsSetTo(bool fts)
        {
            ScenarioContext.Current["FTS"] = fts;
        }

        [Then(@"I should be redirected to (.*)")]
        public void ThenIShouldBeRedirectedTo(string url)
        {
            Assert.AreEqual(url, RedirectionHelper.TestRedirect((string)ScenarioContext.Current["RouteUrl"], (string)ScenarioContext.Current["RequestUrl"], (bool)ScenarioContext.Current["FTS"], (bool)ScenarioContext.Current["FLC"]));
        }

        [Then(@"I should not be redirected")]
        public void ThenIShouldNotBeRedirected()
        {
            Assert.IsNull(RedirectionHelper.TestRedirect((string)ScenarioContext.Current["RouteUrl"], (string)ScenarioContext.Current["RequestUrl"], (bool)ScenarioContext.Current["FTS"], (bool)ScenarioContext.Current["FLC"]));
        }

        [When(@"I request (.*)")]
        public void WhenIRequest(string url)
        {
            ScenarioContext.Current["RequestUrl"] = url;
        }
    }
}
