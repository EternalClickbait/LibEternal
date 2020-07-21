using LibEternal.JetBrains.Annotations;
using System;

namespace LibEternal.Unity
{
	/// <summary>
	///     An attribute used to mark methods as callable from the unity inspector window
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	[MeansImplicitUse]
	// ReSharper disable once ClassNeverInstantiated.Global
	public sealed class InspectorButtonAttribute : Attribute
	{
	}
}