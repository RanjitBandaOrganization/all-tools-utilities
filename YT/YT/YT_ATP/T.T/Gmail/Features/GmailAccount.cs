using Microsoft.VisualStudio.TestTools.UnitTesting;
////////using YT_ATP.Test.Common;
using System.Threading;
using OpenQA.Selenium;
using System;
////////using System.Configuration;
////////using YT_ATP.T.C.Webdrivers;

namespace YT_ATP
{
    [TestClass]
    public class GmailAccount
    {
        [TestMethod]
        public void Step0_BasicTestMethod()
        {
            /*WebHelp.StartWebDriver("Chrome");

            WebHelp.WaitSomeSec(2);
            
            WebHelp.NavigateTo("https://www.youtube.com/");

            WebHelp.WaitSomeSec(5);

            WebHelp.StopWebDriver();

            Thread.Sleep(1);

            WebHelp.StartWebDriver("IE");

            Thread.Sleep(1);

            WebHelp.StopWebDriver();*/

            Thread.Sleep(1);

            WebHelp.StartWebDriver("FireFox");

            Thread.Sleep(3);

            WebHelp.StopWebDriver();
        }

        [TestMethod]
        public void Step1_CreateAccount()
        {
            //Settings personSettings = new DSettings("1901");
            //Settings personSettings = new MSettings("1903");
            //Settings personSettings = new
            //WebHelp.StartWebDriver("Chrome");
            WebHelp.StartWebDriver("FireFox");

            WebHelp.WaitSomeSec(2);
            WebHelp.NavigateTo(SearchPage.URL);
            WebHelp.WaitSomeSec(5);
            WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.SignInSelector, SearchPage.Actions.Click);
            WebHelp.WaitSomeSec(8);
            WebHelp.SelectFromDropDown(SearchPage.CreateAccountSelector, SearchPage.SecondChildSelector);
            WebHelp.WaitSomeSec(8);

            ////////WebHelp.EnterTextIntoWebElement(SearchPage.FirstNameField, personSettings.FirstName);
            ////////WebHelp.EnterTextIntoWebElement(SearchPage.LastNameField, personSettings.LastName);
            ////////WebHelp.EnterTextIntoWebElement(SearchPage.UserNameField, personSettings.UserName);
            ////////WebHelp.EnterTextIntoWebElement(SearchPage.PasswdField, personSettings.Password);
            ////////WebHelp.EnterTextIntoWebElement(SearchPage.ConfPasswdField, personSettings.Password);
            //WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.NextButton, SearchPage.Actions.Click);
            WebHelp.WaitSomeSec(30);

            ////////WebHelp.EnterTextIntoWebElement(SearchPage.PhoneField, personSettings.PhoneNumber);
            //WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.NextButtonPhoneSettings, SearchPage.Actions.Click);


            WebHelp.WaitSomeSec(30);
            WebHelp.SelectFromDropDown(SearchPage.MonthSelector, SearchPage.MonthChildSelector);

            ////////WebHelp.EnterTextIntoWebElement(SearchPage.RecoveryEmailSelector, personSettings.RecoveryMailAddress);
            ////////WebHelp.EnterTextIntoWebElement(SearchPage.DaySelector, personSettings.BirthDay);
            ////////WebHelp.EnterTextIntoWebElement(SearchPage.YearSelector, personSettings.BirthYear);
            ////////WebHelp.SelectFromDropDown(SearchPage.GenderSelector, (string.Equals(personSettings.Gender,"Male") ?SearchPage.MaleGenderChildSelector :SearchPage.FemaleGenderChildSelector));

            ////WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.NextButtonDOB, SearchPage.Actions.Click);

            ////////WebHelp.WaitSomeSec(60);

            ////////WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.SkipButton, SearchPage.Actions.Click);

            ////////WebHelp.WaitSomeSec(20);

            ////////WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.AgreeButton, SearchPage.Actions.Click);

