using LibEternal.JetBrains.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace LibEternal.Helper
{
	[PublicAPI]
	public static class NetworkHelper
	{
		/// <summary>
		///     Returns the current <see cref="IPAddress" />, or <see langword="null" /> if it cannot be found (no connection, no network adapter etc)
		/// </summary>
		/// <returns></returns>
		[CanBeNull]
		public static IPAddress GetCurrentIpSafe()
		{
			if (!NetworkInterface.GetIsNetworkAvailable()) return null;

			return Dns.GetHostEntry(Dns.GetHostName()).AddressList
				.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
		}

		/// <summary>
		///     Will return the current <see cref="IPAddress" />, or <see langword="throw" /> if it cannot be found (no connection, no network adapter etc)
		/// </summary>
		/// <returns></returns>
		[NotNull]
		public static IPAddress GetCurrentIp()
		{
			if (!NetworkInterface.GetIsNetworkAvailable()) throw new InvalidOperationException("Not connected to a network");

			return GetCurrentIpSafe() ?? throw new InvalidOperationException("Current IP not found");
		}

		/// <summary>
		///     Returns a new <see cref="IPEndPoint" /> set up to accept connections from any <see cref="IPAddress" /> on any port
		/// </summary>
		/// <returns></returns>
		[NotNull]
		public static IPEndPoint GetReceiveAllEndpoint()
		{
			return new IPEndPoint(IPAddress.Any, 0);
		}
	}
}