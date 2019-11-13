// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Server.IISIntegration;
using System;

namespace LeagueManager.IdentityServer
{
    public static class AccountOptions
    {
        public const bool AllowLocalLogin = true;
        public const bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public const bool ShowLogoutPrompt = true;
        public const bool AutomaticRedirectAfterSignOut = false;

        // specify the Windows authentication scheme being used
        public static string WindowsAuthenticationSchemeName = IISDefaults.AuthenticationScheme;
        // if user uses windows auth, should we load the groups from windows
        public const bool IncludeWindowsGroups = false;

        public const string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}