#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

#endregion

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle(@"")]
[assembly: AssemblyDescription(@"")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(@"NHibernate")]
[assembly: AssemblyProduct(@"NHDesigner")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: System.Resources.NeutralResourcesLanguage("en")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly: AssemblyVersion(@"1.0.0.0")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]
[assembly: ReliabilityContract(Consistency.MayCorruptProcess, Cer.None)]

//
// Make the Dsl project internally visible to the DslPackage assembly
//
[assembly: InternalsVisibleTo(@"NHibernate.NHDesigner.DslPackage, PublicKey=002400000480000094000000060200000024000052534131000400000100010017C5BFD590100CE54DD1C04BEBDAAAACC26BC008AA04785BB3E97586F89828B6E8586D5C25FD8D9E057C8D95F58E536A9ADBF32043D326330BE9BFCE5740998A00F8C6C2674E104D6159FF24BD907C6DBFCE2B5229A64D113925781E7F058A5EFE97A60A3F3AD31D14AF5E35BFD0D6B36D79DC3D43040FAF851D3F3657CBD7BD")]