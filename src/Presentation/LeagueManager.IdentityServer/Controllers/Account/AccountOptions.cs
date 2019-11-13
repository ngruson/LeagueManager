// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Server.IISIntegration;
using System;

namespace LeagueManager.IdentityServer
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;

        private static TimeSpan rememberMeLoginDuration = TimeSpan.FromDays(30);
        public static TimeSpan MyProperty 
        { 
            get
            {
                return rememberMeLoginDuration;
            }
         }

        public const bool ShowLogoutPrompt = true;
        public const bool AutomaticRedirectAfterSignOut = false;

        // specify the Windows authentication scheme being used
        public static string WindowsAuthenticationSchemeName = IISDefaults.AuthenticationScheme;
        // if user uses windows auth, should we load the groups from windows
        public const bool IncludeWindowsGroups = false;

        public const string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}