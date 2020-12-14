using OpenQA.Selenium;
// ReSharper disable UnusedMember.Global

namespace SimpleCore.Selenium
{
	public class Highlight
	{
		// assuming JS is enabled
		private readonly IJavaScriptExecutor m_driver;
		private          IWebElement         m_lastElem   = null;
		private          string              m_lastBorder = null;

		private const string SCRIPT_GET_ELEMENT_BORDER = "/*\n" +
		                                                 " * Returns all border properties of the specified element as String,\n" +
		                                                 " * in order of \"width style color\" delimited by ';' (semicolon) in the form of:\n" +
		                                                 " * \n" +
		                                                 " * \"2px inset #000000;2px inset #000000;2px inset #000000;2px inset #000000\"\n" +
		                                                 " * \"medium none #ccc;medium none #ccc;1px solid #e5e5e5;medium none #ccc\"\n" +
		                                                 " * etc.\n" +
		                                                 " */\n" +
		                                                 "var elem = arguments[0]; \n" +
		                                                 "if (elem.currentStyle) {\n" +
		                                                 "    // Branch for IE 6,7,8. No idea how this works on IE9, but the script\n" +
		                                                 "    // should take care of it.\n" +
		                                                 "    var style = elem.currentStyle;\n" +
		                                                 "    var border = style['borderTopWidth']\n" +
		                                                 "            + ' ' + style['borderTopStyle']\n" +
		                                                 "            + ' ' + style['borderTopColor']\n" +
		                                                 "            + ';' + style['borderRightWidth']\n" +
		                                                 "            + ' ' + style['borderRightStyle']\n" +
		                                                 "            + ' ' + style['borderRightColor']\n" +
		                                                 "            + ';' + style['borderBottomWidth']\n" +
		                                                 "            + ' ' + style['borderBottomStyle']\n" +
		                                                 "            + ' ' + style['borderBottomColor']\n" +
		                                                 "            + ';' + style['borderLeftWidth']\n" +
		                                                 "            + ' ' + style['borderLeftStyle']\n" +
		                                                 "            + ' ' + style['borderLeftColor'];\n" +
		                                                 "} else if (window.getComputedStyle) {\n" +
		                                                 "    // Branch for FF, Chrome, Opera\n" +
		                                                 "    var style = document.defaultView.getComputedStyle(elem);\n" +
		                                                 "    var border = style.getPropertyValue('border-top-width')\n" +
		                                                 "            + ' ' + style.getPropertyValue('border-top-style')\n" +
		                                                 "            + ' ' + style.getPropertyValue('border-top-color')\n" +
		                                                 "            + ';' + style.getPropertyValue('border-right-width')\n" +
		                                                 "            + ' ' + style.getPropertyValue('border-right-style')\n" +
		                                                 "            + ' ' + style.getPropertyValue('border-right-color')\n" +
		                                                 "            + ';' + style.getPropertyValue('border-bottom-width')\n" +
		                                                 "            + ' ' + style.getPropertyValue('border-bottom-style')\n" +
		                                                 "            + ' ' + style.getPropertyValue('border-bottom-color')\n" +
		                                                 "            + ';' + style.getPropertyValue('border-left-width')\n" +
		                                                 "            + ' ' + style.getPropertyValue('border-left-style')\n" +
		                                                 "            + ' ' + style.getPropertyValue('border-left-color');\n" +
		                                                 "}\n" +
		                                                 "// highlight the element\n" +
		                                                 "elem.style.border = '2px solid red';\n" +
		                                                 "return border;";

		private const string SCRIPT_UNHIGHLIGHT_ELEMENT = "var elem = arguments[0];\n" +
		                                                  "var borders = arguments[1].split(';');\n" +
		                                                  "elem.style.borderTop = borders[0];\n" +
		                                                  "elem.style.borderRight = borders[1];\n" +
		                                                  "elem.style.borderBottom = borders[2];\n" +
		                                                  "elem.style.borderLeft = borders[3];";

		public Highlight(IWebDriver driver)
		{
			m_driver = (IJavaScriptExecutor) driver;
		}

		public void HighlightElement(IWebElement elem)
		{
			ClearHighlight();

			// remember the new element
			m_lastElem   = elem;
			m_lastBorder = (string) (m_driver.ExecuteScript(SCRIPT_GET_ELEMENT_BORDER, elem));
		}

		/// <summary>
		/// Removes previously highlighted element.
		/// </summary>
		public void ClearHighlight()
		{
			if (m_lastElem != null) {
				try {
					// if there already is a highlighted element, unhighlight it
					m_driver.ExecuteScript(SCRIPT_UNHIGHLIGHT_ELEMENT, m_lastElem, m_lastBorder);
				}
				catch (StaleElementReferenceException) {
					// the page got reloaded, the element isn't there
				}
				finally {
					// element either restored or wasn't valid, nullify in both cases
					m_lastElem = null;
				}
			}
		}
	}
}