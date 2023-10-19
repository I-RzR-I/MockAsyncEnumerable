// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2023-04-28 12:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-04-28 14:08
// ***********************************************************************
//  <copyright file="GeneralAssemblyInfo.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Reflection;
using System.Resources;

#endregion

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("RzR ®")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("Copyright © 2022-2023 RzR All rights reserved.")]
[assembly: AssemblyTrademark("® RzR™")]
[assembly: AssemblyDescription("A simple implementation for a list of data transforms to async enumerable, allowing the use of paginated grid results or any action or dynamic aggregated query using EF Core (`Microsoft.EntityFrameworkCore`) through Expressions (`System.Linq.Expressions`)")]

[assembly: AssemblyMetadata("TermsOfService", "")]
[assembly: AssemblyMetadata("ContactUrl", "")]
[assembly: AssemblyMetadata("ContactName", "RzR")]
[assembly: AssemblyMetadata("ContactEmail", "ddpRzR@hotmail.com")]
[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]
[assembly: AssemblyVersion("1.0.1.1250")]
[assembly: AssemblyFileVersion("1.0.1.1250")]
[assembly: AssemblyInformationalVersion("1.0.1.x")]