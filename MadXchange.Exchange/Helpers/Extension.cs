using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System.Net.WebSockets;
using ServiceStack;
using MadXchange.Exchange.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MadXchange.Exchange.Domain.Models;

namespace MadXchange.Exchange.Helpers
{
    public static class Extension
    {
        /// <summary>
        /// Length of 1 based arrays
        /// </summary>
        /// <param name="descriptors"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(this XchangeDescriptor[] descriptors) => descriptors.Length - 1;
        
        /// <summary>
        /// Counts the http endpoints for a exchange descriptor 
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountEndpoints(this XchangeDescriptor descriptor) => descriptor.EndPoints.Length - 1;
        /// <summary>
        /// Used to access the correct endpoint within a XachangeDescriptor
        /// internally data is mapped on an arry, key is enum with 0 as unknown/unspecified value => arrays are 1 based
        /// </summary>
        /// <param name="endPoints"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EndPoint GetEndPoint(this EndPoint[] endPoints, XchangeHttpOperation operation) => endPoints[(int)operation];
        
        /// <summary>
        /// returns an adhoc Id to garanty key property when accessing a connection of a websocket
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetID(this WebSocket socket) => (int)socket.GetId();
  
      


    }
}