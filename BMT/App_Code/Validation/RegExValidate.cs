using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

using BMTBLL.Enumeration;

namespace BMT.Validation
{
    public class RegExValidate
    {
        #region CONSTANTS
        public const string POS_NUMBER_EXPRESSION = @"^[0-9][0-9]*(\.[0-9]*)?$";

        public const string NUM_EXPRESSION = @"^(\+|-)?[0-9][0-9]*(\.[0-9]*)?$";

        public const string POS_NON_DECIMAL_EXPRESSION = @"^[0-9][0-9]*$";

        public const string NON_DECIMAL_EXPRESSION = @"^(\+|-)?[0-9][0-9]*(\.[0-9]*)?$";

        public const string DATES_DD_MMM_YYYY_EXPRESSION = @"^((31(?!\ (Feb(ruary)?|Apr(il)?|June?|
		(Sep(?=\b|t)t?|Nov)(ember)?)))|
	 	((30|29)(?!\ Feb(ruary)?))|(29(?=\ 
		Feb(ruary)?\ (((1[6-9]|[2-9]\d)(0[48]|
		[2468][048]|[13579][26])|((16|[2468][048]|
		[3579][26])00)))))|(0?[1-9])|1\d|2[0-8])\ 
		(Jan(uary)?|Feb(ruary)?|Ma(r(ch)?|y)|Apr(il)?|
		Ju((ly?)|(ne?))|Aug(ust)?|
		Oct(ober)?|(Sep(?=\b|t)t?|Nov|Dec)(ember)?)\ 
		((1[6-9]|[2-9]\d)\d{2})";

        public const string DATE_DDMMYY_EXPRESSION = @"((0[1-9]|[12][0-9]|3[01]))[/|-](0[1-9]|1[0-2])[/|-]((?:\d{2}))";

        public const string DATE_DDMMYYYY_EXPRESSION = @"((0[1-9]|[12][0-9]|3[01]))[/|-](0[1-9]|1[0-2])[/|-]((?:\d{4}|\d{2}))";

        public const string EMAIL_EXPRESSION = @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|
		(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})";

        public const string IP_EXPRESSION = @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>
		2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>
		2[0-4]\d|25[0-5]|[01]?\d\d?)\.
		(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)";

        public const string URL_EPXRESSION = @"(?<protocol />\w+):\/\/(?<Domain>[\w.]+\/?)\S*";

        public const string CUSTOM_CHARACTER1 = @"^[a-zA-Z0-9\s\-\#\\/\&\.\,\'\(\)]{1,50}$";

        #endregion

        #region VARIABLES
        static ArrayList RegexStrings;

        #endregion

        #region CONSTRUCTOR
        static RegExValidate()
        {
            RegexStrings = new ArrayList();

            //PositiveNumber
            RegexStrings.Insert((int)ValidationTypes.PositiveNumber,
            POS_NUMBER_EXPRESSION);

            //Number
            RegexStrings.Insert((int)ValidationTypes.Number,
             NUM_EXPRESSION);

            //PositiveNonDecimal
            RegexStrings.Insert((int)ValidationTypes.PositiveNonDecimal,
             POS_NON_DECIMAL_EXPRESSION);

            //NonDecimal
            RegexStrings.Insert((int)ValidationTypes.NonDecimal,
             NON_DECIMAL_EXPRESSION);

            //Dates(DD/MM/YYYY)
            RegexStrings.Insert((int)ValidationTypes.DatesDDMMYYYY,
            DATE_DDMMYYYY_EXPRESSION);

            //Dates(DD/MM/YY)
            RegexStrings.Insert((int)ValidationTypes.DatesDDMMYY,
            DATE_DDMMYY_EXPRESSION);

            //Dates(DD MMM YYYY)
            RegexStrings.Insert((int)ValidationTypes.DatesDD_MMM_YYYY,
             DATES_DD_MMM_YYYY_EXPRESSION);

            //Email
            RegexStrings.Insert((int)ValidationTypes.Email,
              @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|
		(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})");

            //IPAddress
            RegexStrings.Insert((int)ValidationTypes.IPAddress,
              @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>
		2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>
		2[0-4]\d|25[0-5]|[01]?\d\d?)\.
		(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)");

            //URL
            RegexStrings.Insert((int)ValidationTypes.URL,
              @"(?<protocol />\w+):\/\/(?<Domain>[\w.]+\/?)\S*");

            //Custom Character1
            RegexStrings.Insert((int)ValidationTypes.CustomCharacter1,
              CUSTOM_CHARACTER1);

        }

        #endregion

        #region FUNCTIONS
        public static bool Process(ValidationTypes type, string unknown)
        {
            string receivedType = RegexStrings[(int)type].ToString();

            // Create a new Regex object.
            Regex expressionAgainstType = new Regex(RegexStrings[(int)type].ToString());

            // Find a single match in the string.
            Match checkMatch = expressionAgainstType.Match(unknown);

            return checkMatch.Success;
        }

        #endregion
    }
}