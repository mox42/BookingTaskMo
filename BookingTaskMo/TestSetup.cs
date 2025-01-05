using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;


namespace BookingTaskMo
{
    public class TestSetup
    {

        // ===========================================================================================
        // TESTSETUP
        // ===========================================================================================
        // This class contains all frequently used methods for the test
        // It assist with common actions like browser setup, navigation, and element interaction
        // ===========================================================================================

        //****Update note*****
        //I find out that the test wil run ok without all thread.sleep, so i removed some and changed others to Dynamic wait
        //****Update note*****

        // WebDriver instance for managing browser interaction.
        public static IWebDriver driver = new ChromeDriver();

        // URL of the website to be tested.
        public static string URL = "http://booking.com";

        // This property will store the current ExtentTest object used for logging test results
        public static ExtentTest test = null!;

        // This method initializes the 'test' property with the provided ExtentTest instance
        public static void InitializeTest(ExtentTest extentTest)
        {
            test = extentTest;
        }

        // Method to open the browser and maximize the window.
        public static void OpenDriver()
        {
            driver.Manage().Window.Maximize();
        }

        // Method to navigate to the specified URL.
        public static void NavigateToURL()
        {
            driver.Navigate().GoToUrl(URL);
        }

        // Method to handle and close the pop-up on the webpage (On startup case)
        public static void ClosePopUp()
        {
            // Create a WebDriverWait object to wait for up to 10 seconds.
            WebDriverWait wait = new WebDriverWait(TestSetup.driver, TimeSpan.FromSeconds(10));

            try
            {
                // Wait for the pop-up close button to become visible.
                IWebElement closeButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//button[@aria-label='Dismiss sign-in info.']")));
                

                // Highlight the close button for visual confirmation (The method can be found below)
                HighlightElement(closeButton);

                // Click the close button to dismiss the pop-up.
                closeButton.Click();
            }
            catch
            {
                // If an error occurs, refresh the page.
                driver.Navigate().Refresh();

                // Retry closing the pop-up after refreshing the page.
                ClosePopUp();

                /*
                There is a random issue where the pop-up does not load normally, and if it is processed later, it may show up 
                at a random time during execution, To handle this behavior, we need to make sure that the pop-up appears at 
                the beginning of the test, If it doesn't, the page will be refreshed, and the pop-up will be attempted again
                */

            }
        }


        // Method to Click on Element of specific label (method for specific case)
        public static void ClickLabel(string forAttribute)
        {
            // Find the label element by its "for" attribute value
            IWebElement label = TestSetup.driver.FindElement(By.CssSelector($"label[for='{forAttribute}']"));

            // Click the label element
            label.Click();
        }

        public static void CloseDriver()
        {
            // Close the browser and quit the WebDriver session
            driver.Quit();
        }


        // Method to Highlight Element for tracking purposes
        public static void HighlightElement(IWebElement element)
        {
            // Create an instance of JavaScriptExecutor to execute JavaScript in the browser
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;

            // Highlight the element by adding a blue border and background
            executor.ExecuteScript("arguments[0].setAttribute('style', 'border: 2px solid lightskyblue; border-radius: 10px; background-color: lightskyblue;!important')", element);

            // Sleep for 1 second to make the highlight visible
            //Thread.Sleep(1000);


            // Remove the highlight after 1 second
            executor.ExecuteScript("arguments[0].setAttribute('style' , 'background: none !important')", element);
        }

        // Method to create scrolling behavior
        public static void ScrollElement(IWebElement element)
        {
            // Create an instance of JavaScriptExecutor to execute JavaScript in the browser
            IJavaScriptExecutor executor = (IJavaScriptExecutor)driver;

            // Scroll the specified element into view with centered alignment
            executor.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);

        }

        public static IWebElement WaitUntilElementIsClickable(By locator, int timeoutInSeconds)
        {
            // Initialize WebDriverWait with the provided timeout
            var wait = new WebDriverWait(TestSetup.driver, TimeSpan.FromSeconds(timeoutInSeconds));

            // Wait until the element is clickable and return it
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        public static IWebElement WaitUntilElementIsVisible(By locator, int timeoutInSeconds)
        {
            // Initialize WebDriverWait with the provided timeout
            var wait = new WebDriverWait(TestSetup.driver, TimeSpan.FromSeconds(timeoutInSeconds));

            // Wait until the element is visible and return it
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }



        // Method to handle new pop up window or tap
        public static void SwitchToNewWindow(string originalWindow)
        {
            // Create a WebDriverWait instance to wait for the new window to open
            WebDriverWait wait = new WebDriverWait(TestSetup.driver, TimeSpan.FromSeconds(10));

            // Wait until there are more than one window handles (indicating a new window has opened)
            wait.Until(d => d.WindowHandles.Count > 1);

            // Get the list of all window handles and loop through them
            var windowHandles = TestSetup.driver.WindowHandles;
            foreach (string window in windowHandles)
            {
                // If the window handle is not the original window, switch to it
                if (window != originalWindow)
                {
                    TestSetup.driver.SwitchTo().Window(window);
                }
            }
        }



        // Method to capture a screenshot and save it to a predefined directory
        public static string TakeScreenShot()
        {
            // Create an instance of ITakesScreenshot to capture the screenshot
            ITakesScreenshot screenshotTaker = (ITakesScreenshot)driver;
            Screenshot screenshot = screenshotTaker.GetScreenshot();

            // Define the path where the screenshot will be saved
            string path = "C:\\Users\\Moham\\source\\repos\\BookingTaskMo\\BookingTaskMo\\Report\\";

            // Generate a unique file name for the screenshot using GUID
            // and appending "_image.png" to it, ensuring each screenshot has a unique name
            string imageName = Guid.NewGuid().ToString() + "_image.png";

            // Combine the file path and the generated image name to create the full path 
            string fullPath = Path.Combine(path, imageName);


            // Save the screenshot to the specified location
            screenshot.SaveAsFile(fullPath);

            // Return the full path where the screenshot is saved
            return fullPath;
        }



        // This method executes a given action (test case) and handles any exceptions that may occur during execution
        // It logs the results to the extent report, marking the test as either passed or failed based on the outcome
        // All test cases will rely on this handler to ensure consistent reporting and error management
        public static void ExecuteWithHandling(Action action, string testCaseName)
        {
            // Create a new test in the extent report with the given test case name.
            var test = Unit.extentReports.CreateTest(testCaseName);

            // Store the test in the static TestSetup class
            TestSetup.InitializeTest(test);

            try
            {
                // Execute the action (the test steps).
                action();

                // If the action is successful, log the test as passed in the report
                test.Pass($"{testCaseName} Passed");
            }
            catch (Exception ex)
            {
                // If an exception occurs, log the test as failed and include the exception message.
                test.Fail($"Test Failed. For more information: {ex.Message}");

                // Take a screenshot and add it to the report for visual context.
                string fullPath = TestSetup.TakeScreenShot();
                test.AddScreenCaptureFromPath(fullPath);
            }
        }


        // ===========================================================================================


    }
}
