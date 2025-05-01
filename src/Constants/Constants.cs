namespace N8.Constants
{
    // constants used throughout stuff
    public static class SecurityFlags
    {
        public const uint QueryInformation = 0x0400;
        public const uint LogonFlags = 0x00000001;
        public const uint CreationFlags = 0x00000010;
        public const int ImpersonationSecurity = 2; // Typically SECURITY_IMPERSONATION_LEVEL
    }

    public static class TokenPrivileges
    {
        public const uint AdjustPrivileges = 0x0020;
        public const uint Duplicate = 0x0002;
        public const uint AssignPrimary = 0x0001;
        public const uint Query = 0x0008;
        public const uint AdjustDefault = 0x0080;
        public const uint AdjustSessionID = 0x0100;
        public const int AssignPrimaryToken = 1; // Token type for primary tokens
        public const int SePrivilegeEnabled = 0x00000002;
    }
    public static class ApplicationInformation
    {
        public static string version = "v0.1";
    }

    public static class Messages
    {

        public static string welcomemessage = @"
                                            c=========e
                                                 H
   ____________                            _,,___H____
  (__((__((___()                          //|         |
 (__((__((___()()________________________// | detoN8  |
(__((__((___()()()-----------------------'  |_________|
";


    }
}