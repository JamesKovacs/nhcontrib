// Guids.cs
// MUST match guids.h
using System;

namespace FelicePollano.Hbm2NetAddIn
{
    static class GuidList
    {
        public const string guidHbm2NetAddInPkgString = "1282c7ab-5c1c-4935-ad2a-93e89147b82c";
        public const string guidHbm2NetAddInCmdSetString = "b53cd15c-7e3b-4d4d-9789-94ee4a165394";

        public static readonly Guid guidHbm2NetAddInCmdSet = new Guid(guidHbm2NetAddInCmdSetString);
    };
}