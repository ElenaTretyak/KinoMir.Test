using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace KinoMir.Tests

{
    public class ChooseTicketsTest

    {
        [Fact]
        public void ChooseTickets()
        {
            var driver = new ChromeDriver();
            driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            driver.Navigate().GoToUrl("https://www.kino-mir.ru");

            // Go schedule   
            wait.Until(d => driver.FindElement(By.XPath(TestConstants.Schedule)).Displayed);
            var scheduleLocator = driver.FindElement(By.XPath(TestConstants.Schedule));
            Assert.True(scheduleLocator.Displayed);
            scheduleLocator.Click();

            // Assert your city Novosibirsk                   
            wait.Until(d => driver.FindElement(By.XPath(TestConstants.CityButton)).Displayed);
            var cityLocator = driver.FindElement(By.XPath(TestConstants.CityButton));
            Assert.True(cityLocator.Displayed);
            cityLocator.Click();

            // Choose day tomorrow          
            wait.Until(d => driver.FindElement(By.XPath(TestConstants.Tomorrow)).Displayed);
            var tomorrowLocator = driver.FindElement(By.XPath(TestConstants.Tomorrow));
            Assert.True(tomorrowLocator.Displayed);
            tomorrowLocator.Click();

            // Choose seance            
            wait.Until(d => driver.FindElement(By.ClassName(TestConstants.Seance)).Displayed);
            var seanceLocator = driver.FindElement(By.ClassName(TestConstants.Seance));
            Assert.True(seanceLocator.Displayed);
            seanceLocator.Click();

            // Choose two places
            wait.Until(d => driver.FindElement(By.Id("kw-iframe")).Displayed);
            var seatsFrame2 = driver.FindElement(By.Id("kw-iframe"));
            Assert.True(seatsFrame2.Displayed);

            Actions actions = new Actions(driver);
            actions.MoveToElement(seatsFrame2).Click();
            driver.SwitchTo().Frame(seatsFrame2);

            wait.Until(d => driver.FindElement(By.XPath(TestConstants.Canvas)).Displayed);
            var canvas = (WebElement)driver.FindElement(By.XPath(TestConstants.Canvas));
            Assert.True(canvas.Displayed);

            int x = -32;
            int y = -8;

            new Actions(driver)
                    .MoveToElement(canvas, x, y).Click().Perform();
            x += 16;
            y += 16;
            var ticketsChosen = driver.FindElements(By.ClassName(TestConstants.TicketCard));

            do
            {
                new Actions(driver)
                  .MoveToElement(canvas, x, y).Click().Perform();
                x += 16;
                y += 16;
                ticketsChosen = driver.FindElements(By.ClassName(TestConstants.TicketCard));
            }
            while (ticketsChosen.Count < 2);

            // Buy tickets
            wait.Until(d => driver.FindElement(By.ClassName(TestConstants.BuyButton)).Displayed);
            var buyButtonLocator = driver.FindElement(By.ClassName(TestConstants.BuyButton));
            Assert.True(buyButtonLocator.Displayed);
            buyButtonLocator.Click();

            // Click authorization
            wait.Until(d => driver.FindElement(By.XPath(TestConstants.AuthorizationButton)).Displayed);
            var authorizationButtonLocator = driver.FindElement(By.XPath(TestConstants.AuthorizationButton));
            Assert.True(authorizationButtonLocator.Displayed);

            driver.FindElement(By.XPath(TestConstants.AuthorizationButton)).Click();

            string firstTicker = ticketsChosen[0].Text;
            string secondTicker = ticketsChosen[1].Text;

            // Assert
            Assert.NotEmpty(firstTicker);
            Assert.NotEmpty(secondTicker);

            Console.WriteLine(firstTicker);
            Console.WriteLine(secondTicker);           

            driver.Quit();
        }
    }
}
