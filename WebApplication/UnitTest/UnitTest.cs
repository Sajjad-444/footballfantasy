using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication;
using WebApplication.Presentation;
using WebApplication.BusinessLogic;

namespace UnitTest;

[TestClass]
public class UnitTest
{
    [TestMethod]
    public void TestFirstNameValidation()
    {
        string[] input = { "aLI", "ali", "12alI", "#$aLi", "jaV./lo09" };
        bool[] expected = { true, true, false, false, false };
        for (int i = 0; i < input.Length; i++)
        {
            Assert.AreEqual(expected[i], InputsValidation.firstNameValidation(input[i]));
        }
    }

    [TestMethod]
    public void TestLastNameValidation()
    {
        string[] input = { "Hassani", "haSSini", "1243HassanI", "#$Hasani.-", "mk,<>oi" };
        bool[] expected = { true, true, false, false, false };
        for (int i = 0; i < input.Length; i++)
        {
            Assert.AreEqual(expected[i], InputsValidation.lastNameValidation(input[i]));
        }
    }

    [TestMethod]
    public void TestUserNameValidation()
    {
        string[] input = { "Hassan1231i", "haS-_Si25253ni", "1243H&&^assanI", "#$Hasani.-", "GGt_r4785-" };
        bool[] expected = { true, true, false, false, true };
        for (int i = 0; i < input.Length; i++)
        {
            Assert.AreEqual(expected[i], InputsValidation.userNameValidation(input[i]));
        }
    }

    [TestMethod]
    public void TestPasswordValidation()
    {
        string[] input = { "1234gdEAchg$$567", "haSS<<>84673&*4KKini", "", "#$Hi.-", ",.<.;>675ju&&" };
        bool[] expected = { true, true, false, false, true };
        for (int i = 0; i < input.Length; i++)
        {
            Assert.AreEqual(expected[i], InputsValidation.passwordValidation(input[i]));
        }
    }

    [TestMethod]
    public void TestEmailAddressValidation()
    {
        string[] input =
        {
            "@", "1@3", "65454@yahoo.com", "ggjg@@@--gmail.com", "hjft.234@aol.com", "hyr7&yandex.com",
            "hj8-9@sbcglobal.net"
        };
        bool[] expected = { false, false, true, false, true, false, true };
        for (int i = 0; i < input.Length; i++)
        {
            Assert.AreEqual(expected[i], InputsValidation.emailAddressValidation(input[i]));
        }
    }

    [TestMethod]

    public void TestSignUpInputValidtions()
    {
        List<User> input = new List<User>();
        input.Add(
            new User()
            {
                firstName = "HLOKibdRYTbh",
                lastName = "klod.nJHy83dM",
                userName = "Kaurinvb8",
                emailAddress = "Hwjii@@gmail.com",
                password = "123@5#"   
            }
        );
        input.Add(
            new User()
            {
                firstName = "HLOK/09^YTbh",
                lastName = "lnd*&JH.y~dM",
                userName = "Kaurinvb8",
                emailAddress = "Hwjilpomnvji@gmail.com",
                password = "123@5#gfh.j"   
            }
        );
        input.Add(
            new User()
            {
                firstName = "HLO17&*(K-=ibdRYTbh",
                lastName = "klodn/JH14.ydM",
                userName = "Kaur./%$invb8",
                emailAddress = "Hwjilpomnvji@gmail.com",
                password = "123@5#gfh.j"   
            }
        );
        input.Add(
            new User()
            {
                firstName = "VolfrAWQ",
                lastName = "LtrdwQE",
                userName = "NuyR-987",
                emailAddress = "H9juEi@outlook.com",
                password = "8%hl7#\\@nhKI\"/"   
            }
        );
        List<messageInfo> expected = new List<messageInfo>();
        expected.Add(
            new messageInfo()
            {
                message = "FirstName is invalid"
            }
        );
        expected.Add(
            new messageInfo()
            {
                message = "LastName is invalid"
            }
        );
        expected.Add(
            new messageInfo()
            {
                message = "UserName is invalid"
            }
        );
       expected.Add(
           new messageInfo()
           {
               message = "Password is invalid"
           }
       );
       expected.Add(
           new messageInfo()
           {
               message = "EmailAddress is invalid"
           }
       );
       expected.Add(
           new messageInfo()
            {
                message = "All inputs are valid" 
            }
       );
       //Test For Input[1]
       for (int i = 0; i < 2; i++)
       {
           Assert.AreEqual(expected[i].message, InputsValidation.signUpInputValidations(input[1])[i].message);
       }
       //Test Fot Input[2]
       for (int i = 0; i < 3; i++)
        {
            Assert.AreEqual(expected[i].message, InputsValidation.signUpInputValidations(input[2])[i].message);
        }
       //Test For Input[0]
       Assert.AreEqual(expected[1].message, InputsValidation.signUpInputValidations(input[0])[0].message);
       Assert.AreEqual(expected[3].message, InputsValidation.signUpInputValidations(input[0])[1].message);
       Assert.AreEqual(expected[4].message, InputsValidation.signUpInputValidations(input[0])[2].message);
       //Test For Input[3]
       Assert.AreEqual(expected[5].message, InputsValidation.signUpInputValidations(input[3])[0].message);
    }
    
}