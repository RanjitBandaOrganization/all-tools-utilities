using System;
using System.Collections.Generic;
using System.Text;

namespace YT_ATP
{
    public static class SearchPage
    {
        public enum Actions
        {
            Click,
            SafeClick,
            Enter,
            Return
        }
        //public static string URL = "https://www.gmail.com.br";

        public static string URL = "https://www.google.com";
        public static string URLYT = "https://www.youtube.com/";
        public static string SignInSelector = "div#gb div.gb_Me > a";
        public static string CreateAccountSelector = "div#view_container div.daaWTb > div > div > div:nth-child(1) > div > button";
        public static string SecondChildSelector = "div#view_container li:nth-child(2) > span.VfPpkd-StrnGf-rymPhb-b9t22c";

        //Create your Google Account
        public static string FirstNameField = "input#firstName";
        public static string LastNameField = "input#lastName";
        public static string UserNameField = "input#username";
        public static string PasswdField = "input[name=\"Passwd\"]";
        public static string ConfPasswdField = "input[name=\"ConfirmPasswd\"]";
        public static string NextButton = "div#accountDetailsNext span";
        public static string PhoneField = "#phoneNumberId";

        //phone SETTINGS
        public static string NextButtonPhoneSettings = "#view_container > div > div > div.pwWryf.bxPAYd > div > div.zQJV3 > div > div.qhFLie > div > div > button > span";

        //dob screen
        public static string MonthSelector = "#month";
        public static string MonthChildSelector = "#month > option:nth-child(2)";

        public static string RecoveryEmailSelector = "#view_container > div > div > div.pwWryf.bxPAYd > div > div.WEQkZc > div > form > span > section > div > div > div.akwVEf.OcVpRe > div.d2CFce.cDSmF.OcVpRe > div > div.aCsJod.oJeWuf > div > div.Xb9hP > input";
        public static string DaySelector = "#day";
        public static string YearSelector = "#year";
        public static string GenderSelector = "#gender";
        public static string MaleGenderChildSelector = "#gender > option:nth-child(3)";
        public static string FemaleGenderChildSelector = "#gender > option:nth-child(2)";
        public static string NextButtonDOB = "#view_container > div > div > div.pwWryf.bxPAYd > div > div.zQJV3 > div > div.qhFLie > div > div > button > span";

        //Agree screen
        public static string SkipButton = "#view_container > div > div > div.pwWryf.bxPAYd > div > div.zQJV3 > div.dG5hZc > div.daaWTb > div > div > button > span";

        //Agree screen
        public static string AgreeButton= "#view_container > div > div > div.pwWryf.bxPAYd > div > div.zQJV3 > div > div.qhFLie > div > div > button > span";

        //CONFIGURATION SETTINGS
        public static string GenderChildSelector = MaleGenderChildSelector;
    }
}