            WebHelp.WaitSomeSec(30);
            /////NOT REQUIRED RIGHT NOW
            ////////////WebHelp.NavigateTo(SearchPage.URL);
            ////////////WebHelp.WaitSomeSec(5);
            ////////////WebHelp.EnterTextIntoWebElement("input#search", "KarVe Castle");
            ////////////WebHelp.SafeSelectWebElementAndPerformAction("button#search-icon-legacy > yt-icon", SearchPage.Actions.Click);
            ////////////WebHelp.WaitSomeSec(5);
            ////////////WebHelp.SafeSelectWebElementAndPerformAction("#subscribe-button > ytd-subscribe-button-renderer > tp-yt-paper-button > yt-formatted-string", SearchPage.Actions.Click);
            WebHelp.WaitSomeSec(30);
            //WebHelp.ITakeScreenShot();
            WebHelp.StopWebDriver();
        }

        [Ignore]
        [TestMethod]
        public void GmailAccountCreate()
        {

            //generate random data for testes
            Random r = new Random();

            String firstName = $"FirstName{r.Next()}";
            String lastName = $"LastName{r.Next()}";
            String userName = $"UserName{r.Next()}";
            String passwd = $"PassWd{r.Next()}";
            String phone = "11999995555";

            Console.WriteLine("First Name is: " + firstName);
            Console.WriteLine("Last Name is: " + lastName);
            Console.WriteLine("User Name is: " + userName);
            Console.WriteLine("Passowrd is: " + passwd);

            WebHelp.StartWebDriver("Chrome");

            WebHelp.WaitSomeSec(2);

            WebHelp.NavigateTo(SearchPage.URL);

            WebHelp.WaitSomeSec(2);

            WebHelp.Webdriver.FindElement(By.XPath("//*[@id='view_container']/div/div/div[2]/div/div[2]/div/div[2]/div/div/content/span")).Click();
            WebHelp.WaitSomeSec(2);
            WebHelp.firstNameElementInsert(SearchPage.FirstNameField, firstName);
            WebHelp.lastNameElementInsert(SearchPage.LastNameField, lastName);
            WebHelp.userNameElementInsert(SearchPage.UserNameField, userName);
            WebHelp.passwdElementInsert(SearchPage.UserNameField, passwd);
            WebHelp.confirmpasswdElementInsert(SearchPage.ConfPasswdField, passwd);

            //botão proxima
            //WebHelp.Webdriver.FindElement(By.XPath("//*[@id='accountDetailsNext']/content/span")).Click();

            //WebHelp.phoneElementInsert(SearchPage.PhoneField, phone);

            //WebHelp.Webdriver.FindElement(By.XPath("//*[@id='gradsIdvPhoneNext']/content/span")).Click();
            
            /*WebHelp.Webdriver.FindElement(By.Id("day")).SendKeys("01");

            WebHelp.Webdriver.FindElement(By.XPath("//*[@id='month']")).Click();
            WebHelp.Webdriver.FindElement(By.XPath("//*[@id='month']/option[13]")).Click();

            WebHelp.Webdriver.FindElement(By.Id("year")).SendKeys("1980");

            WebHelp.Webdriver.FindElement(By.XPath("//*[@id='gender']")).Click();
            WebHelp.Webdriver.FindElement(By.XPath("//*[@id='gender']/option[2]")).Click();

            WebHelp.Webdriver.FindElement(By.XPath("//*[@id='personalDetailsNext']/content/span")).Click();
            WebHelp.Webdriver.FindElement(By.XPath("//*[@id='view_container']/form/div[2]/div/div[3]/div[1]/div[2]/button")).Click();

            WebHelp.Webdriver.FindElement(By.XPath("//body/div/div/div[2]/div/div[2]/form/div[2]/div/div/div/div/div/div/content/span")).Click();
            WebHelp.Webdriver.FindElement(By.XPath("//body/div/div/div[2]/div/div[2]/form/div[2]/div/div/div/div/div/div/content/span")).Click();
            WebHelp.Webdriver.FindElement(By.XPath("//body/div/div/div[2]/div/div[2]/form/div[2]/div/div/div/div/div/div/content/span")).Click();

            WebHelp.Webdriver.FindElement(By.XPath("//*[@id='termsofserviceNext']/content/span")).Click();

            WebHelp.Webdriver.FindElement(By.XPath("//body/div[22]/div[3]/button")).Click();
            
            /*WebHelp.Webdriver.FindElement(By.Id("identifierId")).Click();
            WebHelp.Webdriver.FindElement(By.Id("identifierId")).SendKeys("Teste");*/

            WebHelp.WaitSomeSec(2);

            WebHelp.StopWebDriver();
        }
    }
}