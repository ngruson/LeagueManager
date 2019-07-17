using LeagueManager.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace LeagueManager.Persistence.EntityFramework
{
    public class DbInitializer
    {
        private readonly string environment;
        private readonly IConfiguration configuration;
        private readonly IImageFileLoader imageFileLoader;

        public DbInitializer(
            IConfiguration configuration,
            IImageFileLoader imageFileLoader)
        {
            this.configuration = configuration;
            environment = configuration["ASPNETCORE_ENVIRONMENT"];
            this.imageFileLoader = imageFileLoader;
        }

        public void Initialize(LeagueManagerDbContext context)
        {
            SeedEverything(context);
        }

        private void SeedEverything(LeagueManagerDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Countries.Any())
            {
                SeedCountries(context);
            }

            if (environment == "Development")
            {
                if (!context.Teams.Any())
                    SeedTeams(context);
            }
        }        

        //Got countries from https://datahub.io/core/country-codes
        private void SeedCountries(LeagueManagerDbContext context)
        {
            var countries = new Country[]
            {
                new Country { Name = "Taiwan", Code = "TPE" },
                new Country { Name = "Afghanistan", Code = "AFG" },
                new Country { Name = "Albania", Code = "ALB" },
                new Country { Name = "Algeria", Code = "ALG" },
                new Country { Name = "American Samoa", Code = "ASA" },
                new Country { Name = "Andorra", Code = "AND" },
                new Country { Name = "Angola", Code = "ANG" },
                new Country { Name = "Anguilla", Code = "AIA" },
                new Country { Name = "Antarctica", Code = "ROS" },
                new Country { Name = "Antigua & Barbuda", Code = "ATG" },
                new Country { Name = "Argentina", Code = "ARG" },
                new Country { Name = "Armenia", Code = "ARM" },
                new Country { Name = "Aruba", Code = "ARU" },
                new Country { Name = "Australia", Code = "AUS" },
                new Country { Name = "Austria", Code = "AUT" },
                new Country { Name = "Azerbaijan", Code = "AZE" },
                new Country { Name = "Bahamas", Code = "BAH" },
                new Country { Name = "Bahrain", Code = "BHR" },
                new Country { Name = "Bangladesh", Code = "BAN" },
                new Country { Name = "Barbados", Code = "BRB" },
                new Country { Name = "Belarus", Code = "BLR" },
                new Country { Name = "Belgium", Code = "BEL" },
                new Country { Name = "Belize", Code = "BLZ" },
                new Country { Name = "Benin", Code = "BEN" },
                new Country { Name = "Bermuda", Code = "BER" },
                new Country { Name = "Bhutan", Code = "BHU" },
                new Country { Name = "Bolivia", Code = "BOL" },
                new Country { Name = "Caribbean Netherlands", Code = "ANT" },
                new Country { Name = "Bosnia", Code = "BIH" },
                new Country { Name = "Botswana", Code = "BOT" },
                new Country { Name = "Bouvet Island", Code = "" },
                new Country { Name = "Brazil", Code = "BRA" },
                new Country { Name = "British Indian Ocean Territory", Code = "" },
                new Country { Name = "British Virgin Islands", Code = "VGB" },
                new Country { Name = "Brunei", Code = "BRU" },
                new Country { Name = "Bulgaria", Code = "BUL" },
                new Country { Name = "Burkina Faso", Code = "BFA" },
                new Country { Name = "Burundi", Code = "BDI" },
                new Country { Name = "Cape Verde", Code = "CPV" },
                new Country { Name = "Cambodia", Code = "CAM" },
                new Country { Name = "Cameroon", Code = "CMR" },
                new Country { Name = "Canada", Code = "CAN" },
                new Country { Name = "Cayman Islands", Code = "CAY" },
                new Country { Name = "Central African Republic", Code = "CTA" },
                new Country { Name = "Chad", Code = "CHA" },
                new Country { Name = "Chile", Code = "CHI" },
                new Country { Name = "China", Code = "CHN" },
                new Country { Name = "Hong Kong", Code = "HKG" },
                new Country { Name = "Macau", Code = "MAC" },
                new Country { Name = "Christmas Island", Code = "CXR" },
                new Country { Name = "Cocos (Keeling) Islands", Code = "CCK" },
                new Country { Name = "Colombia", Code = "COL" },
                new Country { Name = "Comoros", Code = "COM" },
                new Country { Name = "Congo - Brazzaville", Code = "CGO" },
                new Country { Name = "Cook Islands", Code = "COK" },
                new Country { Name = "Costa Rica", Code = "CRC" },
                new Country { Name = "Croatia", Code = "CRO" },
                new Country { Name = "Cuba", Code = "CUB" },
                new Country { Name = "Curaçao", Code = "" },
                new Country { Name = "Cyprus", Code = "CYP" },
                new Country { Name = "Czechia", Code = "CZE" },
                new Country { Name = "Côte d’Ivoire", Code = "CIV" },
                new Country { Name = "North Korea", Code = "PRK" },
                new Country { Name = "Congo - Kinshasa", Code = "COD" },
                new Country { Name = "Denmark", Code = "DEN" },
                new Country { Name = "Djibouti", Code = "DJI" },
                new Country { Name = "Dominica", Code = "DMA" },
                new Country { Name = "Dominican Republic", Code = "DOM" },
                new Country { Name = "Ecuador", Code = "ECU" },
                new Country { Name = "Egypt", Code = "EGY" },
                new Country { Name = "El Salvador", Code = "SLV" },
                new Country { Name = "Equatorial Guinea", Code = "EQG" },
                new Country { Name = "Eritrea", Code = "ERI" },
                new Country { Name = "Estonia", Code = "EST" },
                new Country { Name = "Ethiopia", Code = "ETH" },
                new Country { Name = "Falkland Islands", Code = "FLK" },
                new Country { Name = "Faroe Islands", Code = "FRO" },
                new Country { Name = "Fiji", Code = "FIJ" },
                new Country { Name = "Finland", Code = "FIN" },
                new Country { Name = "France", Code = "FRA" },
                new Country { Name = "French Guiana", Code = "GUF" },
                new Country { Name = "French Polynesia", Code = "TAH" },
                new Country { Name = "French Southern Territories", Code = "" },
                new Country { Name = "Gabon", Code = "GAB" },
                new Country { Name = "Gambia", Code = "GAM" },
                new Country { Name = "Georgia", Code = "GEO" },
                new Country { Name = "Germany", Code = "GER" },
                new Country { Name = "Ghana", Code = "GHA" },
                new Country { Name = "Gibraltar", Code = "GBZ" },
                new Country { Name = "Greece", Code = "GRE" },
                new Country { Name = "Greenland", Code = "GRL" },
                new Country { Name = "Grenada", Code = "GRN" },
                new Country { Name = "Guadeloupe", Code = "GLP" },
                new Country { Name = "Guam", Code = "GUM" },
                new Country { Name = "Guatemala", Code = "GUA" },
                new Country { Name = "Guernsey", Code = "GBG" },
                new Country { Name = "Guinea", Code = "GUI" },
                new Country { Name = "Guinea-Bissau", Code = "GNB" },
                new Country { Name = "Guyana", Code = "GUY" },
                new Country { Name = "Haiti", Code = "HAI" },
                new Country { Name = "Heard & McDonald Islands", Code = "" },
                new Country { Name = "Vatican City", Code = "VAT" },
                new Country { Name = "Honduras", Code = "HON" },
                new Country { Name = "Hungary", Code = "HUN" },
                new Country { Name = "Iceland", Code = "ISL" },
                new Country { Name = "India", Code = "IND" },
                new Country { Name = "Indonesia", Code = "IDN" },
                new Country { Name = "Iran", Code = "IRN" },
                new Country { Name = "Iraq", Code = "IRQ" },
                new Country { Name = "Ireland", Code = "IRL" },
                new Country { Name = "Isle of Man", Code = "GBM" },
                new Country { Name = "Israel", Code = "ISR" },
                new Country { Name = "Italy", Code = "ITA" },
                new Country { Name = "Jamaica", Code = "JAM" },
                new Country { Name = "Japan", Code = "JPN" },
                new Country { Name = "Jersey", Code = "GBJ" },
                new Country { Name = "Jordan", Code = "JOR" },
                new Country { Name = "Kazakhstan", Code = "KAZ" },
                new Country { Name = "Kenya", Code = "KEN" },
                new Country { Name = "Kiribati", Code = "KIR" },
                new Country { Name = "Kuwait", Code = "KUW" },
                new Country { Name = "Kyrgyzstan", Code = "KGZ" },
                new Country { Name = "Laos", Code = "LAO" },
                new Country { Name = "Latvia", Code = "LVA" },
                new Country { Name = "Lebanon", Code = "LIB" },
                new Country { Name = "Lesotho", Code = "LES" },
                new Country { Name = "Liberia", Code = "LBR" },
                new Country { Name = "Libya", Code = "LBY" },
                new Country { Name = "Liechtenstein", Code = "LIE" },
                new Country { Name = "Lithuania", Code = "LTU" },
                new Country { Name = "Luxembourg", Code = "LUX" },
                new Country { Name = "Madagascar", Code = "MAD" },
                new Country { Name = "Malawi", Code = "MWI" },
                new Country { Name = "Malaysia", Code = "MAS" },
                new Country { Name = "Maldives", Code = "MDV" },
                new Country { Name = "Mali", Code = "MLI" },
                new Country { Name = "Malta", Code = "MLT" },
                new Country { Name = "Marshall Islands", Code = "MHL" },
                new Country { Name = "Martinique", Code = "MTQ" },
                new Country { Name = "Mauritania", Code = "MTN" },
                new Country { Name = "Mauritius", Code = "MRI" },
                new Country { Name = "Mayotte", Code = "MYT" },
                new Country { Name = "Mexico", Code = "MEX" },
                new Country { Name = "Micronesia", Code = "FSM" },
                new Country { Name = "Monaco", Code = "MON" },
                new Country { Name = "Mongolia", Code = "MNG" },
                new Country { Name = "Montenegro", Code = "MNE" },
                new Country { Name = "Montserrat", Code = "MSR" },
                new Country { Name = "Morocco", Code = "MAR" },
                new Country { Name = "Mozambique", Code = "MOZ" },
                new Country { Name = "Myanmar", Code = "MYA" },
                new Country { Name = "Namibia", Code = "NAM" },
                new Country { Name = "Nauru", Code = "NRU" },
                new Country { Name = "Nepal", Code = "NEP" },
                new Country { Name = "Netherlands", Code = "NED", Flag = imageFileLoader.LoadImage("nl.png") },
                new Country { Name = "New Caledonia", Code = "NCL" },
                new Country { Name = "New Zealand", Code = "NZL" },
                new Country { Name = "Nicaragua", Code = "NCA" },
                new Country { Name = "Niger", Code = "NIG" },
                new Country { Name = "Nigeria", Code = "NGA" },
                new Country { Name = "Niue", Code = "NIU" },
                new Country { Name = "Norfolk Island", Code = "NFK" },
                new Country { Name = "Northern Mariana Islands", Code = "NMI" },
                new Country { Name = "Norway", Code = "NOR" },
                new Country { Name = "Oman", Code = "OMA" },
                new Country { Name = "Pakistan", Code = "PAK" },
                new Country { Name = "Palau", Code = "PLW" },
                new Country { Name = "Panama", Code = "PAN" },
                new Country { Name = "Papua New Guinea", Code = "PNG" },
                new Country { Name = "Paraguay", Code = "PAR" },
                new Country { Name = "Peru", Code = "PER" },
                new Country { Name = "Philippines", Code = "PHI" },
                new Country { Name = "Pitcairn Islands", Code = "PCN" },
                new Country { Name = "Poland", Code = "POL" },
                new Country { Name = "Portugal", Code = "POR" },
                new Country { Name = "Puerto Rico", Code = "PUR" },
                new Country { Name = "Qatar", Code = "QAT" },
                new Country { Name = "South Korea", Code = "KOR" },
                new Country { Name = "Moldova", Code = "MDA" },
                new Country { Name = "Romania", Code = "ROU" },
                new Country { Name = "Russia", Code = "RUS" },
                new Country { Name = "Rwanda", Code = "RWA" },
                new Country { Name = "Réunion", Code = "REU" },
                new Country { Name = "St. Barthélemy", Code = "" },
                new Country { Name = "St. Helena", Code = "SHN" },
                new Country { Name = "St. Kitts & Nevis", Code = "SKN" },
                new Country { Name = "St. Lucia", Code = "LCA" },
                new Country { Name = "St. Martin", Code = "" },
                new Country { Name = "St. Pierre & Miquelon", Code = "SPM" },
                new Country { Name = "St. Vincent & Grenadines", Code = "VIN" },
                new Country { Name = "Samoa", Code = "SAM" },
                new Country { Name = "San Marino", Code = "SMR" },
                new Country { Name = "São Tomé & Príncipe", Code = "STP" },
                new Country { Name = "Saudi Arabia", Code = "KSA" },
                new Country { Name = "Senegal", Code = "SEN" },
                new Country { Name = "Serbia", Code = "SRB" },
                new Country { Name = "Seychelles", Code = "SEY" },
                new Country { Name = "Sierra Leone", Code = "SLE" },
                new Country { Name = "Singapore", Code = "SIN" },
                new Country { Name = "Sint Maarten", Code = "" },
                new Country { Name = "Slovakia", Code = "SVK" },
                new Country { Name = "Slovenia", Code = "SVN" },
                new Country { Name = "Solomon Islands", Code = "SOL" },
                new Country { Name = "Somalia", Code = "SOM" },
                new Country { Name = "South Africa", Code = "RSA" },
                new Country { Name = "South Georgia & South Sandwich Islands", Code = "" },
                new Country { Name = "South Sudan", Code = "" },
                new Country { Name = "Spain", Code = "ESP" },
                new Country { Name = "Sri Lanka", Code = "SRI" },
                new Country { Name = "Palestine", Code = "PLE" },
                new Country { Name = "Sudan", Code = "SUD" },
                new Country { Name = "Suriname", Code = "SUR" },
                new Country { Name = "Svalbard & Jan Mayen", Code = "" },
                new Country { Name = "Swaziland", Code = "SWZ" },
                new Country { Name = "Sweden", Code = "SWE" },
                new Country { Name = "Switzerland", Code = "SUI" },
                new Country { Name = "Syria", Code = "SYR" },
                new Country { Name = "Tajikistan", Code = "TJK" },
                new Country { Name = "Thailand", Code = "THA" },
                new Country { Name = "Macedonia", Code = "MKD" },
                new Country { Name = "Timor-Leste", Code = "TLS" },
                new Country { Name = "Togo", Code = "TOG" },
                new Country { Name = "Tokelau", Code = "TKL" },
                new Country { Name = "Tonga", Code = "TGA" },
                new Country { Name = "Trinidad & Tobago", Code = "TRI" },
                new Country { Name = "Tunisia", Code = "TUN" },
                new Country { Name = "Turkey", Code = "TUR" },
                new Country { Name = "Turkmenistan", Code = "TKM" },
                new Country { Name = "Turks & Caicos Islands", Code = "TCA" },
                new Country { Name = "Tuvalu", Code = "TUV" },
                new Country { Name = "Uganda", Code = "UGA" },
                new Country { Name = "Ukraine", Code = "UKR" },
                new Country { Name = "United Arab Emirates", Code = "UAE" },
                new Country { Name = "England", Code = "ENG" },
                new Country { Name = "Northern Ireland", Code = "NIR" },
                new Country { Name = "Scotland", Code = "SCO" },
                new Country { Name = "Wales", Code = "WAL" },
                new Country { Name = "Tanzania", Code = "TAN" },
                new Country { Name = "U.S. Outlying Islands", Code = "" },
                new Country { Name = "U.S. Virgin Islands", Code = "VIR" },
                new Country { Name = "US", Code = "USA" },
                new Country { Name = "Uruguay", Code = "URU" },
                new Country { Name = "Uzbekistan", Code = "UZB" },
                new Country { Name = "Vanuatu", Code = "VAN" },
                new Country { Name = "Venezuela", Code = "VEN" },
                new Country { Name = "Vietnam", Code = "VIE" },
                new Country { Name = "Wallis & Futuna", Code = "WLF" },
                new Country { Name = "Western Sahara", Code = "SAH" },
                new Country { Name = "Yemen", Code = "YEM" },
                new Country { Name = "Zambia", Code = "ZAM" },
                new Country { Name = "Zimbabwe", Code = "ZIM" },
                new Country { Name = "Åland Islands", Code = "ALD" }
            };

            context.Countries.AddRange(countries);
            context.SaveChanges();
        }

        private void SeedTeams(LeagueManagerDbContext context)
        {
            SeedTeamsNL(context);
        }

        private void SeedTeamsNL(LeagueManagerDbContext context)
        {
            var country = context.Countries.SingleOrDefault(c => c.Name == "Netherlands");

            var teams = new Team[]
            {
                new Team { Name = "ADO Den Haag", Country = country },
                new Team { Name = "AZ", Country = country },
                new Team { Name = "Ajax", Country = country },
                new Team { Name = "FC Emmen", Country = country },
                new Team { Name = "Feyenoord", Country = country },
                new Team { Name = "Fortuna Sittard", Country = country },
                new Team { Name = "FC Groningen", Country = country },
                new Team { Name = "SC Heerenveen", Country = country },
                new Team { Name = "Heracles Almelo", Country = country },
                new Team { Name = "PEC Zwolle", Country = country },
                new Team { Name = "PSV", Country = country },
                new Team { Name = "RKC Waalwijk", Country = country },
                new Team { Name = "Sparta Rotterdam", Country = country },
                new Team { Name = "FC Twente", Country = country },
                new Team { Name = "FC Utrecht", Country = country },
                new Team { Name = "VVV Venlo", Country = country },
                new Team { Name = "Vitesse", Country = country },
                new Team { Name = "Willem II", Country = country },
            };

            context.Teams.AddRange(teams);
            context.SaveChanges();
        }
    }
}