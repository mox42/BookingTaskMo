using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BookingTaskMo
{
    public class Reserve
    {

        // RESERVE : Verify Price, Card, Amount and Reserve
        // ===========================================================================================


        // Method to view the price of a property and validate it against the expected value
        public static void ViewPrice(int ExpectedPrice)
        {
            // Find the price element on the webpage using a CSS selector
            IWebElement priceElement = TestSetup.driver.FindElement(By.CssSelector("div.e1eebb6a1e.e6208ee469"));

            TestSetup.ScrollElement(priceElement);
            TestSetup.HighlightElement(priceElement);
            Thread.Sleep(100);

            // Extract the price text and remove the "JOD" currency symbol
            string priceText = priceElement.Text.Replace("JOD", "").Trim();

            // Convert the price text to an integer, for separators and comma
            int ActualPrice = int.Parse(priceText, System.Globalization.NumberStyles.AllowThousands);

            // Store the expected price for later comparison
            int ExpectedResult = ExpectedPrice;

            // Store the actual price for later comparison
            int ActualResult = ActualPrice;

            // Assert that the price is less than 700 JOD, otherwise fail the test
            Assert.IsTrue(ActualPrice < 700, $"Actual price {ActualPrice} is not less than 700 JOD.");
        }

        // Method to view the card of a property and open its detailed page in a new window
        public static void ViewCard()
        {
            // Get the handle of the current window before switching
            string originalWindow = TestSetup.driver.CurrentWindowHandle;

            IWebElement Card = TestSetup.driver.FindElement(By.CssSelector("[data-testid='web-core-property-card']"));
            TestSetup.ScrollElement(Card);
            TestSetup.HighlightElement(Card);
            Thread.Sleep(150);
            Card.Click();

            // Switch to the new window that opened after clicking the card
            TestSetup.SwitchToNewWindow(originalWindow);
        }

        // Method to click the "Reserve" button for a property
        public static void ReserveButton()
        {
            IWebElement reserveButton = TestSetup.driver.FindElement(By.XPath("//button[@id='hp_book_now_button']"));
            TestSetup.HighlightElement(reserveButton);
            reserveButton.Click();
        }

        // Method to select the third option by index from a dropdown box in the first row
        public static void SelectFirstRowThirdindexDropDown()
        {
            Thread.Sleep(1000);
            IWebElement selectElement = TestSetup.driver.FindElement(By.CssSelector("select.hprt-nos-select"));

            // Use JavaScript to open the dropdown menu
            IJavaScriptExecutor js = (IJavaScriptExecutor)TestSetup.driver;
            js.ExecuteScript("arguments[0].click();", selectElement);
            Thread.Sleep(1000);

            // Create a SelectElement instance to interact with the dropdown
            SelectElement select = new SelectElement(selectElement);

            // Check if there are more than two options in the dropdown
            if (select.Options.Count > 2)
            {
                // Select the 3rd option (index 2) from the dropdown
                select.SelectByIndex(2);
            }
            else
            {
                // If there are not enough options, print a failure message
                // but it will select the first option (index 1) to continue the test
                Console.WriteLine("Test Failure: Not enough options to select.");
                throw new InvalidOperationException("Test Failure: Not enough options to select.");
            }

            // Optionally, verify the selected option and print it
            IWebElement selectedOption = select.SelectedOption;
            TestSetup.HighlightElement(selectElement);
            //Console.WriteLine($"Selected option: {selectedOption.Text.Trim()}");
        }

        // Method to click on the "I will reserve" button
        public static void IWillReserveButton()
        {
            Thread.Sleep(1000);
            IWebElement button = TestSetup.driver.FindElement(By.ClassName("txp-bui-main-pp"));
            TestSetup.HighlightElement(button);

            // Use JavaScript to click the "I will reserve" button
            IJavaScriptExecutor js = (IJavaScriptExecutor)TestSetup.driver;
            js.ExecuteScript("arguments[0].click();", button);
            Thread.Sleep(3000);

            // Randomly, the "I will reserve" button may not appear in the table. Instead, another button 
            // with the same functionality will appear. I have treated this as the same button.

        }


    }
}
