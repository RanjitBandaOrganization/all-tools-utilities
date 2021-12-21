using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReadAndCopy;
using System;
using System.Collections.Generic;
using System.IO;

namespace YT_ATP
{
    [TestClass]
    public class YT
    {
        [TestMethod]
        public void ReadAndAssert()
        {
            List<CopyDetails> list = null;
            if (File.Exists(".\\HelperFiles\\YTM.csv"))
                list = new ProcessCopy().ReadAndPopulateList(@".\HelperFiles\YTM.csv");
            else
                throw new Exception("YTM.csv file doesn't found");
            foreach (CopyDetails details in list)
            {
                WebHelp.StartWebDriver("Chrome");

                WebHelp.WaitSomeSec(2);

                WebHelp.NavigateTo(SearchPage.URLyt);

                WebHelp.WaitSomeSec(2);


                WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.SignInSelectoryt, SearchPage.Actions.Click);
                //WebHelp.EnterTextIntoWebElement(SearchPage.FirstNameField, personSettings.FirstName);
                WebHelp.EnterTextIntoWebElement(SearchPage.UserNameSelectoryt, details.UN);
                WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.UserNameNextSelectoryt, SearchPage.Actions.Click);
                WebHelp.EnterTextIntoWebElement(SearchPage.PasswordSelectoryt, details.PS);
                WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.PasswordNextSelectoryt, SearchPage.Actions.Click);

                WebHelp.EnterTextIntoWebElement(SearchPage.SearchFieldSelectoryt, details.Ser);
                WebHelp.SafeSelectWebElementAndPerformAction(SearchPage.SearchButtonSelectoryt, SearchPage.Actions.Click);

                string text = WebHelp.ReadTextInTheWebElement(SearchPage.SubscribeSelectoryt, "text");

                WebHelp.WaitSomeSec(3);
                Assert.IsTrue(string.Equals(text, "SUBSCRIBED"), string.Concat(details.UN, " is failed"));
                WebHelp.StopWebDriver();

            }

            /*Thread.Sleep(1000);

            WebHelp.StartWebDriver("IE");

            Thread.Sleep(1000);

            WebHelp.StopWebDriver();

            Thread.Sleep(1000);

            WebHelp.StartWebDriver("FireFox");

            Thread.Sleep(1000);

            WebHelp.StopWebDriver();*/
        }


        //[Ignore]
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    WebHelp.StartWebDriver("Chrome");

        //    WebHelp.WaitSomeSec(2);
            
        //    WebHelp.NavigateTo(SearchPage.URLyt);

        //    WebHelp.WaitSomeSec(2);

        //    WebHelp.StopWebDriver();

        //    /*Thread.Sleep(1000);

        //    WebHelp.StartWebDriver("IE");

        //    Thread.Sleep(1000);

        //    WebHelp.StopWebDriver();

        //    Thread.Sleep(1000);

        //    WebHelp.StartWebDriver("FireFox");

        //    Thread.Sleep(1000);

        //    WebHelp.StopWebDriver();*/ 
        //}

        ////[Ignore]
        ////[TestMethod]
        ////public void TestMethod2()
        ////{
        ////    WebHelp.StartWebDriver("Chrome");
        ////    WebHelp.WaitSomeSec(2);
        ////    WebHelp.NavigateTo("https://www.gmail.com/");
        ////    WebHelp.WaitSomeSec(2);
        ////    WebHelp.StopWebDriver();
        ////}
    }
}