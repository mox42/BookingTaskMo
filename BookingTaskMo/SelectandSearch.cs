using OpenQA.Selenium;
using System.Globalization;


namespace BookingTaskMo
{
    public class SelectandSearch
    {

        // SELECTANDSEARCH : Select City, Date, Number and Search
        // ===========================================================================================

        // Method to select a specific city from a dropdown box
        public static void SelectScity()
        {
            // Locate the city dropdown element using its unique ID
            IWebElement dropdownElement = TestSetup.driver.FindElement(By.Id(":rh:"));
            TestSetup.HighlightElement(dropdownElement);
            TestSetup.ScrollElement(dropdownElement);
            dropdownElement.Click();

            // Get all the options in the dropdown
            List<IWebElement> options = new List<IWebElement>(dropdownElement.FindElements(By.XPath("//li[@role='option']")));

            // Define the search term for the city to be selected
            string searchTerm = "Aqaba";

            // Iterate through the options to find and select the city matching the search term
            foreach (var option in options)
            {
                // Trim the text of the option
                string optionText = option.Text.Trim();

                // Split the option text by newline and compare the first part (city name)
                string city = optionText.Split('\n')[0].Trim();

                if (city == searchTerm)
                {
                    TestSetup.ScrollElement(option);
                    TestSetup.HighlightElement(option);

                    // Click on the matching city option
                    option.Click();
                    Console.WriteLine($"Option '{searchTerm}' selected.");
                    break;  // Exit the loop once the city is found and selected
                }
                //Console.WriteLine($"{option.Text}");  // Print option text for debugging
            }
        }

        // Method to select a date range by specifying 'from' and 'to' dates.
        public static void SelectDateRange(string fromDateStr, string toDateStr)
        {
            // Parse the 'from' and 'to' dates using the specified format
            DateTime fromDate = DateTime.ParseExact(fromDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(toDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            // To select a specific date from the calendar
            void SelectSpecificDate(DateTime targetDate)
            {
                string targetMonth = targetDate.ToString("MMMM yyyy", CultureInfo.InvariantCulture);  // Get the target month
                string targetDay = targetDate.Day.ToString();  // Get the target day

                while (true)
                {
                    // Get the current visible month from the calendar
                    IWebElement currentMonthElement = TestSetup.driver.FindElement(By.CssSelector("h3.e1eebb6a1e.ee7ec6b631"));
                    string currentMonth = currentMonthElement.Text;

                    // If the current month matches the target month, break the loop
                    if (currentMonth == targetMonth)
                    {
                        break;
                    }

                    // Click the "Next" button to move to the next month
                    IWebElement nextButton = TestSetup.driver.FindElement(By.CssSelector("button[aria-label='Next month']"));
                    TestSetup.HighlightElement(nextButton);
                    nextButton.Click();
                    //Console.WriteLine("Attempting to locate month: " + currentMonth);
                }

                // Select the specific day corresponding to the target date
                By dayElementLocator = By.CssSelector($"span[data-date='{targetDate.ToString("yyyy-MM-dd")}']");
                IWebElement dayElement = TestSetup.WaitUntilElementIsClickable(dayElementLocator, 10);
                dayElement.Click();
            }

            // Select the 'from' and 'to' dates by calling the helper function for each
            SelectSpecificDate(fromDate);
            SelectSpecificDate(toDate);
        }


        // Method to configure occupancy details such as adults, children, rooms, and pets
        public static void ConfigureOccupancy(int adults, int children, int rooms, bool withPets = false)
        {
            // Click the "Occupancy" button to open the occupancy configuration section
            TestSetup.driver.FindElement(By.CssSelector("button[data-testid='occupancy-config']")).Click();

            // Click on labels to activate the inputs and then set values for adults, children, and rooms
            TestSetup.ClickLabel("group_adults");
            SetValue("input#group_adults", adults);

            TestSetup.ClickLabel("group_children");
            SetValue("input#group_children", children);

            TestSetup.ClickLabel("no_rooms");
            SetValue("input#no_rooms", rooms);

            // If traveling with pets true, it will click on the "pets" label to select it
            if (withPets)
            {
                TestSetup.ClickLabel("pets");
            }

            // Click the "Done" button to save the occupancy configuration
            By doneBtnLocator = By.CssSelector("button.a83ed08757.c21c56c305.bf0537ecb5");
            IWebElement doneBtn = TestSetup.WaitUntilElementIsClickable(doneBtnLocator, 10);
            TestSetup.HighlightElement(doneBtn);

            doneBtn.Click(); 
        }


        // Helper method to set values for occupancy input fields such as adults, children, and rooms
        private static void SetValue(string cssSelector, int targetValue)
        {
            // Locate the input field using its CSS selector
            IWebElement input = TestSetup.driver.FindElement(By.CssSelector(cssSelector));

            // Get the current value of the input field
            int currentValue = int.Parse(input.GetAttribute("value"));

            // Adjust the value by simulating key presses to increase or decrease it
            while (currentValue < targetValue)
            {
                input.SendKeys(Keys.ArrowRight);
                currentValue++;
            }
            while (currentValue > targetValue)
            {
                input.SendKeys(Keys.ArrowLeft);
                currentValue--;
            }
        }

        // Method to click the "Search" button to initiate a search
        public static void SearchBtn()
        {
            IWebElement searchButton = TestSetup.driver.FindElement(By.XPath("//button[span[text()='Search']]"));
            TestSetup.HighlightElement(searchButton);
            searchButton.Click();
        }



    }
}
