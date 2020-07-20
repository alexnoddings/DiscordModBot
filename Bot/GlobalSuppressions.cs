using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Localisation is currently out-of-scope for this project.", Scope = "namespaceanddescendants", Target = "Elvet")]

[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Not necessary for migrations.", Scope = "namespaceanddescendants", Target = "Elvet.Data.Migrations")]