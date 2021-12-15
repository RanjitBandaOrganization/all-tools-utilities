using YT_ATP.Test.Steps;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;
using OpenQA.Selenium.Interactions;

namespace YT_ATP
{
    public static class WebHelp
    {

        private static IWebDriver webdriver = null;

        public static IWebDriver Webdriver { get => webdriver; set => webdriver = value; }

        public static void StartWebDriver(string browserName)
        {
            Console.WriteLine("Starting webdriver" + browserName + " from " + SetUp.driverPath);

            switch (browserName)
            {
                case "Chrome":
                    {
                        //CHROME----ignore-certificate-errors.png
                        ChromeOptions chromeoptions = new ChromeOptions();
                        chromeoptions.AddArgument("--ignore-certificate-errors");
                        Webdriver = new ChromeDriver(SetUp.driverPath);
                        break;
                    }
                case "IE":
                    {
                        //ReadOnlyDesiredCapabilities capabilities = new ReadOnlyDesiredCapabilities();
                        //capabilities.SetCapability(CapabilityType.AcceptSslCertificates, true);
                        //Webdriver = new InternetExplorerDriver(SetUp.driverPath);
                        break;
                    }
                case "FireFox":
                    {
                        Webdriver = new FirefoxDriver(SetUp.driverPath);
                        break;
                    }
                default: Console.WriteLine(browserName + "driver does not implemented in the startbrowser");
                    break;
            }

            webdriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
            webdriver.Manage().Window.Maximize();
        }

        public static void StopWebDriver()
        {
            Console.WriteLine("Stopping webdriver");

            Webdriver.Close();
            Webdriver.Quit();
        }

        public static void NavigateTo(string URL)
        {
            try
            {
                webdriver.Navigate().GoToUrl(URL);                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        #region "     Waits     "

        public static void WaitSomeSec(int wait)
        {
            try
            {
                Thread.Sleep(wait * 1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static bool isDisplayed(string elementLocator)
        {
            try
            {
                IWebElement webElement = webdriver.FindElement(By.CssSelector(elementLocator));
                return (webElement.Displayed && webElement.Enabled);
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
            catch (InvalidElementStateException ex)
            {
                return false;
            }
            catch (StaleElementReferenceException ex)
            {
                return false;
            }
            catch (TargetInvocationException ex)
            {
                return false;
            }

        }


        public static bool WaitToAppear(string elementLocator)
        {
            try
            {
                int waitTime = 250;
                int maxWaitTime = 20000;
                int startTime = 0;

                while (startTime < maxWaitTime)
                {
                    if(isDisplayed(elementLocator))
                    {
                        return true;
                    }
                    Thread.Sleep(waitTime);
                    startTime += waitTime;
                }
                Console.WriteLine("Element with locator " + elementLocator + "did not appear");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        #endregion
        //public static void SelectWebElementAndClick(string selector)
        //{
        //    try
        //    {
        //        WebHelp.Webdriver.FindElement(By.CssSelector(selector)).Click();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //    }
        //}

        public static void SafeSelectWebElementAndPerformAction(string elementLocator, SearchPage.Actions action)
        {
            try
            {
                WaitToAppear(elementLocator);
                IWebElement webElement = webdriver.FindElement(By.CssSelector(elementLocator));
                if(action == SearchPage.Actions.Click)
                {
                    webElement.Click();
                } 
                else
                {
                    Actions actionsVar = new Actions(webdriver);
                    actionsVar.MoveToElement(webElement);
                    actionsVar.Perform();

                    if (action == SearchPage.Actions.SafeClick)
                        webElement.Click();
                    if (action == SearchPage.Actions.Enter)
                        webElement.SendKeys(Keys.Enter);
                    if (action == SearchPage.Actions.Return)
                        webElement.SendKeys(Keys.Return);

                }
                Console.WriteLine("Selected the element with locator " + elementLocator + " with " + action);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        public static void EnterTextIntoWebElement(string elementLocator, string entry)
        {
            try
            {
                WaitToAppear(elementLocator);
                IWebElement webElement = webdriver.FindElement(By.CssSelector(elementLocator));
                webElement.SendKeys(entry);
                Console.WriteLine(entry + " entered into the element with locator " + elementLocator);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void SelectFromDropDown(string dropDownLocator, string optionLocator)
        {
            try
            {
                SafeSelectWebElementAndPerformAction(dropDownLocator,  SearchPage.Actions.Click);
                SafeSelectWebElementAndPerformAction(optionLocator, SearchPage.Actions.Click);
                Console.WriteLine("Selected the element with locator " + optionLocator + " from dropdown with locator " + dropDownLocator);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public static void firstNameElementInsert(String FirstNameField, String firstName)
        {
            try
            {
                WebHelp.Webdriver.FindElement(By.Id(SearchPage.FirstNameField)).SendKeys(firstName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void lastNameElementInsert(String LastNameField, String lastName)
        {
            try
            {
                WebHelp.Webdriver.FindElement(By.Id(SearchPage.LastNameField)).SendKeys(lastName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void userNameElementInsert(String UserNameField, String userName)
        {
            try
            {
                WebHelp.Webdriver.FindElement(By.Id(SearchPage.UserNameField)).SendKeys(userName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void passwdElementInsert(String PasswdField, String passwd)
        {
            try
            {
                WebHelp.Webdriver.FindElement(By.XPath(PasswdField)).SendKeys(passwd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void confirmpasswdElementInsert(String ConfPasswdField, String passwd)
        {
            try
            {
                WebHelp.Webdriver.FindElement(By.XPath(ConfPasswdField)).SendKeys(passwd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void phoneElementInsert(String PhoneField, String phone)
        {
            try
            {
                WebHelp.Webdriver.FindElement(By.Id(PhoneField)).SendKeys(phone);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
