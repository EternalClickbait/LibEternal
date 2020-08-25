using System.Runtime.CompilerServices;

//So we can access internal stuff from our test class
[assembly: InternalsVisibleTo("LibEternal.Tests")]
//And our other LibEternal projects
[assembly: InternalsVisibleTo("LibEternal.Unity")]
[assembly: InternalsVisibleTo("LibEternal.Unity.Editor")]
