/* Console app that get weekly feed from SSI and tweet changes in national flu level.
 * Copyright(c) 2016 Mads Breusch Klinkby.
 * GPLv3 license: https://www.gnu.org/licenses/gpl-3.0.html
 */

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Klinkby.Flu")]
[assembly: AssemblyDescription("Console app that get weekly feed from SSI and tweet changes in national flu level.")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyProduct("Klinkby.Flu")]
[assembly: AssemblyCopyright("Copyright(c) 2016 Mads Breusch Klinkby. GPLv3 licensed")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
