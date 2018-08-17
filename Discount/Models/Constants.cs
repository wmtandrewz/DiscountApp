using System;
using System.Collections.Generic;

namespace Discount.Models
{
    public static class Constants
    {
        //DEV URL 
        public static string _gatewayURL = "https://nwgateway.keells.lk:44300";
        public static string _cookie = "sap-XSRF_NWG_100";

        public static string _azureAPIMDEVBase = "https://jkhapimdev.azure-api.net/api/";

        //Access Token for gateway services
        public static string _access_token = "";
        public static string _expires_in = "";

        //User Login Details
        public static User _user;

        //Hotel Information variables
        public static string _hotel_code = "CNG";
        public static string _hotel_number = "3000";
        public static string _hotel_name = "Cinnamon Grand";
        public static string[] _hotelList = 
            { 
                "Cinnamon Grand",
                "Cinnamon Lakeside",
                "Cinnamon Red",
                "Cinnamon Bey",
                "Hikka Tranz by Cinnamon",
                "Cinnamon Wild",
                "Cinnamon Lodge",
                "Trinco Blu by Cinnamon",
                "Cinnamon Citadel",
                "Habarana Village by Cinnamon",
                "Bentota Beach by Cinnamon",
                "Ellaidhoo Maldives by Cinnamon",
                "Cinnamon Dhonveli",
                "Cinnamon Hakuraa Huraa"
            };

        public static List<string> StatusTypesList = new List<string>()
                {
                    "Pending",
                    "Approved",
                    "Rejected"
                };

        //Selected discound details
        public static DiscountHeaderModel _selectedDiscountHeader = null;
        public static DiscountGroupModel _selectedDiscountGroup = null;

        //Messages
        public static string _userNotExistInNWGateway = "Authentication Successful. Login Unsuccessful due to unavailability of username in SAP system. Please contact your authorization team.";
        public static string _gatewayUrlError = "Unable to Connect. This error may occur if the gateway URL is incorrect!";

    }
}
