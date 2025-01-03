using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookingTaskMo
{
    public class Properties
    {

        // PROPERTIES : Verify number of properties and adding to wishlist
        // ===========================================================================================


        // Retrieves the number of properties, scrolls to a specific card, and adds it to the wishlist
        public static void NumerOfPropertiesAndScrolling()
        {
            // Retrieve the total number of properties and print it to the console
            string numberOfProperties = GetNumberOfProperties();
            Console.WriteLine($"Number of properties found: {numberOfProperties}");
            TestSetup.test.Pass($"Number of properties found: {numberOfProperties}");

            // Scroll to the 10th property in the list and highlight it
            IWebElement property10 = ScrollToProperty(9);

            // Get the name of the 10th property and print it
            string propertyName = GetPropertyName(property10);
            Console.WriteLine("Property Name: " + propertyName);
            TestSetup.test.Pass("Property Name: " + propertyName);

            // Add the 10th property to the wishlist and navigate to the new window
            AddToWishlistAndNavToNewWindow(property10);
        }

        // Gets the total number of properties displayed on the page
        public static string GetNumberOfProperties()
        {
            // Wait for the results text element to be visible on the page
            var wait = new WebDriverWait(TestSetup.driver, TimeSpan.FromSeconds(10));
            IWebElement resultsTextElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//h1[@aria-live='assertive']")));

            // Re-locate the results element
            resultsTextElement = TestSetup.driver.FindElement(By.XPath("//h1[@aria-live='assertive']"));
            TestSetup.HighlightElement(resultsTextElement);

            // Extract the number of properties from the text using regex
            string results = resultsTextElement.Text;
            return Regex.Match(results, @"\d+").Value;
        }


        // Scrolls to and highlights a property based on the given index
        public static IWebElement ScrollToProperty(int index)
        {
            // Find the property at the specified index in the list of property cards
            IWebElement property = TestSetup.driver.FindElements(By.CssSelector("[data-testid='property-card']")).ElementAt(index);

            // Scroll to the property and highlight it
            TestSetup.ScrollElement(property);
            TestSetup.HighlightElement(property);

            return property;
        }

        // Extracts the name of the specified property
        public static string GetPropertyName(IWebElement propertyElement)
        {
            // Find and return the name of the property from the given property element
            IWebElement propertyNameElement = propertyElement.FindElement(By.CssSelector(".f6431b446c.a15b38c233"));
            return propertyNameElement.Text;
        }

        // Adds a property to the wishlist and navigates to the wishlist page in a new window
        public static void AddToWishlistAndNavToNewWindow(IWebElement propertyElement)
        {
            // Get the current window handle to switch back later
            string originalWindow = TestSetup.driver.CurrentWindowHandle;

            // Click the heart icon to add the property to the wishlist
            IWebElement heartIcon = propertyElement.FindElement(By.CssSelector("[data-testid='wishlist-button']"));
            heartIcon.Click();

            // Wait for the wishlist popover to be visible within 10 seconds
            var wait = new WebDriverWait(TestSetup.driver, TimeSpan.FromSeconds(10));

            // Find the wishlist popover element once it becomes visible
            IWebElement wishlistPopover = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("[data-testid='wishlist-popover-content']")));

            // Find the link inside the popover to view the wishlist
            IWebElement link = wishlistPopover.FindElement(By.CssSelector("a"));
            Thread.Sleep(1000);
            TestSetup.HighlightElement(link);
            link.Click();

            // Switch to the new window that opened after clicking the wishlist link
            TestSetup.SwitchToNewWindow(originalWindow);
        }



    }
}
