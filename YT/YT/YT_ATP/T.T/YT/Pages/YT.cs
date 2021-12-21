using Microsoft.VisualStudio.TestTools.UnitTesting;
using YT_ATP.Test.Common;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using ReadAndCopy;
using System;

namespace YT_ATP
{
    public class YTMain
    {
        public void ReadAndAssert()
        {
            List<CopyDetails> list = null;
            if (File.Exists(".\\HelperFiles\\YTM.csv"))
                list = new ProcessCopy().ReadAndPopulateList(@".\HelperFiles\YT_URLs.csv");
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
                
                WebHelp.WaitSomeSec(30);
                Assert.IsTrue(string.Equals(text, "Subscribed"), string.Concat(details.UN, " is failed"));
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
    }
}