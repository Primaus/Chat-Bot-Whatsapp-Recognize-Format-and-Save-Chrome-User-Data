using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ChatBot_Whatsapp
{
    class detectarFormato
    {
        static void main_brain(IWebDriver driver, WebDriverWait wait)
        {
            wait.Until(drv => drv.FindElement(By.XPath("//span[contains(@aria-label, 'mensaje no leído') or " +
                "contains(@aria-label, 'mensajes no leídos')] | //div[contains(@aria-label, 'Lista de mensajes')]" +
                "//div[last()][contains(@class, 'message-in')]")));

            try
            {
                driver.FindElement(By.XPath("//span[contains(@aria-label, 'mensaje no leído') or " +
                "contains(@aria-label, 'mensajes no leídos')]//ancestor::div[@class='_3OvU8']")).Click();
            }
            catch { }
        }
        static void Main(string[] args)
        {
            string userData = @"C:\Users\Juan José Martinez B\AppData\Local\Google\Chrome\User Data";

            ChromeOptions options = new ChromeOptions();

            options.AddArguments("--start-maximized");
            options.AddExcludedArgument("enable-automation");
            options.AddArguments("--user-data-dir=" + userData);

            IWebDriver driver = new ChromeDriver(options);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(300));

            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;

            driver.Navigate().GoToUrl(@"https://web.whatsapp.com");

            wait.Until(drv => drv.FindElement(By.Id("side")));

            Thread.Sleep(5000);

            main_brain(driver, wait);

            while (true)
            {
                var textbox = wait.Until(drv => drv.FindElement(By.XPath("/html/body/div[1]/div/div/div[4]/div/footer/div[1]/div/span[2]/div/div[2]/div[1]")));

                string path = "//div[contains(@aria-label, 'Lista de mensajes')]//div[last()][contains(@class, 'message-in')]";

                try
                {
                    driver.FindElement(By.XPath($"{path}//span[@aria-label='Mensaje de voz']"));

                    jse.ExecuteScript($"arguments[0].innerHTML = 'es audio'", textbox);
                }
                catch
                {
                    try
                    {
                        driver.FindElement(By.XPath($"{path}//img[@alt='Sticker sin etiquetas']"));

                        jse.ExecuteScript($"arguments[0].innerHTML = 'es sticker'", textbox);
                    }
                    catch
                    {
                        try
                        {
                            driver.FindElement(By.XPath($"{path}//div[@data-testid='image-thumb']"));

                            jse.ExecuteScript($"arguments[0].innerHTML = 'es imagen'", textbox);
                        }
                        catch
                        {
                            try
                            {
                                driver.FindElement(By.XPath($"{path}//button[@data-testid='document-thumb']"));

                                jse.ExecuteScript($"arguments[0].innerHTML = 'es documento'", textbox);
                            }
                            catch
                            {
                                try
                                {
                                    driver.FindElement(By.XPath($"{path}//img[@data-testid='link-preview-thumbnail-jpeg']"));

                                    jse.ExecuteScript($"arguments[0].innerHTML = 'es link-ubicacion'", textbox);
                                }
                                catch
                                {
                                    try
                                    {
                                        driver.FindElement(By.XPath($"{path}//span[@class='i0jNr selectable-text copyable-text']"));

                                        jse.ExecuteScript($"arguments[0].innerHTML = 'es texto'", textbox);
                                    }
                                    catch
                                    {
                                        jse.ExecuteScript($"arguments[0].innerHTML = 'es otra cosa'", textbox);
                                    }
                                }
                            }
                        }
                    }
                }

                main_brain(driver, wait);
            }

            /*
             <span dir="ltr" class="i0jNr selectable-text copyable-text"><span>Hi</span></span> // texto
             <img alt="Sticker sin etiquetas"                                                   // sticker
             <span aria-label="Mensaje de voz"></span>                                          // audio
             <div role="button" tabindex="0" data-testid="image-thumb"                          // image
             <button data-testid="document-thumb" />                                            // documento
             <img data-testid="link-preview-thumbnail-jpeg"                                     // ubicacion
            */
        }
    }
}
