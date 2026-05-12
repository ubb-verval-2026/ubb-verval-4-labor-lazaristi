using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoTests
{
    private IWebDriver driver = null!;

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void TeardownTest()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void BlazeDemo_MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl("https://blazedemo.com/");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var departureSelect = wait.Until(ExpectedConditions.ElementExists(By.Name("fromPort")));
        departureSelect.FindElement(By.XPath(".//option[. = 'Mexico City']")).Click();

        var destinationSelect = wait.Until(ExpectedConditions.ElementExists(By.Name("toPort")));
        destinationSelect.FindElement(By.XPath(".//option[. = 'Dublin']")).Click();

        // Act
        var findFlightsButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("input[type='submit']")));
        findFlightsButton.Click();

        // Assert
        var flightRows = wait.Until(_ =>
        {
            var rows = driver.FindElements(By.CssSelector("table tbody tr"));
            return rows.Count > 0 ? rows : null;
        });

        flightRows.Should().HaveCountGreaterThanOrEqualTo(3);
    }
}
