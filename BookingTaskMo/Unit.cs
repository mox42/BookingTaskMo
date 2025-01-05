using NUnit.Framework;
using OpenQA.Selenium;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;

namespace BookingTaskMo
{
    [TestFixture]
    public class Unit
    {

        // =========================================================================================== 
        // CONFIGURATION
        // ===========================================================================================

        // Create an instance of ExtentReports to manage the test report
        public static ExtentReports extentReports = new ExtentReports();

        // Create an HTML reporter that saves the test report to the specified path
        public static ExtentHtmlReporter reporter = new ExtentHtmlReporter("C:\\Users\\Moham\\source\\repos\\BookingTaskMo\\BookingTaskMo\\Report\\");

        // This method runs once before any tests in the class
        [OneTimeSetUp]
        public static void SetUpBeforeClass()
        {
            // Attach the HTML reporter to the ExtentReports instance to generate the test report
            extentReports.AttachReporter(reporter);

            // Open the WebDriver to start the browser session for tests
            TestSetup.OpenDriver();


        }

        // This method runs once after all tests in the class
        [OneTimeTearDown]
        public static void CleanUpAfterClass()
        {
            // Finalize and save the report to the specified location
            extentReports.Flush();

            // Quit the WebDriver to close the browser and end the session
            TestSetup.driver.Quit();
        }


        // ===========================================================================================
        // TEST CASES
        // ===========================================================================================


        // -------------------------------Verify Website Title----------------------------------------

        [Test, Order(1)]
        public void TitleText()
        {
            // Create a new test entry in the ExtentReports instance with the title "Verify Website Title"
            var test = extentReports.CreateTest("Verify Website Title");

            try
            {
                TestSetup.NavigateToURL();
                TestSetup.ClosePopUp();

                IWebElement TextHeader = TestSetup.driver.FindElement(By.XPath("//header/h1"));
                TestSetup.HighlightElement(TextHeader);
                string ExpectedResult = TextHeader.Text;
                string ActualResult = "Find your next stay";

                // Assert that the actual result matches the expected result
                // If they don't match, an assertion exception will be thrown
                Assert.AreEqual(ExpectedResult, ActualResult,$"Actual value '{ActualResult}' does not match expected value '{ExpectedResult}'");
                
                // Log the test as successful in the report.
                test.Pass($"Verify Website Title Passed");
            }
            catch (Exception ex)
            {
                // Log the failure in the test report, including the exception message
                test.Fail($"Test Failed, For more info: {ex.Message}");

                // Take a screenshot of the current browser state to help diagnose the issue
                string fullPath = TestSetup.TakeScreenShot();

                // Add the screenshot to the test report for better visualization
                test.AddScreenCaptureFromPath(fullPath);
            }
        }



        // --------------------------Select City, Date, Number and Search-----------------------------

        [Test, Order(2)]
        public void SelectSearch()
        {
            TestSetup.ExecuteWithHandling(() => SelectandSearch.SelectScity(), "Verify Selecting City");
            TestSetup.ExecuteWithHandling(() => SelectandSearch.SelectDateRange("2025-12-24", "2025-12-29"), "Verify Select Date");
            TestSetup.ExecuteWithHandling(() => SelectandSearch.ConfigureOccupancy(1, 0, 2, true), "Verify Selecting Occupancy");
            TestSetup.ExecuteWithHandling(() => SelectandSearch.SearchBtn(), "Verify Search Button");
        }



        // ----------------------Verify number of properties and adding to wishlist-------------------

        [Test, Order(3)]
        public void SelectProperties()
        {
            TestSetup.ExecuteWithHandling(() => Properties.NumerOfPropertiesAndScrolling(), "Verify number of properties and adding to wishlist");
        }

        // ----------------------Verify Price, Card, Amount and Reserve-------------------------------

        [Test, Order(4)]
        public void ViewandReserve()
        {
            TestSetup.ExecuteWithHandling(() => Reserve.ViewPrice(700), "Compare Price");
            TestSetup.ExecuteWithHandling(() => Reserve.ViewCard(), "Click on card");
            TestSetup.ExecuteWithHandling(() => Reserve.ReserveButton(), "Verify Reserve Button");
            TestSetup.ExecuteWithHandling(() => Reserve.SelectFirstRowThirdindexDropDown(), "Verify Selecting the 3rd option by index in amount dropdown box");
            TestSetup.ExecuteWithHandling(() => Reserve.IWillReserveButton(), "Verify I Will Reserve Button");
        }

    }
        // ===========================================================================================


}
